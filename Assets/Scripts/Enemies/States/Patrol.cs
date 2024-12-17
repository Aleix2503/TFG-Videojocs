using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : StateBehaviour
{
    [Header("PatrolValues")]
    [SerializeField]
    protected List<GameObject> patrolPoints;

    [SerializeField]
    public float patrolSpeed = 3;
    protected int currentPointIndex = 0;

    new void Start()
    {
        base.Start();

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

    private void MoveToPoint()
    {
        Vector3 targetPosition = patrolPoints[currentPointIndex].transform.position;

        Vector2 direction = (targetPosition - transform.position).normalized;
        Vector2 movement = direction * patrolSpeed;

        rb.velocity = movement;

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
            
            Flip();

            /*Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = targetRotation;*/
        }
    }

    private void Flip()
    {
        if (transform.position.x > patrolPoints[currentPointIndex].transform.position.x && spriteRenderer[0].flipX)
        {
            for (int i = 0; i < spriteRenderer.Length; i++)
                spriteRenderer[i].flipX = false;
        }
        else if (transform.position.x < patrolPoints[currentPointIndex].transform.position.x && !spriteRenderer[0].flipX)
        {
            for (int i = 0; i < spriteRenderer.Length; i++)
                spriteRenderer[i].flipX = true;
        }
    }

    public void RotateTowardsPoint()
    {
        /*// Calcular la dirección hacia el jugador
        Vector2 direction = patrolPoints[currentPointIndex].transform.position - transform.position;

        // Calcular el ángulo en radianes y convertirlo a grados
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Crear una rotación en Z hacia el ángulo calculado
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        transform.rotation = targetRotation;*/

        Flip();
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
