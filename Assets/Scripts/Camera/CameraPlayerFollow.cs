using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody2D targetRB2D;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float YOffset;
    [SerializeField] private float XOffsetVelocityThreshold;
    [SerializeField] private float XOffsetFromVelocityMultiplier;

    [SerializeField] private float YOffsetVelocityThreshold;
    [SerializeField] private float YOffsetFromVelocityMultiplier;

    [SerializeField] private Vector2 cameraBoundariesMin;
    [SerializeField] private Vector2 cameraBoundariesMax;

    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = CalculateTargetPosition();
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            ApplyCameraBoundaries();
        }
    }

    private Vector3 CalculateTargetPosition()
    {
        float targetX = target.position.x;
        float targetY = target.position.y + YOffset;
        if (Mathf.Abs(targetRB2D.velocity.x) > XOffsetVelocityThreshold)
        {
            targetX += targetRB2D.velocity.x * XOffsetFromVelocityMultiplier;
        }

        if (Mathf.Abs(targetRB2D.velocity.y) > YOffsetVelocityThreshold)
        {
            targetY += targetRB2D.velocity.y * YOffsetFromVelocityMultiplier;
        }

        return new Vector3(targetX, targetY, transform.position.z);
    }

    private void ApplyCameraBoundaries()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(newPosition.x, cameraBoundariesMin.x, cameraBoundariesMax.x);
        newPosition.y = Mathf.Clamp(newPosition.y, cameraBoundariesMin.y, cameraBoundariesMax.y);
        transform.position = newPosition;
    }
}
