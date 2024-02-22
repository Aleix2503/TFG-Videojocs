using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerFollow : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody2D targetRB2D;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float YOffset;
    [SerializeField] private float XOffset;

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
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            ApplyCameraBoundaries();
        }
    }

    private Vector3 CalculateTargetPosition()
    {
        float targetX = target.position.x + XOffset;
        float targetY = target.position.y + YOffset;

        return new Vector3(targetX, targetY, transform.position.z);
    }

    private void ApplyCameraBoundaries()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(newPosition.x, cameraBoundariesMin.x, cameraBoundariesMax.x);
        newPosition.y = Mathf.Clamp(newPosition.y, cameraBoundariesMin.y, cameraBoundariesMax.y);
        transform.position = newPosition;
    }

    private void OnDrawGizmos()
    {
        if (cam == null) return;
    }
}
