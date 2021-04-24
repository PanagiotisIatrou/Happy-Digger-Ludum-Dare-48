using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundManager : MonoBehaviour
{
    public Tilemap tilemap;

    private void Start()
    {

    }

    private void Update()
    {
        
    }

    public bool ExistsTileInPosition(Vector2Int position)
    {
        return tilemap.HasTile((Vector3Int)position);
    }

    public void DestroyTileInPosition(Vector2Int position)
    {
        tilemap.SetTile((Vector3Int)position, null);
    }
}
