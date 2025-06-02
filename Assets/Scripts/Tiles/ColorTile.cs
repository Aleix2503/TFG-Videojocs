using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ColorTile", menuName = "Tiles/ColorTile")]
public class ColorTile : Tile
{
    public Color tintColor = Color.white;
    public Material material;

    public List<StampData> stamps = new();

    [System.Serializable]
    public class StampData
    {
        public Vector4 pos;
        public int spriteIndex;
        public float rotation;
        public float scale;
        public Color color;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.color = tintColor;
    }

    public void SaveStamp(Vector4 pos, int spriteIndex, float rotation, float scale, Color color)
    {
        stamps.Add(new StampData
        {
            pos = pos,
            spriteIndex = spriteIndex,
            rotation = rotation,
            scale = scale,
            color = color
        });
    }
}
