using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapMaskController : MonoBehaviour
{
    public Tilemap tilemap;
    public Camera mainCamera;

    public RenderTexture maskRenderTexture;
    public Material tilemapMaterial;  // Material con shader TilemapMaskShader
    public Material stampMaterial;    // Material con shader StampPainter

    public Sprite[] stampSprites;

    // Datos guardados por mancha
    [System.Serializable]
    public class StampData
    {
        public Vector3Int cellPos;
        public int spriteIndex;
        public float rotation;
        public float scale;
    }

    public List<StampData> savedStamps = new();

    void Start()
    {
        // Asignamos la RenderTexture al material del Tilemap
        tilemapMaterial.SetTexture("_MaskTex", maskRenderTexture);

        ReloadStamps();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = tilemap.WorldToCell(mouseWorld);

            if (tilemap.HasTile(cellPos))
            {
                PaintStamp(cellPos);
            }
        }
    }

    void PaintStamp(Vector3Int cellPos)
    {
        int spriteIndex = Random.Range(0, stampSprites.Length);
        Sprite stamp = stampSprites[spriteIndex];

        float rotation = Random.Range(0f, 360f);
        float scale = Random.Range(0.5f, 1.5f);

        // Guardar datos para persistencia
        savedStamps.Add(new StampData
        {
            cellPos = cellPos,
            spriteIndex = spriteIndex,
            rotation = rotation,
            scale = scale
        });

        // Calcular UV de la celda para la RenderTexture
        Vector2 uv = CellToUV(cellPos);

        // Pintar el stamp en la RenderTexture
        DrawStampOnRenderTexture(stamp, uv, rotation, scale, cellPos);
    }

    Vector2 CellToUV(Vector3Int cellPos)
    {
        BoundsInt bounds = tilemap.cellBounds;

        float u = (cellPos.x - bounds.xMin + 0.5f) / bounds.size.x;
        float v = (cellPos.y - bounds.yMin + 0.5f) / bounds.size.y;

        return new Vector2(u, v);
    }

    void DrawStampOnRenderTexture(Sprite stamp, Vector2 uvPos, float rotation, float scale, Vector3Int cellPos)
    {
        // Crear textura del sprite
        Texture2D stampTex = SpriteToTexture(stamp);

        // Asignar parámetros
        stampMaterial.SetTexture("_StampTex", stampTex);
        stampMaterial.SetVector("_StampPos", new Vector4(uvPos.x, uvPos.y, 0, 0));
        stampMaterial.SetFloat("_StampRotation", rotation);
        stampMaterial.SetFloat("_StampScale", scale);

        // Usar una temporal para evitar sobrescribir mientras blitteas
        RenderTexture tempRT = RenderTexture.GetTemporary(maskRenderTexture.width, maskRenderTexture.height, 0, maskRenderTexture.format);
        Graphics.Blit(maskRenderTexture, tempRT); // Copia el contenido actual

        Vector3Int origin = tilemap.origin;             // esquina inferior del área usada
        Vector3Int size = tilemap.size;                 // tamaño en tiles

        Vector2 tilemapSize = new(size.x, size.y);

        Vector2 clickedUV = new Vector2(cellPos.x, cellPos.y) / tilemapSize;
        tilemapMaterial.SetVector("_ClickedTileUV", clickedUV);
        tilemapMaterial.SetVector("_TilemapSize", tilemapSize);

        Graphics.Blit(tempRT, maskRenderTexture, stampMaterial); // Aplica el nuevo stamp

        RenderTexture.ReleaseTemporary(tempRT);
    }

    Texture2D SpriteToTexture(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width || sprite.rect.height != sprite.texture.height)
        {
            Texture2D newTex = new((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] pixels = sprite.texture.GetPixels(
                (int)sprite.textureRect.x,
                (int)sprite.textureRect.y,
                (int)sprite.textureRect.width,
                (int)sprite.textureRect.height
            );
            newTex.SetPixels(pixels);
            newTex.Apply();
            return newTex;
        }
        else
        {
            return sprite.texture;
        }
    }

    // Método para recargar manchas (por ejemplo al cargar escena)
    public void ReloadStamps()
    {
        // Limpiar RenderTexture
        RenderTexture.active = maskRenderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = null;

        /*foreach (var stamp in savedStamps)
        {
            Sprite s = stampSprites[stamp.spriteIndex];
            Vector2 uv = CellToUV(stamp.cellPos);
            DrawStampOnRenderTexture(s, uv, stamp.rotation, stamp.scale, cellPos);
        }*/
    }
}
