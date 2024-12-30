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

        GetComponent<EnemySoundEmitter>().PlayPatrolMosca();
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
        Flip();
    }

    public void setCurrentPointIndex()
    {
        currentPointIndex = 0;
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
