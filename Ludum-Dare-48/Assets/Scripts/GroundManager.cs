using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public GameObject[] TilePrefabs;
    public Transform TilesHolder;
    private Dictionary<Vector2Int, Tile> groundTiles = new Dictionary<Vector2Int, Tile>();

    private void Start()
    {
        GenerateInitialGround();
    }

    private void Update()
    {
        
    }

    private void GenerateInitialGround()
    {
        for (int i = -10; i <= 10; i++)
        {
            for (int j = -10; j <= -2; j++)
            {
                Vector2Int pos = new Vector2Int(i, j);
                Vector3 worldPos = new Vector3(pos.x, pos.y, 0f);

                GameObject tile;
                // Generate only dirt on top 1 layer
                if (j == -2)
                {
                    tile = Instantiate(TilePrefabs[0], worldPos, Quaternion.identity, TilesHolder);
                }
                else
                {
                    // 90% dirt 10% other ores
                    int r1 = Random.Range(0, 10);
                    if (r1 == 9)
                    {
                        int r2 = Random.Range(1, 2);
                        tile = Instantiate(TilePrefabs[r2], worldPos, Quaternion.identity, TilesHolder);
                    }
                    else
                    {
                        tile = Instantiate(TilePrefabs[0], worldPos, Quaternion.identity, TilesHolder);
                    }
                }
                groundTiles.Add(pos, new Tile(pos, tile));
            }
        }
    }

    public bool ExistsTileInPosition(Vector2Int position)
    {
        return groundTiles.ContainsKey(position);
    }

    public void DestroyTileInPosition(Vector2Int position)
    {
        Destroy(groundTiles[position].GetGameObject());
        groundTiles.Remove(position);
    }

    public void SetTileColliderStateInPosition(Vector2Int position, bool state)
    {
        groundTiles[position].GetGameObject().GetComponent<BoxCollider2D>().enabled = state;
    }
}

public class Tile
{
    private Vector2Int position;
    GameObject obj;

    public Tile(Vector2Int position, GameObject obj)
    {
        this.position = position;
        this.obj = obj;
    }

    public GameObject GetGameObject()
    {
        return obj;
    }
}
