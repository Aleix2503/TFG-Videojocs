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
    [SerializeField] private float smoothTimeYWhenVelocityDown = 0.02f;
    [SerializeField] private float smoothTimeYOutsideBoundsUp = 0.2f;
    [SerializeField] private float smoothTimeYOutsideBoundsDown = 0.02f;
    [SerializeField] private float YOffset;
    [SerializeField] private float XOffset;

    [SerializeField] private float DownVelocityLimit = -4;
    [SerializeField] private float YDownLimit = -2;
    [SerializeField] private float YUpLimit = 3;

    [SerializeField] private float DownVelocityExtraOffsetMultiplier = 0.2f;

    [SerializeField] private Vector2 cameraBoundariesSize;
    [SerializeField] private Vector2 cameraBoundariesPosition;
    private bool areCameraBoundariesActive = true;

    private Vector3 velocity = Vector3.zero;

    public float currentHeight { get; private set; }
    public float currentWidth { get; private set; }

    private float halfHeight;
    private float halfWidth;

    private bool isTransitioningBetweenBounds = false;
    private Vector2 originalPosition = Vector2.zero;
    [SerializeField] private float boundsTransitionTime = 0.5f;
    private float transitionElapsedTime = 0f;


    private void Start()
    {
        currentHeight = 2f * cam.orthographicSize;
        currentWidth = currentHeight * cam.aspect;

        halfHeight = currentHeight / 2f;
        halfWidth = currentWidth / 2f;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            cam.transform.position = new Vector3(transform.position.x + XOffset, transform.position.y + YOffset, targetZ);

            Vector3 targetPosition = CalculateTargetPosition();
            Vector3 finalPosition = transform.position;
            finalPosition.x = Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref velocity.x, smoothTimeX);

            float relativePosition = target.position.y - transform.position.y;

            if (targetRB2D.velocity.y < DownVelocityLimit)
            {
                finalPosition.y = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocity.y, smoothTimeYWhenVelocityDown) - DownVelocityExtraOffsetMultiplier * (targetRB2D.velocity.y - DownVelocityLimit);
            }
            else if (relativePosition < YDownLimit)
            {
                finalPosition.y = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocity.y, smoothTimeYOutsideBoundsDown);
            }
            else if (relativePosition > YUpLimit)
            {
                finalPosition.y = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocity.y, smoothTimeYOutsideBoundsUp);
            }
            else
            {
                finalPosition.y = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocity.y, smoothTimeY);
            }

            ApplyCameraBounds(ref finalPosition);

            if (isTransitioningBetweenBounds)
            {
                InterpolateFromInitialPosition(ref finalPosition);
            }

            transform.position = finalPosition;
        }
    }

    private Vector3 CalculateTargetPosition()
    {
        float targetX = target.position.x;
        float targetY = target.position.y;

        return new Vector3(targetX, targetY, targetZ);
    }

    private void ApplyCameraBounds(ref Vector3 position)
    {
        if (!areCameraBoundariesActive) return;

        if (currentWidth > cameraBoundariesSize.x)
        {
            position.x = cameraBoundariesPosition.x;
        }
        else
        {
            float minX = cameraBoundariesPosition.x - cameraBoundariesSize.x / 2f + halfWidth;
            float maxX = cameraBoundariesPosition.x + cameraBoundariesSize.x / 2f - halfWidth;
            position.x = Mathf.Clamp(position.x, minX, maxX);
        }

        if (currentHeight > cameraBoundariesSize.y)
        {
            position.y = cameraBoundariesPosition.y;
        }
        else
        {
            float minY = cameraBoundariesPosition.y - cameraBoundariesSize.y / 2f + halfHeight;
            float maxY = cameraBoundariesPosition.y + cameraBoundariesSize.y / 2f - halfHeight;
            position.y = Mathf.Clamp(position.y, minY, maxY);
        }
    }

    private void InterpolateFromInitialPosition(ref Vector3 position)
    {
        if (transitionElapsedTime <= boundsTransitionTime)
        {
            float t = transitionElapsedTime / boundsTransitionTime;
            position = Vector3.Lerp(originalPosition, position, t);
            transitionElapsedTime += Time.fixedDeltaTime;
        }
        else
        {
            isTransitioningBetweenBounds = false;
        }
    }



    private void OnDrawGizmosSelected()
    {
        if (cam == null) return;

        //camera bounds
        Gizmos.color = new Color(1f, 1, 0.5f, 0.5f);
        Gizmos.DrawWireCube(cameraBoundariesPosition, cameraBoundariesSize);
    }

    public void UpdateCameraInfo(CameraInfo cameraInfo)
    {
        XOffset = cameraInfo.XOffset;
        YOffset = cameraInfo.YOffset;

        if (cameraInfo.areCameraBoundariesActive)
        {
            cameraBoundariesSize = cameraInfo.cameraBoundariesSize;
            cameraBoundariesPosition = cameraInfo.cameraBoundariesPosition + cameraInfo.cameraBoundariesOffset;
            StartBoundsTransition();
        }
        else
        {
            areCameraBoundariesActive = false;
        }

        currentHeight = 2f * cam.orthographicSize;
        currentWidth = currentHeight * cam.aspect;
        halfHeight = currentHeight / 2f;
        halfWidth = currentWidth / 2f;
    }

    private void StartBoundsTransition()
    {
        originalPosition = transform.position;
        transitionElapsedTime = 0f;

        isTransitioningBetweenBounds = true;
    }


}

[Serializable]
public class CameraInfo
{
    public bool areCameraBoundariesActive;
    public Vector2 cameraBoundariesSize;
    public Vector2 cameraBoundariesOffset;
    [HideInInspector] public Vector2 cameraBoundariesPosition;
    public float XOffset;
    public float YOffset;
}
