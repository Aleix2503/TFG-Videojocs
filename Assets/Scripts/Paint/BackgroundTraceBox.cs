using UnityEngine;

public class BackgroundTraceBox : MonoBehaviour
{
    
    //TODO only if touching ground
    
    public GameObject splatPrefab; // Prefab of the splat game object
    public float moveThreshold = 0.1f; // Minimum movement threshold to instantiate a splat
    public float instantiationInterval = 0.1f; // Interval between instantiating splats
    public Transform leftTraceTransform; // Transform for left trace
    public Transform rightTraceTransform; // Transform for right trace

    private Vector3 lastPosition; // Last position of the character
    private float timeSinceLastInstantiation; // Time elapsed since the last instantiation
    public float distanceMoved;
    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        
        // Calculate the distance moved by the character since the last frame
        distanceMoved = (transform.position - lastPosition).magnitude;
        Debug.Log(distanceMoved);

        if (Mathf.Abs(distanceMoved) >= moveThreshold)
        {
            // Update the last position
            lastPosition = transform.position;
            
            // Increment the time since the last instantiation
            timeSinceLastInstantiation += Time.deltaTime;

            // Check if enough time has passed since the last instantiation
            if (timeSinceLastInstantiation >= instantiationInterval)
            {
                //Transform traceTransform = distanceMoved < 0 ? leftTraceTransform : rightTraceTransform;
                
                // Instantiate the splat at the character's current position
                PaintManager._instance.PlaceBackgroundTrace(rightTraceTransform.position);

                // Reset the time since the last instantiation
                timeSinceLastInstantiation = 0f;
            }
            
        }
        
    }

    private void InstantiateSplat(Vector3 position)
    {
        GameObject trace =
            Instantiate(splatPrefab, position, Quaternion.identity) as GameObject;
        //splat.transform.SetParent(splatHolder, true);
        Trace traceScript = trace.GetComponent<Trace>();
        traceScript.Initialize(Trace.SplatLoacation.Foreground, 10, Color.white);

    }
}