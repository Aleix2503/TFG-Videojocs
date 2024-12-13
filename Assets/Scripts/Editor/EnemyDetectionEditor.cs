using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerDetection))]
public class EnemyDetectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Referencia al objeto objetivo
        PlayerDetection detection = (PlayerDetection)target;

        // Dibuja el tipo de detección
        detection.detectionType = (PlayerDetection.DetectionType)EditorGUILayout.EnumPopup("Detection Type", detection.detectionType);

        // Mostrar u ocultar campos según el tipo de detección
        if (detection.detectionType == PlayerDetection.DetectionType.Circle)
        {
            detection.detectionRadius = EditorGUILayout.FloatField("Detection Radius", detection.detectionRadius);
        }
        else if (detection.detectionType == PlayerDetection.DetectionType.Cone)
        {
            detection.coneAngle = EditorGUILayout.FloatField("Cone Angle", detection.coneAngle);
            detection.coneDistance = EditorGUILayout.FloatField("Cone Distance", detection.coneDistance);
        }

        // Guardar cambios en la escena
        if (GUI.changed)
        {
            EditorUtility.SetDirty(detection);
        }
    }
}
