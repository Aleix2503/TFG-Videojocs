using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : StateBehaviour
{
    [SerializeField]
    private List<GameObject> patrolPoints;

    [SerializeField]
    private float patrolSpeed;
    private int currentPointIndex = 0;
    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (patrolPoints == null || patrolPoints.Count == 0)
        {
            Debug.LogError("No patrol points have been assigned");
        }

        Flip();
    }

    public override void Behaviour()
    {
        if (patrolPoints.Count == 0)
            return;

        MoveToPoint();
    }

    void MoveToPoint()
    {
        Vector3 targetPosition = patrolPoints[currentPointIndex].transform.position;

        Vector2 direction = (targetPosition - transform.position).normalized;
        Vector2 movement = direction * patrolSpeed;

        rb.velocity = movement;

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
            Flip();
        }
    }

    private void Flip()
    {
        float rotationY = RoundToZero(transform.rotation.eulerAngles.y);

        if (transform.position.x > patrolPoints[currentPointIndex].transform.position.x && rotationY == 0)
        {
            transform.Rotate(0, 180, 0);
        } else if (transform.position.x < patrolPoints[currentPointIndex].transform.position.x && rotationY == 180)
        {
            transform.Rotate(0, 180, 0);
        }

        // Corregir la rotación si es necesario
        CorrectRotation();
    }

    float RoundToZero(float value, float epsilon = 0.01f)
    {
        return Mathf.Abs(value) < epsilon ? 0f : value;
    }

    void CorrectRotation()
    {
        // Obtener los ángulos de rotación actuales
        Vector3 rotation = transform.rotation.eulerAngles;

        // Normalizar la rotación a valores exactos de 0 o 180
        if (Mathf.Abs(rotation.y % 360) < 1f) // Cerca de 0
        {
            rotation.y = 0f;
        }
        else if (Mathf.Abs((rotation.y - 180f) % 360) < 1f) // Cerca de 180
        {
            rotation.y = 180f;
        }

        // Aplicar la corrección de rotación
        transform.rotation = Quaternion.Euler(rotation);
    }

    void OnDrawGizmos()
    {
        // Dibujar las líneas entre puntos de patrulla para visualizarlos en la escena
        if (patrolPoints == null || patrolPoints.Count < 2)
            return;

        Gizmos.color = Color.green;
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            // Dibujar líneas entre los puntos
            Vector3 current = patrolPoints[i].transform.position;
            Vector3 next = patrolPoints[(i + 1) % patrolPoints.Count].transform.position;
            Gizmos.DrawLine(current, next);

            // Dibujar un pequeño círculo en cada punto
            Gizmos.color = Color.red; // Cambiar color para los puntos
            Gizmos.DrawSphere(current, 0.2f); // Dibujar esfera con un radio pequeño
        }
    }
}
