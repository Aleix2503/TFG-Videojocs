using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : StateBehaviour
{
    [SerializeField]
    private GameObject idlePoint;
    [SerializeField]
    private float idleSpeed;

    private bool alreadyArrived = true;
    private bool alreadyIdleing = false;

    private enum Direction
    {
        Right,
        Left
    }
    [SerializeField]
    private Direction direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        fsmEnemies = GetComponent<FSMEnemies>();

        SetAlreadyArrivedFalse();
    }

    public override void Behaviour()
    {
        if (!alreadyArrived) MoveToIdlePoint();
        else if (!alreadyIdleing)
        {
            alreadyIdleing = true;
            animator.SetTrigger("isArrived");
        }
    }

    private void MoveToIdlePoint()
    {
        Vector3 targetPosition = idlePoint.transform.position;

        Vector2 direction = (targetPosition - transform.position).normalized;
        Vector2 movement = direction * idleSpeed;

        rb.velocity = movement;

        if (Vector2.Distance(transform.position, targetPosition) < 0.2f)
        {
            alreadyArrived = true;

            Vector3 rotation = transform.rotation.eulerAngles;
            if (this.direction == Direction.Right)
                rotation.y = 0f;
            else
                rotation.y = 180f;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

    public void SetAlreadyArrivedFalse()
    {
        float rotationY = RoundToZero(transform.rotation.eulerAngles.y);

        if (transform.position.x > idlePoint.transform.position.x && rotationY == 0)
        {
            transform.Rotate(0, 180, 0);
        }
        else if (transform.position.x < idlePoint.transform.position.x && rotationY == 180)
        {
            transform.Rotate(0, 180, 0);
        }

        alreadyArrived = false;
        alreadyIdleing = false;
    }

    float RoundToZero(float value, float epsilon = 0.01f)
    {
        return Mathf.Abs(value) < epsilon ? 0f : value;
    }
}
