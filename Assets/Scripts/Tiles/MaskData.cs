using UnityEngine;

[System.Serializable]
public struct MaskData
{
    public Texture2D texture;
    public Vector2 position; // en espacio local de la tile (0–1)
    public float scale;
    public float rotation; // en grados
}