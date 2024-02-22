using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerFollow : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody2D targetRB2D;
    [SerializeField] private float targetZ = 10;
    [SerializeField] private float smoothTimeX = 0.1f;
    [SerializeField] private float smoothTimeY = 2f;
    [SerializeField] private float smoothTimeYOutsideBoundsUp = 0.2f;
    [SerializeField] private float smoothTimeYOutsideBoundsDown = 0.02f;
    [SerializeField] private float YOffset;
    [SerializeField] private float XOffset;

    [SerializeField] private float YDownLimit = -1;
    [SerializeField] private float YUpLimit = 3;

    [SerializeField] private Vector2 cameraBoundariesMin;
    [SerializeField] private Vector2 cameraBoundariesMax;

    private Vector3 velocity = Vector3.zero;

    public float currentHeight { get; private set; }
    public float currentWidth { get; private set; }

    private void Start()
    {
        currentHeight = 2f * cam.orthographicSize;
        currentWidth = currentHeight * cam.aspect;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = CalculateTargetPosition();
            Vector3 finalPosition = transform.position;
            finalPosition.x = Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref velocity.x, smoothTimeX);

            float relativePosition = target.position.y - transform.position.y;

            if (relativePosition < YDownLimit)
            {
                finalPosition.y = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocity.y, smoothTimeYOutsideBoundsDown);
            } else if (relativePosition > YUpLimit)
            {
                finalPosition.y = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocity.y, smoothTimeYOutsideBoundsUp);
            } else
            {
                finalPosition.y = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocity.y, smoothTimeY);
            }

            //finalPosition = ApplyCameraBoundaries(finalPosition);

            transform.position = finalPosition;
        }
    }

    private bool isTargetWithinYBounds()
    {
        float relativePosition = target.position.y - transform.position.y;
        return relativePosition > YDownLimit && relativePosition < YUpLimit;
    }

    private Vector3 CalculateTargetPosition()
    {
        float targetX = target.position.x + XOffset;
        float targetY = target.position.y + YOffset;

        return new Vector3(targetX, targetY, targetZ);
    }

    private Vector3 ApplyCameraBoundaries(Vector2 position)
    {
        position.x = Mathf.Clamp(position.x, cameraBoundariesMin.x, cameraBoundariesMax.x);
        position.y = Mathf.Clamp(position.y, cameraBoundariesMin.y, cameraBoundariesMax.y);
        return position;
    }

    private void OnDrawGizmos()
    {
        if (cam == null) return;
    }
}
