using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileChecker : MonoBehaviour
{
    public Tilemap tilemap;
    public Sprite[] sprites;

    public float minSizeMod = 0.8f;
    public float maxSizeMod = 1.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = tilemap.WorldToCell(mouseWorld);

            TileBase tile = tilemap.GetTile(cellPos);
            if (tile is ColorTile colorTile)
            {
                Debug.Log("Tile color: " + colorTile.tintColor);
            }
        }
    }
}