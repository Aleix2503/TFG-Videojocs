using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using static UnityEditor.PlayerSettings;
using UnityEngine.UI;
using static ColorTile;
using UnityEngine.UIElements;

public class TilemapMaskController : MonoBehaviour
{
    public Tilemap tilemap;
    public Camera mainCamera;

    private RenderTexture maskRenderTexture;
    public Material tilemapMaterial;  // Material con shader TilemapMaskShader
    public Material stampMaterial;    // Material con shader StampPainter

    public Sprite[] stampSprites;

    public static TilemapMaskController _instance;


    [System.Serializable]
    public class TileStampData
    {
        public Vector3Int cellPos;
        public List<StampData> stamps;
    }

    [System.Serializable]
    public class SaveData
    {
        public List<TileStampData> allTileStamps = new();
    }


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    void Start()
    {
        BoundsInt bounds = tilemap.cellBounds;
        Vector4 tilemapData = new(bounds.size.x, bounds.size.y, bounds.xMin, bounds.yMin);

        RenderTexture rt = new(bounds.size.x * 16, bounds.size.y * 16, 0)
        {
            enableRandomWrite = true 
        };

        rt.Create();

        maskRenderTexture = rt;        

        tilemapMaterial.SetTexture("_MaskTex", maskRenderTexture);

        tilemapMaterial.SetVector("_TilemapSize", tilemapData);

        LoadTilemapStamps();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveTilemapStamps();
        }
    }

    public void PaintStamp(Vector3Int cellPos, Vector3Int playerCell, Color color)
    {
        int spriteIndex = Random.Range(0, stampSprites.Length);
        Sprite stamp = stampSprites[spriteIndex];

        float rotation = Random.Range(0f, 360f);
        float scale = 0.008f;

        Vector3Int delta = playerCell - cellPos;

        // Calcular UV de la celda para la RenderTexture
        Vector2 uv = CellToUV(cellPos);
        Vector4 pos = new(uv.x, uv.y + 0.002f, 0);

        if (delta.y > 0) pos = new Vector4(uv.x, uv.y + 0.002f, 0);
        else if (delta.y < 0) pos = new Vector4(uv.x, uv.y - 0.002f, 0);
        else if (delta.x > 0) pos = new Vector4(uv.x + 0.002f, uv.y, 0);
        else if (delta.x < 0) pos = new Vector4(uv.x - 0.002f, uv.y, 0);


        ColorTile tile = (ColorTile) tilemap.GetTile(cellPos);
        if (tile != null)
        {
            tile.SaveStamp(pos, spriteIndex, rotation, scale, color);
        }

        // Pintar el stamp en la RenderTexture
        DrawStampOnRenderTexture(stamp, rotation, scale, pos, color);
    }

    Vector2 CellToUV(Vector3Int cellPos)
    {
        BoundsInt bounds = tilemap.cellBounds;

        float u = (cellPos.x - bounds.xMin + 0.5f) / bounds.size.x;
        float v = (cellPos.y - bounds.yMin + 0.5f) / bounds.size.y;

        return new Vector2(u, v);
    }

    void DrawStampOnRenderTexture(Sprite stamp, float rotation, float scale, Vector4 pos, Color color)
    {
        // Crear textura del sprite
        Texture2D stampTex = SpriteToTexture(stamp);

        // Asignar parámetros
        stampMaterial.SetTexture("_StampTex", stampTex);
        stampMaterial.SetVector("_StampPos", pos);
        stampMaterial.SetFloat("_StampRotation", rotation);
        stampMaterial.SetFloat("_StampScale", scale);
        stampMaterial.SetColor("_StampColor", color);

        if (maskRenderTexture != null)
        {
            // Usar una temporal para evitar sobrescribir mientras blitteas
            RenderTexture tempRT = RenderTexture.GetTemporary(maskRenderTexture.width, maskRenderTexture.height, 0, maskRenderTexture.format);
            Graphics.Blit(maskRenderTexture, tempRT); // Copia el contenido actual

            Graphics.Blit(tempRT, maskRenderTexture, stampMaterial); // Aplica el nuevo stamp

            RenderTexture.ReleaseTemporary(tempRT);
        }
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

    void OnDestroy()
    {
        if (maskRenderTexture != null)
        {
            maskRenderTexture.Release();
            Destroy(maskRenderTexture);
        }
    }

    public void SaveTilemapStamps()
    {
        SaveData saveData = new();

        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPos = new(x, y, 0);
                TileBase tile = tilemap.GetTile(cellPos);

                if (tile is ColorTile myTile && myTile.stamps.Count > 0)
                {
                    TileStampData data = new()
                    {
                        cellPos = cellPos,
                        stamps = myTile.stamps
                    };
                    saveData.allTileStamps.Add(data);
                }
            }
        }

        string json = JsonUtility.ToJson(saveData, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/stampsSave.json", json);

        Debug.Log("Save completed at: " + Application.persistentDataPath);
    }

    public void LoadTilemapStamps()
    {
        string path = Application.persistentDataPath + "/stampsSave.json";
        if (!System.IO.File.Exists(path))
        {
            Debug.LogWarning("No save file found");
            return;
        }

        string json = System.IO.File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        foreach (var tileData in saveData.allTileStamps)
        {
            TileBase tile = tilemap.GetTile(tileData.cellPos);

            if (tile is ColorTile myTile)
            {
                myTile.stamps = tileData.stamps;

                foreach (var stamp in myTile.stamps)
                {
                    DrawStampOnRenderTexture(
                        stampSprites[stamp.spriteIndex],
                        stamp.rotation,
                        stamp.scale,
                        stamp.pos,
                        stamp.color);
                }     
            }
        }

        Debug.Log("Load completed");
    }

}
