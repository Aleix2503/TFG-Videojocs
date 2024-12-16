using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class ExactOutlineGenerator : MonoBehaviour
{
    public static List<Vector2> GenerateOutline(Texture2D texture, float threshold = 0.1f)
    {
        int width = texture.width;
        int height = texture.height;
        List<Vector2> outlinePoints = new List<Vector2>();

        // Recorremos todos los píxeles de la textura para detectar bordes
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                Color pixelColor = texture.GetPixel(x, y);
                Color pixelLeft = texture.GetPixel(x - 1, y);
                Color pixelRight = texture.GetPixel(x + 1, y);
                Color pixelUp = texture.GetPixel(x, y + 1);
                Color pixelDown = texture.GetPixel(x, y - 1);

                // Si el píxel es suficientemente visible y sus vecinos son transparentes, lo marcamos como borde
                if (pixelColor.a > threshold &&
                    (pixelLeft.a <= threshold || pixelRight.a <= threshold ||
                     pixelUp.a <= threshold || pixelDown.a <= threshold))
                {
                    outlinePoints.Add(new Vector2(x, y));
                }
            }
        }

        return outlinePoints;
    }

    public static void SaveOutlineToMetaFile(string metaFilePath, List<Vector2> outlinePoints)
    {
        // Leemos el archivo .meta
        string metaFileString = File.ReadAllText(metaFilePath);

        // Generamos la cadena de contorno
        string outlineString = "outline: [";
        foreach (var point in outlinePoints)
        {
            outlineString += $"({point.x},{point.y}), ";
        }
        outlineString = outlineString.TrimEnd(',', ' ') + "]";

        // Reemplazamos el contorno existente (si lo hay) y lo guardamos
        if (metaFileString.Contains("outline:"))
        {
            metaFileString = metaFileString.Replace(GetStringBetween(metaFileString, "outline:", "physicsShape:"), outlineString);
        }
        else
        {
            metaFileString = metaFileString.Replace("physicsShape:", outlineString + "\nphysicsShape:");
        }

        // Guardamos el archivo .meta modificado
        File.WriteAllText(metaFilePath, metaFileString);
        AssetDatabase.ImportAsset(metaFilePath);
    }

    public static string GetStringBetween(string strSource, string strStart, string strEnd)
    {
        int Start, End;
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - Start);
        }
        else
        {
            return "";
        }
    }

    [MenuItem("Tools/Generate Outline for Selected Sprite")]
    public static void GenerateOutlineForSelectedSprite()
    {
        // Verifica si se ha seleccionado un sprite en el proyecto de Unity
        var selectedObject = Selection.activeObject;

        if (selectedObject == null || !(selectedObject is Texture2D))
        {
            Debug.LogError("Please select a sprite texture in the project.");
            return;
        }

        // Obtener la textura del sprite seleccionado
        Texture2D texture = (Texture2D)selectedObject;

        // Generar el contorno
        List<Vector2> outlinePoints = GenerateOutline(texture);

        // Obtener la ruta del archivo .meta del sprite
        string spritePath = AssetDatabase.GetAssetPath(texture);
        string metaFilePath = spritePath + ".meta";

        // Guardar el contorno en el archivo .meta
        SaveOutlineToMetaFile(metaFilePath, outlinePoints);
        Debug.Log($"Generated new outline for {texture.name}");
    }
}
