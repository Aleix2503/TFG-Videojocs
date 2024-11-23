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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (patrolPoints == null || patrolPoints.Count == 0)
        {
            Debug.LogError("No patrol points have been assigned");
        }
    }

    public override void Behaviour()
    {
        //Debug.Log("Estoy en patrol");
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
        }
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
