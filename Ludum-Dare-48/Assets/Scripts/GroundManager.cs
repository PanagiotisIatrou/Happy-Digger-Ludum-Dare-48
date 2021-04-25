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
    public Transform SpecialTilesHolder;
    public GameObject OrePrefab;
    private Dictionary<Vector2Int, bool> minedTiles = new Dictionary<Vector2Int, bool>();
    private Dictionary<Vector2Int, GameObject> specialTiles = new Dictionary<Vector2Int, GameObject>();
    private PlayerMovement player;

    private void Start()
    {
        player = PlayerTR.GetComponent<PlayerMovement>();
    }

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
                    if (pos.y < -2)
                    {
                        int r1 = Random.Range(0, 10);
                        if (r1 == 9)
                        {
                            GameObject oreGO = Instantiate(OrePrefab, new Vector3(pos.x, pos.y, -1), Quaternion.identity, SpecialTilesHolder);
                            int r2 = Random.Range(0, 5);
                            if (r2 == 0)
                            {
                                oreGO.GetComponent<SpriteRenderer>().color = Color.yellow;
                                oreGO.name = "Gold";
                            }
                            else
                            {
                                oreGO.name = "Silver";
                            }
                            specialTiles.Add(pos, oreGO);
                        }
                    }
                    
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
        if (Instance.specialTiles.ContainsKey(position))
        {
            GameObject ore = Instance.specialTiles[position];
            Destroy(ore);
            Instance.player.GetInventory().AddOre(ore.name);
            Instance.specialTiles.Remove(position);
        }
    }

    public static void AddMinedTile(Vector2Int position)
    {
        Instance.minedTiles.Add(position, true);
    }
}
