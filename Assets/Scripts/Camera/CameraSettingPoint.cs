using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettingPoint : MonoBehaviour
{
    [SerializeField] CameraInfo cameraInfo;
    CameraPlayerFollow cameraPlayerFollow;
    private void Start()
    {
        cameraPlayerFollow = FindObjectOfType<CameraPlayerFollow>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cameraInfo.cameraBoundariesPosition = transform.position;
        cameraPlayerFollow.UpdateCameraInfo(cameraInfo);
    }

    private void OnDrawGizmosSelected()
    {
        if (cameraInfo == null) return;
        if (cameraInfo.areCameraBoundariesActive == true)
        {
            //camera bounds
            Gizmos.color = new Color(1f, 1, 0f, 0.2f);
            Gizmos.DrawWireCube(transform.position + (Vector3)cameraInfo.cameraBoundariesOffset, cameraInfo.cameraBoundariesSize);
        }
    }
}
