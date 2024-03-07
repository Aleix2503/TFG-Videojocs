using UnityEngine;

public class SplashArtParallax : MonoBehaviour
{
    public float movementModifier = 0.5f;
    private Vector3 initialPosition;
    private Camera cam;

    void Start()
    {
        initialPosition = transform.position;
        // If your canvas is set to Screen Space - Camera, assign the camera here
        // Otherwise, if it's Screen Space - Overlay, you might not need this
        cam = Camera.main;
    }

    void Update()
    {
        // Convert the mouse position to world space
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - cam.transform.position.z));

        // Calculate the target position based on movementModifier
        Vector3 targetPosition = Vector3.Lerp(initialPosition, mouseWorldPosition, movementModifier);

        // Apply the calculated position to the UI element
        transform.position = new Vector3(targetPosition.x, targetPosition.y, initialPosition.z); // Keep original z position
    }
}
