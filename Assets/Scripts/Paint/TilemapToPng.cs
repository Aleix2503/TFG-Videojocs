/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(TilemapToPng))]
public class TilemapToPngEditor : Editor
{
    string nombre = "";

    public override void OnInspectorGUI ()
    {
        TilemapToPng GTM = (TilemapToPng)target;


        //DrawDefaultInspector();

        if (GTM.Img == null)
        {
            if (GUILayout.Button("Create png"))
            {
                GTM.Pack();
            }
        }
        else
        {
            GUILayout.Label("File name");
            nombre = GUILayout.TextField(nombre);
            if(nombre.Length > 0)
            {
                if (GUILayout.Button("Export png"))
                {
                    GTM.ExportAsPng(nombre);
                    //GTM.SaveTextureAsset(nombre);
                }
            }
            
        }
            
        EditorGUILayout.LabelField("Tilemap Size", GTM.GetTilemapSize());
        
        GTM.Img = (Texture2D)EditorGUILayout.ObjectField("Texture", GTM.Img, typeof(Texture2D), false);

    }

}
#endif



public class TilemapToPng : MonoBehaviour
{

    Tilemap tm;

    int minX,maxX,minY,maxY;
    
    public Texture2D Img;

    public void Pack ()
    {
        tm = GetComponent<Tilemap>();
        Sprite SpriteCualquiera = null;


        for (int x = 0; x < tm.size.x; x++) //Hallamos el punto menor y mayor
        {
            for (int y = 0; y < tm.size.y; y++)
            {
                Vector3Int pos = new Vector3Int(-x, -y, 0);
                if (tm.GetSprite(pos) != null)
                {
                    SpriteCualquiera = tm.GetSprite(pos); //seleccionamos un sprite cualquiera para más tarde saber las dimensiones de los sprites
                    if (minX > pos.x)
                    {
                        minX = pos.x;
                    }
                    if (minY > pos.y)
                    {
                        minY = pos.y;
                    }
                }

                pos = new Vector3Int(x, y, 0);
                if (tm.GetSprite(pos) != null)
                {
                    if (maxX < pos.x)
                    {
                        maxX = pos.x;
                    }
                    if (maxY < pos.y)
                    {
                        maxY = pos.y;
                    }
                }
            }
        }


        //Hallamos el tamaño del sprite en pixeles
        float width = SpriteCualquiera.rect.width;
        float height = SpriteCualquiera.rect.height;


        //creamos una textura con el tamaño multiplicado por el numero de celdas
        Texture2D ImagenCreada = new Texture2D((int)width * tm.size.x, (int)height * tm.size.y);

        //Asignamos toda la imagen invisible
        Color[] invisible = new Color[ImagenCreada.width * ImagenCreada.height];
        for (int i = 0; i < invisible.Length; i++)
        {
            invisible[i] = new Color(0f, 0f, 0f, 0f);
        }
        ImagenCreada.SetPixels(0,0,ImagenCreada.width, ImagenCreada.height, invisible);
        

        //Ahora asignamos a cada bloque sus respectivos pixeles
        for (int x = minX; x <= maxX; x++)
        {
            for(int y = minY; y <= maxY; y++)
            {
                if (tm.GetSprite(new Vector3Int(x, y, 0)) != null)
                {
                    //mapeamos los pixeles para que el minX = 0 y minY = 0
                    ImagenCreada.SetPixels((x - minX) * (int)width, (y - minY) * (int)height, (int)width, (int)height, GetCurrentSprite(tm.GetSprite(new Vector3Int(x, y, 0))).GetPixels()   );
                }
            }
        }
        ImagenCreada.Apply();

        Img = ImagenCreada; //Almacenamos la textura de la imagen lista
    }

    Texture2D GetCurrentSprite(Sprite sprite) //metodo para obtener el sprite recortado tal y como lo ponemos
    {
        var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                         (int)sprite.textureRect.y,
                                         (int)sprite.textureRect.width,
                                         (int)sprite.textureRect.height);

        Texture2D textura = new Texture2D((int)sprite.textureRect.width,
                                         (int)sprite.textureRect.height);

        textura.SetPixels(pixels);
        textura.Apply();
        return textura;
    }

         public void ExportAsPng (string name) //metodo que exporta como png
         {
             byte[] bytes = Img.EncodeToPNG();
             
             
             
             Debug.Log(Img.width);
             
             var dirPath = Application.dataPath + "/Exported Tilemaps/";
             if (!Directory.Exists(dirPath))
             {
                 Directory.CreateDirectory(dirPath);
             }
             File.WriteAllBytes(dirPath + name + ".png", bytes);
            AssetDatabase.Refresh();
            Img = null;
         }

     public string GetTilemapSize()
     {
         
         Tilemap tm = GetComponent<Tilemap>();
         if (tm == null)
             return "Tilemap not assigned.";

         return "Size: " + tm.size.x + "x" + tm.size.y;
     }
     
     public void SaveTextureAsset(string name)
     {
         if (Img == null)
         {
             Debug.LogError("No texture available to save.");
             return;
         }

         // Specify the directory path
         string dirPath = "Assets/Exported Tilemaps/";

         // Create the directory if it doesn't exist
         if (!Directory.Exists(dirPath))
         {
             Directory.CreateDirectory(dirPath);
         }

         // Specify the file path
         string filePath = dirPath + name + ".asset";

         // Save the texture as an asset
         AssetDatabase.CreateAsset(Img, filePath);
         AssetDatabase.SaveAssets();

         Debug.Log("Texture asset saved: " + filePath);
     }
}*/