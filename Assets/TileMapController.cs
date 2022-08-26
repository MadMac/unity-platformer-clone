using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapController : MonoBehaviour
{
    private Tilemap worldMap;

    [SerializeField]
    private GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        worldMap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update() { }

    public void DestroyTileAt(Vector3Int tileLocation)
    {
        if (worldMap.GetTile(tileLocation) != null)
        {
            Debug.Log("## DESTROY TILE");
            worldMap.SetTile(tileLocation, null);
            Instantiate(explosionPrefab, tileLocation, Quaternion.identity);
        }
    }
}
