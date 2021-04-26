using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private FuelManager fuelManager;
    private static float forceSpeed = 1000f;
    private static float maxSpeed = 5f;
    private static float diggingTime = 1f;
    private Rigidbody2D rb;

    private bool isInMiningState = false;
    private bool isFlying = false;
    private bool isDirLeft = true;
    private float switchTime = 0.07f;
    private Coroutine switchDirCoroutine;
    private Transform spriteTR;

    private Animator movingAnim;
    private Animator horizontalDrillAnim;
    private Animator downDrillAnim;
    private GameObject horizontalDrillGO;
    private GameObject downDrillGO;
    private GameObject thrusterGO;
    private Animator thrusterAnim;

    private AudioSource digSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fuelManager = GetComponent<FuelManager>();
        spriteTR = transform.GetChild(0);
        movingAnim = spriteTR.GetComponent<Animator>();
        horizontalDrillAnim = spriteTR.GetChild(0).GetComponent<Animator>();
        downDrillAnim = spriteTR.GetChild(1).GetComponent<Animator>();
        horizontalDrillGO = spriteTR.GetChild(0).gameObject;
        downDrillGO = spriteTR.GetChild(1).gameObject;
        thrusterGO = spriteTR.GetChild(2).gameObject;
        thrusterAnim = thrusterGO.GetComponent<Animator>();
        digSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (UpgradesStation.IsShopOpen())
            return;

        // Check for fuel consumption
        if (isInMiningState || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            fuelManager.DecreaseFuel(Time.deltaTime / 3);
        }

        if (!isFlying && IsTouchingGround() && (isInMiningState || rb.velocity.magnitude > 0.5f))
            movingAnim.SetBool("isMoving", true);
        else
            movingAnim.SetBool("isMoving", false);

        if (isFlying && IsTouchingGround())
        {
            isFlying = false;
            UseHorizontalDrill();
        }

        // Check for mining state
        if (!isInMiningState && !isFlying && rb.velocity.magnitude == 0)
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
        if (isInMiningState || UpgradesStation.IsShopOpen())
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
        if (Input.GetKeyDown(KeyCode.W))
        {
            CameraShaker.Instance.StartShake(0.3f, 3, 0f);
        }
        if (!Input.GetKey(KeyCode.W)) // Just checking for Input.GetKeyUp(KeyCode.W) rarely misses a shake
        {
            thrusterAnim.SetBool("isThrusting", false);
            foreach (CameraShakeInstance shake in CameraShaker.Instance.ShakeInstances)
                shake.StartFadeOut(0f);
            CameraShaker.Instance.ShakeInstances.Clear();
        }
        if (Input.GetKey(KeyCode.W))
        {
            isFlying = true;
            UseThruster();
            thrusterAnim.SetBool("isThrusting", true);
            rb.AddForce(new Vector2(0f, forceSpeed * Time.fixedDeltaTime * 0.8f));
        }

        // Clamp speed
        if (rb.velocity.magnitude >= maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private bool IsTouchingGround()
    {
        return Physics2D.Raycast(thrusterGO.transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Default")).distance == 0;
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

    private void UseHorizontalDrill()
    {
        horizontalDrillGO.gameObject.SetActive(true);
        downDrillGO.gameObject.SetActive(false);
        thrusterGO.SetActive(false);
    }

    private void UseDownDrill()
    {
        downDrillGO.gameObject.SetActive(true);
        horizontalDrillGO.gameObject.SetActive(false);
        thrusterGO.SetActive(false);
    }

    private void UseThruster()
    {
        thrusterGO.SetActive(true);
        downDrillGO.gameObject.SetActive(false);
        horizontalDrillGO.gameObject.SetActive(false);
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
        horizontalDrillGO.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        downDrillGO.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        // Enable drill animation
        horizontalDrillAnim.SetTrigger("StartMining");
        downDrillAnim.SetTrigger("StartMining");

        // Play SFX
        float initialLength = digSource.clip.length;
        digSource.pitch = 1f / initialLength / 1.5f * (1f / diggingTime);
        digSource.Play();

        // Move the player
        float time = 0f;
        CameraShaker.Instance.ShakeOnce(0.5f, 3, 1f, 1f);
        while (time < diggingTime)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * (1f / diggingTime));
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
        horizontalDrillGO.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        downDrillGO.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();

        // Enable drill animation
        horizontalDrillAnim.SetTrigger("StopMining");
        downDrillAnim.SetTrigger("StopMining");

        isInMiningState = false;
        UseHorizontalDrill();
    }

    private void StartMining(Vector2Int startPos, Vector2Int direction)
    {
        if (isInMiningState)
            return;

        isInMiningState = true;
        if (direction == Vector2Int.down)
            UseDownDrill();

        StartCoroutine(IEStartMining(startPos, direction));
    }

    public void SetDrillingTime(float time)
    {
        diggingTime = time;
    }

    public void SetMaxSpeedAndAcceleration(float maxSpeed, float acceleration)
    {
        PlayerMovement.maxSpeed = maxSpeed;
        forceSpeed = acceleration;
    }
}
