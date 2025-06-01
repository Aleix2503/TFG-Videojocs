using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

public class TileCounter : MonoBehaviour
{
    public Tilemap tilemap;
    public Camera mainCamera;

    public GameObject player;

    void Start()
    {
        /*int tileCount = 0;

        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                tileCount++;
            }
        }

        Debug.Log("Total tiles: " + tileCount);*/

        CountTriangles(player);
    }

    private void Update()
    {
        //CountVisibleTileData();
    }

    void CountTriangles(GameObject obj)
    {
        int totalVertices = 0;
        int totalTriangles = 0;

        SpriteRenderer[] sprites = obj.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in sprites)
        {
            Sprite sprite = sr.sprite;
            if (sprite != null)
            {
                totalVertices += sprite.GetVertexCount();

                NativeArray<ushort> indices = sprite.GetIndices();
                totalTriangles += indices.Length / 3;
                // NO Dispose aquí
            }
        }

        Debug.Log($"[{obj.name}] Triángulos totales: {totalTriangles}, Vértices totales: {totalVertices}");
    }

    void CountVisibleTileData()
    {
        int tileCount = 0;
        int totalVertices = 0;
        int totalTriangles = 0;

        // Obtener el área visible en celdas
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero);
        Vector3 topRight = mainCamera.ViewportToWorldPoint(Vector3.one);

        Vector3Int min = tilemap.WorldToCell(bottomLeft);
        Vector3Int max = tilemap.WorldToCell(topRight);

        BoundsInt bounds = new BoundsInt(min, max - min + Vector3Int.one);

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile == null)
                continue;

            Sprite sprite = null;

            // Si es un Tile normal (no rule tile, animated, etc.)
            if (tile is Tile)
            {
                sprite = ((Tile)tile).sprite;
            }

            if (sprite == null)
                continue;

            tileCount++;
            totalVertices += sprite.GetVertexCount();

            NativeArray<ushort> indices = sprite.GetIndices();
            totalTriangles += indices.Length / 3;
        }

        Debug.Log($"Tiles visibles: {tileCount}, Triángulos: {totalTriangles}, Vértices: {totalVertices}");
    }
}
