using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundManager : MonoBehaviour
{
    // Singleton
    private static GroundManager _instance;
    public static GroundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GroundManager>();
            }

            return _instance;
        }
    }

    public Tilemap tilemap;
    public Transform PlayerTR;
    public RuleTile RuleTile;
    private Dictionary<Vector2Int, bool> minedTiles = new Dictionary<Vector2Int, bool>();

    private void Update()
    {
        Vector2Int playerPos = new Vector2Int(Mathf.RoundToInt(PlayerTR.position.x), Mathf.RoundToInt(PlayerTR.position.y));
        for (int i = -8; i <= 8; i++)
        {
            for (int j = -8; j <= 8; j++)
            {
                Vector2Int pos = playerPos + new Vector2Int(i, j);
                if (pos.y > -1)
                    continue;

                if (!ExistsTileInPosition(pos) && !minedTiles.ContainsKey(pos))
                {
                    tilemap.SetTile((Vector3Int)pos, RuleTile);
                }
            }
        }
    }

    public static bool ExistsTileInPosition(Vector2Int position)
    {
        return Instance.tilemap.HasTile((Vector3Int)position);
    }

    public static void DestroyTileInPosition(Vector2Int position)
    {
        Instance.tilemap.SetTile((Vector3Int)position, null);
    }

    public static void AddMinedTile(Vector2Int position)
    {
        Instance.minedTiles.Add(position, true);
    }
}
