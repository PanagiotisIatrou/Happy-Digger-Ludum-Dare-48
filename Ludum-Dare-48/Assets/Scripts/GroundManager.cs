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
    public GameObject[] FossilePrefabs;
    private GameObject playerGO;
    private Transform cameraTR;
    private Dictionary<Vector2Int, bool> minedTiles = new Dictionary<Vector2Int, bool>();
    private Dictionary<Vector2Int, GameObject> specialTiles = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, bool> indestructableTiles = new Dictionary<Vector2Int, bool>();

    private int[,] levelOresChances = { { 100, 0, 0, 0, 0 }, { 70, 100, 0, 0, 0 }, { 30, 70, 100, 0, 0 }, { 20, 40, 70, 100, 0 }, { 10, 30, 50, 70, 100 } };

    private void Start()
    {
        playerGO = GameManager.GetCurrentPlayer();
        cameraTR = Camera.main.transform;

        // Fuel Station base
        indestructableTiles.Add(new Vector2Int(-1, -1), true);
        indestructableTiles.Add(new Vector2Int(0, -1), true);
        indestructableTiles.Add(new Vector2Int(1, -1), true);
        indestructableTiles.Add(new Vector2Int(2, -1), true);

        // Selling Station base
        indestructableTiles.Add(new Vector2Int(8, -1), true);
        indestructableTiles.Add(new Vector2Int(9, -1), true);
        indestructableTiles.Add(new Vector2Int(10, -1), true);
        indestructableTiles.Add(new Vector2Int(11, -1), true);
        indestructableTiles.Add(new Vector2Int(12, -1), true);

        // Upgrades Station base
        indestructableTiles.Add(new Vector2Int(17, -1), true);
        indestructableTiles.Add(new Vector2Int(18, -1), true);
        indestructableTiles.Add(new Vector2Int(19, -1), true);
        indestructableTiles.Add(new Vector2Int(20, -1), true);
        indestructableTiles.Add(new Vector2Int(21, -1), true);
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
                            GameObject oreGO;
                            bool isFossile = Random.Range(0, 100) >= 98;
                            if (isFossile)
                            {
                                oreGO = Instantiate(FossilePrefabs[Random.Range(0, FossilePrefabs.Length)], new Vector3(pos.x, pos.y, -1), Quaternion.identity, SpecialTilesHolder);
                                oreGO.name = "Fossile";
                            }
                            else
                            {
                                int level;
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

                                oreGO = Instantiate(OrePrefabs[Random.Range(0, OrePrefabs.Length)], new Vector3(pos.x, pos.y, -1), Quaternion.identity, SpecialTilesHolder);
                                int r2 = Random.Range(0, 100);
                                if (r2 <= levelOresChances[level, 0])
                                {
                                    oreGO.name = "Silver";
                                }
                                else if (r2 <= levelOresChances[level, 1])
                                {
                                    oreGO.name = "Gold";
                                    oreGO.GetComponent<SpriteRenderer>().color = Color.yellow;
                                }
                                else if (r2 <= levelOresChances[level, 2])
                                {
                                    oreGO.name = "Emerald";
                                    oreGO.GetComponent<SpriteRenderer>().color = Color.green;
                                }
                                else if (r2 <= levelOresChances[level, 3])
                                {
                                    oreGO.name = "Red Iron";
                                    oreGO.GetComponent<SpriteRenderer>().color = Color.red;
                                }
                                else if (r2 <= levelOresChances[level, 4])
                                {
                                    oreGO.name = "Lapis";
                                    oreGO.GetComponent<SpriteRenderer>().color = Color.blue;
                                }
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
        return Instance.tilemap.HasTile((Vector3Int)position) && Instance.tilemap.GetTile((Vector3Int)position).name == "Rules";
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

    public static bool IsTileIndestructable(Vector2Int position)
    {
        return Instance.indestructableTiles.ContainsKey(position);
    }
}
