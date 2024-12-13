using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(PatrolRoomba))]
public class TileSelectorEditor : Editor
{
    private bool selectingTile = false; // Bandera para saber si estamos seleccionando una Tile

    public override void OnInspectorGUI()
    {
        // Dibujar el inspector predeterminado
        DrawDefaultInspector();

        PatrolRoomba selector = (PatrolRoomba)target;

        // Agregar un botón personalizado
        if (GUILayout.Button("Set Patrol Points"))
        {
            selector.SetPatrolPoints(); // Ejecutar el método del script
        }

        // Botón para habilitar/deshabilitar el modo de selección
        if (GUILayout.Button(selectingTile ? "Cancelar Selección" : "Seleccionar Tile"))
        {
            selectingTile = !selectingTile;

            // Redibujar la vista de escena para reflejar los cambios
            SceneView.RepaintAll();
        }

        // Mostrar la posición seleccionada (si existe)
        if (selector.tilemap != null)
        {
            EditorGUILayout.LabelField("Posición seleccionada:", selector.selectedTilePosition.ToString());
        }
    }

    private void OnSceneGUI()
    {
        if (!selectingTile) return;

        PatrolRoomba selector = (PatrolRoomba)target;

        // Detectar clic izquierdo en la escena
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Vector2 mousePos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;

            if (selector.tilemap != null)
            {
                // Convertir la posición del mouse a celda del Tilemap
                Vector3Int cellPosition = selector.tilemap.WorldToCell(mousePos);

                // Guardar la posición seleccionada
                selector.SaveSelectedTile(cellPosition);

                // Detener el modo de selección
                selectingTile = false;
                SceneView.RepaintAll();
            }

            e.Use(); // Consumir el evento para evitar comportamientos no deseados
        }
    }
}

