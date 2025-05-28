using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ColorTile", menuName = "Tiles/ColorTile")]
public class ColorTile : Tile
{
    public Color tintColor = Color.white;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.color = tintColor;
    }
}
