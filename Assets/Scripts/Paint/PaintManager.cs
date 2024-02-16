using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintManager : MonoBehaviour
{
    //MonoBehaviour. Es un gameobject presente en la escena.

    //Singleton, para que solo haya uno activo a la vez
    public static PaintManager _instance;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject); //DontDestroyOnLoad para que se quede entre escenas. Se podría quitar, depende de como hagamos handling entre escenas.
    }

    public void PlacePaintSplat(Vector2 coords, Vector2 size, Color color)
    {
        //Coloca pegote de pintura de este color y tamaño en estas coordenadas. Podeis colocar 
    }

    //Otras funciones especiales pueden llamar a la función base si es necesario. Por ejemplo, pintar con color random entre 2 colores:
    public void PlacePaintWithColorBetweenRange(Vector2 coords, Vector2 size, Color color1, Color color2)
    {
        Color randomColor = Color.Lerp(color1, color2, Random.Range(0f, 1f));

        PlacePaintSplat(coords, size, randomColor);
    }

    //Todo el tema de pintar la pintura debería estar aquí. Tambien se puede poner el tema de cargar/descargar pintura tambien en esta clase.
}
