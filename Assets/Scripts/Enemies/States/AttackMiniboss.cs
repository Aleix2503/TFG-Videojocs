using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class AttackMiniboss : Attack
{
    [Header("Values")]
    [SerializeField]
    private float velocity = 2f;
    [SerializeField]
    private float distance = 10f;

    private Vector3 startPosition;
    private Vector3 targetPosition; 
    private bool isMoving = false; 

    [Header("Player")]
    [SerializeField]
    private Transform player;

    [Header("Wall Collision")]
    [SerializeField] 
    private float wallCheckDistance = 1f;
    [SerializeField] 
    private LayerMask wallLayer;

    new void Start()
    {
        base.Start();
    }

    public override void Behaviour()
    {
        if (!isMoving)
        {
            Flip();

            startPosition = transform.position;
            float directionSign = Mathf.Sign(player.position.x - startPosition.x);
            Vector3 direction = new Vector3(directionSign, 0);

            targetPosition = startPosition + direction * distance;
            targetPosition = new Vector3(targetPosition.x, startPosition.y, startPosition.z);

            isMoving = true;
            GetComponent<EnemySoundEmitter>().PlayDashMiniBoss();
        }

        float step = velocity * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f || IsWallAhead())
        {
            isMoving = false; 
            OnReachedTarget();
        }
    }

    private void OnReachedTarget()
    {
        Flip();
        fsmEnemies.state = FSMEnemies.State.Recover;
    }

    private bool IsWallAhead()
    {
        float direction = Mathf.Sign(targetPosition.x - transform.position.x);
        Vector2 origin = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right * direction, wallCheckDistance, wallLayer);

        return hit.collider != null;
    }

    private void Flip()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        if (direction < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        else transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
