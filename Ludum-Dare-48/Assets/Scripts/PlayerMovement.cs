using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float forceSpeed = 1000f;
    private float maxSpeed = 5f;
    private Rigidbody2D rb;

    private bool isInMiningState = false;
    private bool isDirLeft = true;
    private float switchTime = 0.07f;
    private Coroutine switchDirCoroutine;
    private Transform spriteTR;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteTR = transform.GetChild(0);
    }

    private void Update()
    {
        // Check for mining state
        if (!isInMiningState && rb.velocity.magnitude == 0)
        {
            // Get player tile position
            Vector2Int pos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

            // Get direction to see if player wants to mine
            if (Input.GetKey(KeyCode.A) && isDirLeft && GroundManager.ExistsTileInPosition(pos + new Vector2Int(-1, 0)))
            {
                StartMining(pos, new Vector2Int(-1, 0));
            }
            if (Input.GetKey(KeyCode.D) && !isDirLeft && GroundManager.ExistsTileInPosition(pos + new Vector2Int(1, 0)))
            {
                StartMining(pos, new Vector2Int(1, 0));
            }
            if (Input.GetKey(KeyCode.S) && GroundManager.ExistsTileInPosition(pos + new Vector2Int(0, -1)))
            {
                StartMining(pos, new Vector2Int(0, -1));
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
            rb.AddForce(new Vector2(-forceSpeed * Time.fixedDeltaTime, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (isDirLeft)
                SwitchDirection();
            rb.AddForce(new Vector2(forceSpeed * Time.fixedDeltaTime, 0));
        }

        if (rb.velocity.magnitude >= maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
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

        // Move the player
        float time = 0f;
        while (time < 1f)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, -1);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosFloats;

        // Destroy mined tile
        GroundManager.DestroyTileInPosition(startPos + direction);

        // Reenable player physics
        GetComponent<CircleCollider2D>().enabled = true;
        rb.isKinematic = false;

        isInMiningState = false;
    }

    private void StartMining(Vector2Int startPos, Vector2Int direction)
    {
        if (isInMiningState)
            return;

        isInMiningState = true;
        StartCoroutine(IEStartMining(startPos, direction));
    }
}
