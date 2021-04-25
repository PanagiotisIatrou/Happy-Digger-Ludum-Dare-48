using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private FuelManager fuelManager;
    private MoneyManager moneyManager;
    private Inventory inventory;
    private float forceSpeed = 1000f;
    private float maxSpeed = 5f;
    private Rigidbody2D rb;

    private bool isInMiningState = false;
    private bool isDirLeft = true;
    private bool isMiningDown = false;
    private float switchTime = 0.07f;
    private Coroutine switchDirCoroutine;
    private Transform spriteTR;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fuelManager = GetComponent<FuelManager>();
        moneyManager = GetComponent<MoneyManager>();
        inventory = GetComponent<Inventory>();
        spriteTR = transform.GetChild(0);
    }

    private void Update()
    {
        // Check for fuel consumption
        if (isInMiningState || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            fuelManager.DecreaseFuel(Time.deltaTime / 3);
        }

        // Sell inventory
        if (Input.GetKeyDown(KeyCode.G))
        {
            int silverOres = inventory.GetNumberOfOresWithName("Silver");
            int goldOres = inventory.GetNumberOfOresWithName("Gold");
            moneyManager.AddMoney(5 * silverOres);
            moneyManager.AddMoney(20 * goldOres);

            inventory.RemoveAllOres();
        }

        // Check for mining state
        if (!isInMiningState && rb.velocity.magnitude == 0)
        {
            // Get player tile position
            Vector2Int pos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

            // Get direction to see if player wants to mine
            if (Input.GetKey(KeyCode.A) && isDirLeft && GroundManager.ExistsTileInPosition(pos + new Vector2Int(-1, 0)))
            {
                StartMining(pos, Vector2Int.left);
            }
            if (Input.GetKey(KeyCode.D) && !isDirLeft && GroundManager.ExistsTileInPosition(pos + new Vector2Int(1, 0)))
            {
                StartMining(pos, Vector2Int.right);
            }
            if (Input.GetKey(KeyCode.S) && GroundManager.ExistsTileInPosition(pos + new Vector2Int(0, -1)))
            {
                StartMining(pos, Vector2Int.down);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isInMiningState)
            return;

        if (Input.GetKey(KeyCode.A))
        {
            if (!isDirLeft)
                SwitchDirection();
            rb.AddForce(new Vector2(-forceSpeed * Time.fixedDeltaTime, 0f));
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (isDirLeft)
                SwitchDirection();
            rb.AddForce(new Vector2(forceSpeed * Time.fixedDeltaTime, 0f));
        }

        // Clamp speed
        if (rb.velocity.magnitude >= maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    private void SwitchDirection()
    {
        isDirLeft = !isDirLeft;
        if (switchDirCoroutine != null)
            StopCoroutine(switchDirCoroutine);
        switchDirCoroutine = StartCoroutine(IETurnToDirection(isDirLeft));
    }

    private IEnumerator IETurnToDirection(bool isDirLeft)
    {
        if (isDirLeft)
        {
            while (spriteTR.localScale.x <= 1f)
            {
                spriteTR.localScale = new Vector3(spriteTR.localScale.x + Time.deltaTime * 2 * (1 / switchTime), 1f, 1f);
                yield return null;
            }
            spriteTR.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            while (spriteTR.localScale.x >= -1f)
            {
                spriteTR.localScale = new Vector3(spriteTR.localScale.x - Time.deltaTime * 2 * (1 / switchTime), 1f, 1f);
                yield return null;
            }
            spriteTR.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void SetMiningDownDrillState(bool state)
    {
        isMiningDown = state;
        Transform graphics = transform.GetChild(0);
        graphics.GetChild(0).gameObject.SetActive(!state);
        graphics.GetChild(1).gameObject.SetActive(state);
    }

    private IEnumerator IEStartMining(Vector2Int startPos, Vector2Int direction)
    {
        Vector2 playerStartPos = transform.position;
        Vector2Int targetPos = startPos + direction;
        GroundManager.AddMinedTile(targetPos);

        Vector2 targetPosFloats = targetPos;
        Vector2 diff = targetPos - playerStartPos;
        float totalDist = diff.magnitude;
        Vector2 offset = diff / totalDist;

        // Temorarilly disable player physics
        GetComponent<CircleCollider2D>().enabled = false;
        rb.isKinematic = true;

        // Enable dirt particle emmision
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Play();
        transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ParticleSystem>().Play();

        // Move the player
        float time = 0f;
        CameraShaker.Instance.ShakeOnce(0.5f, 3, 1f, 1f);
        while (time < 1f)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, -2);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosFloats;

        // Destroy mined tile
        GroundManager.DestroyTileInPosition(startPos + direction);

        // Reenable player physics
        GetComponent<CircleCollider2D>().enabled = true;
        rb.isKinematic = false;

        // Disable dirt particle emmision
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Stop();
        transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<ParticleSystem>().Stop();

        isInMiningState = false;
        SetMiningDownDrillState(false);
    }

    private void StartMining(Vector2Int startPos, Vector2Int direction)
    {
        if (isInMiningState)
            return;

        isInMiningState = true;
        if (direction == Vector2Int.down)
            SetMiningDownDrillState(true);

        StartCoroutine(IEStartMining(startPos, direction));
    }
}
