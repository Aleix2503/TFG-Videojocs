using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public enum DetectionType
    {
        None,       // Sin detección
        Circle,     // Detección por círculo
        Cone        // Detección por cono
    }

    [Header("Configuración de detección")]
    public DetectionType detectionType = DetectionType.None;

    // Variables relacionadas con el círculo
    [HideInInspector] public float detectionRadius = 5f;

    // Variables relacionadas con el cono
    [HideInInspector] public float coneAngle = 45f;
    [HideInInspector] public float coneDistance = 10f;

    private Transform player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (detectionType == DetectionType.Circle)
        {
            DetectByCircle();
        }
        else if (detectionType == DetectionType.Cone)
        {
            DetectByCone();
        }
    }

    // Método para detectar con un círculo
    private void DetectByCircle()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRadius)
        {
            Debug.Log("Jugador detectado en el círculo de visión");
        }
    }

    // Método para detectar con un cono
    private void DetectByCone()
    {
        if (player == null) return;

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer);

        if (angleToPlayer <= coneAngle / 2 &&
            Vector2.Distance(transform.position, player.position) <= coneDistance)
        {
            Debug.Log("Jugador detectado en el cono de visión");
        }
    }

    void OnDrawGizmos()
    {
        // Dibujar los detectores en caso de visión
        if (detectionType == DetectionType.Circle)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
        else if (detectionType == DetectionType.Cone)
        {
            Gizmos.color = Color.green;

            Vector3 forward = transform.right * coneDistance;
            Quaternion leftRayRotation = Quaternion.Euler(0, 0, coneAngle / 2);
            Quaternion rightRayRotation = Quaternion.Euler(0, 0, -coneAngle / 2);

            Vector3 leftRay = leftRayRotation * forward;
            Vector3 rightRay = rightRayRotation * forward;

            Gizmos.DrawLine(transform.position, transform.position + leftRay);
            Gizmos.DrawLine(transform.position, transform.position + rightRay);
        }
    }
}
