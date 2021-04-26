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
    public RuleTile RuleTile;
    public Tile BackgroundTile;
    public Transform SpecialTilesHolder;
    public GameObject[] OrePrefabs;
    private GameObject playerGO;
    private Transform cameraTR;
    private Dictionary<Vector2Int, bool> minedTiles = new Dictionary<Vector2Int, bool>();
    private Dictionary<Vector2Int, GameObject> specialTiles = new Dictionary<Vector2Int, GameObject>();

    private int[,] levelOresChances = { { 100, 0, 0, 0, 0 }, { 70, 100, 0, 0, 0 }, { 30, 70, 100, 0, 0 }, { 20, 40, 70, 100, 0 }, { 10, 30, 50, 70, 100 } };

    private void Start()
    {
        playerGO = GameManager.GetCurrentPlayer();
        cameraTR = Camera.main.transform;
    }

    private void Update()
    {
        if (playerGO == null)
            playerGO = GameManager.GetCurrentPlayer();

        Vector2Int playerPos = new Vector2Int(Mathf.RoundToInt(cameraTR.position.x), Mathf.RoundToInt(cameraTR.position.y));
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
                            int level = -1;
                            if (pos.y > -8)
                                level = 0;
                            else if (pos.y > -20)
                                level = 1;
                            else if (pos.y > -40)
                                level = 2;
                            else if (pos.y > -80)
                                level = 3;
                            else
                                level = 4;
                            GameObject oreGO = Instantiate(OrePrefabs[Random.Range(0, OrePrefabs.Length)], new Vector3(pos.x, pos.y, -1), Quaternion.identity, SpecialTilesHolder);
                            int r = Random.Range(0, 100);
                            if (r <= levelOresChances[level, 0])
                            {
                                oreGO.name = "Silver";
                            }
                            else if (r <= levelOresChances[level, 1])
                            {
                                oreGO.name = "Gold";
                                oreGO.GetComponent<SpriteRenderer>().color = Color.yellow;
                            }
                            else if (r <= levelOresChances[level, 2])
                            {
                                oreGO.name = "Emerald";
                                oreGO.GetComponent<SpriteRenderer>().color = Color.green;
                            }
                            else if (r <= levelOresChances[level, 3])
                            {
                                oreGO.name = "Red Iron";
                                oreGO.GetComponent<SpriteRenderer>().color = Color.red;
                            }
                            else if (r <= levelOresChances[level, 4])
                            {
                                oreGO.name = "Lapis";
                                oreGO.GetComponent<SpriteRenderer>().color = Color.blue;
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
        Instance.tilemap.SetTile((Vector3Int)position, Instance.BackgroundTile);
        if (Instance.specialTiles.ContainsKey(position))
        {
            GameObject ore = Instance.specialTiles[position];
            AudioSource.PlayClipAtPoint(GameManager.Instance.CoinPickupSound, ore.transform.position);
            Destroy(ore);
            Instance.playerGO.GetComponent<Inventory>().AddOre(ore.name);
            Instance.specialTiles.Remove(position);
        }
    }

    public static void AddMinedTile(Vector2Int position)
    {
        Instance.minedTiles.Add(position, true);
    }
}
