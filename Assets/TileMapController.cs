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
            worldMap.SetTile(tileLocation, null);
            Instantiate(explosionPrefab, tileLocation, Quaternion.identity);
        }
    }

    public void StartBounceTile(Vector3Int tileLocation)
    {
        StartCoroutine(BounceTile(tileLocation));
    }

    IEnumerator BounceTile(Vector3Int tileLocation)
    {
        float[] movementArray = new float[]
        {
            0.1f,
            0.2f,
            0.3f,
            0.4f,
            0.5f,
            0.4f,
            0.3f,
            0.2f,
            0.1f,
            0.0f
        };
        for (int i = 0; i < movementArray.Length; i++)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(
                new Vector3(0f, movementArray[i], 0.0f),
                Quaternion.Euler(0f, 0f, 0f),
                Vector3.one
            );
            worldMap.SetTransformMatrix(tileLocation, matrix);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
