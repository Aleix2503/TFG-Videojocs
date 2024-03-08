using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum PlayerPaintingState { def, moving, dashing, expanded }

public class PaintManager : MonoBehaviour
{
    //MonoBehaviour. Es un gameobject presente en la escena.
    
    //Singleton, para que solo haya uno activo a la vez
    public static PaintManager _instance;
    
    

    public PlayerPaintingState playerPaintingState = PlayerPaintingState.def;
    public PlayerPaintingState lastState = PlayerPaintingState.def;
    
    public enum ColorOption { OneColor, RandomBetweenTwoColors }

    public ParticleSystem particleSystem;
    public ParticleSystem dashParticleSystem;
    public ParticleSystem jumpParticleSystem;
    public ParticleSystem expandedParticleSystem;
    public ParticleSystem bubbleParticleSystem;
    
    public ColorOption colorOption;
    
    // Color variables for one color option
    public Color paintColor;

    // Color variables for random between two colors option
    public Color color1;
    public Color color2;
    
    public Color dashPaintColor;
    public Color expandedPaintColor;
    public Color bubblePaintColor;

    public Transform traceSpawnPosition;
    public Transform backgroundTraceSpawnPosition;
    public Transform backgroundExpandedTraceSpawnPosition;
    public Transform onFallTraceSpawnPosition;

    public GameObject splatPrefab;
    
    public GameObject tracePrefab;
    public GameObject backgroundTracePrefab;
    
    public Vector3 lastPosition; // Last position of the character
    public float timeSinceLastInstantiation; // Time elapsed since the last instantiation
    public float timeSinceLastBackgroundTraceInstantiation; // Time elapsed since the last instantiation

    public float moveThreshold = 0.1f; // Minimum movement threshold to instantiate a splat
    public float instantiationInterval = 0.1f; // Interval between instantiating splats
    
    public float dashThreshold = 0.1f; // Minimum movement threshold to instantiate a splat
    public float dashingInstantiationInterval = 0.1f; // Interval between instantiating splats

    public Vector3 distanceMoved;
    
    public Transform playerTransform;
    public Transform bubbleTransform;
    
    public int currentLayer = 3;


    public TMP_Text debugText;
    private int previousLayer; // Variable to store the previous layer value for comparison
    private float layersPerSecondInterval = 1f; // Time interval for calculating layers per second
    private float timeSinceLastLayersPerSecondCalculation; // Time elapsed since the last layers per second calculation

    public BubbleController bubble;
    private bool bubbleWasActive;
    public float buublePaintingRate = 2f;
    private float timeSinceLastBubblePaint = 0f;
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject); //DontDestroyOnLoad para que se quede entre escenas. Se podría quitar, depende de como hagamos handling entre escenas.
    }
    
    private void Start()
    {
        lastPosition = playerTransform.position;

        bubble = FindObjectOfType<BubbleController>();
    }

    private void Update()
    {
        distanceMoved = (playerTransform.position - lastPosition);
        //Debug.Log(distanceMoved);
        //lastPosition = playerTransform.position;

        if ((playerPaintingState == PlayerPaintingState.dashing && 
         lastState != PlayerPaintingState.dashing) ||
            (playerPaintingState == PlayerPaintingState.expanded && 
             lastState != PlayerPaintingState.expanded) ||
            (playerPaintingState == PlayerPaintingState.moving || playerPaintingState == PlayerPaintingState.def) &&
            (lastState != PlayerPaintingState.moving && lastState != PlayerPaintingState.def))
        {
            lastState = playerPaintingState;
            currentLayer++;
        }

        
        if (bubble.isActive)
        {
            if (!bubbleWasActive)
            {
                currentLayer++;
            }

            // Update the time since the last paint
            timeSinceLastBubblePaint += Time.deltaTime;

            // Check if enough time has passed to paint another bubble
            if (timeSinceLastBubblePaint >= 1f / buublePaintingRate)
            {
                PlaceBackgroundBubbleTrace(bubble.transform.position);
                // Reset the timer
                timeSinceLastBubblePaint = 0f;
            }
        }
        bubbleWasActive = bubble.isActive;
        


        if (playerPaintingState == PlayerPaintingState.dashing)
        {
            if (Mathf.Abs(distanceMoved.magnitude) >= dashThreshold)
            {
                // Increment the time since the last instantiation
                timeSinceLastBackgroundTraceInstantiation += Time.deltaTime;

                // Check if enough time has passed since the last instantiation
                if (timeSinceLastBackgroundTraceInstantiation >= dashingInstantiationInterval)
                {
                    PlaceBackgroundDashingTrace(backgroundTraceSpawnPosition.position);
                    // Reset the time since the last instantiation
                    timeSinceLastBackgroundTraceInstantiation = 0f;
                }
            
            }
        }
        else if (playerPaintingState == PlayerPaintingState.expanded)
        {
            if (Mathf.Abs(distanceMoved.magnitude) >= dashThreshold)
            {
                // Increment the time since the last instantiation
                timeSinceLastBackgroundTraceInstantiation += Time.deltaTime;

                // Check if enough time has passed since the last instantiation
                if (timeSinceLastBackgroundTraceInstantiation >= dashingInstantiationInterval)
                {
                    PlaceBackgroundExpandedTrace(backgroundExpandedTraceSpawnPosition.position);
                    // Reset the time since the last instantiation
                    timeSinceLastBackgroundTraceInstantiation = 0f;
                }
            
            }
        }
        else
        {
            if (Mathf.Abs(distanceMoved.magnitude) >= moveThreshold)
            {
                // Increment the time since the last instantiation
                timeSinceLastBackgroundTraceInstantiation += Time.deltaTime;

                // Check if enough time has passed since the last instantiation
                if (timeSinceLastBackgroundTraceInstantiation >= instantiationInterval)
                {
                    PlaceBackgroundTrace(backgroundTraceSpawnPosition.position);
                    // Reset the time since the last instantiation
                    timeSinceLastBackgroundTraceInstantiation = 0f;
                }
            
            }
        }
        
        
        
        switch (playerPaintingState)
        {
            case PlayerPaintingState.moving:
                // Calculate the distance moved by the character since the last frame
                float horizontalDistanceMoved = playerTransform.position.x - lastPosition.x;

                if (Mathf.Abs(horizontalDistanceMoved) >= moveThreshold)
                {
                    // Increment the time since the last instantiation
                    timeSinceLastInstantiation += Time.deltaTime;

                    // Check if enough time has passed since the last instantiation
                    if (timeSinceLastInstantiation >= instantiationInterval)
                    {
                        //Transform traceTransform = distanceMoved < 0 ? leftTraceTransform : rightTraceTransform;

                        // Instantiate the splat at the character's current position
                        PlaceTrace(traceSpawnPosition.position);

                        // Reset the time since the last instantiation
                        timeSinceLastInstantiation = 0f;
                    }
            
                }
                break;
               
            case PlayerPaintingState.dashing:
                // Calculate the distance dashed by the character since the last frame
                float distanceDashed = playerTransform.position.x - lastPosition.x;

                if (Mathf.Abs(distanceDashed) >= dashThreshold)
                {
                    // Increment the time since the last dashing instantiation
                    timeSinceLastInstantiation += Time.deltaTime;

                    // Check if enough time has passed since the last dashing instantiation
                    if (timeSinceLastInstantiation >= dashingInstantiationInterval)
                    {
                        // Instantiate the splat at the character's current position
                        PlaceDashingTrace(traceSpawnPosition.position);

                        // Reset the time since the last dashing instantiation
                        timeSinceLastInstantiation = 0f;
                    }

                }
                break;
            
            
        }
        // Update the last position
        lastPosition = playerTransform.position;

        UpdateDebugText();
    }
    
    public void PlaceTrace(Vector3 position)
    {
        Color traceColor = paintColor; // Default color is the single paint color

        switch (colorOption)
        {
            case ColorOption.OneColor:
                // Use the single paint color
                traceColor = paintColor;
                break;
            case ColorOption.RandomBetweenTwoColors:
                // Use a random color between color1 and color2
                traceColor = Color.Lerp(color1, color2, Random.Range(0f, 1f));
                break;
        }

        GameObject trace = Instantiate(tracePrefab, position, Quaternion.identity);
        Trace traceScript = trace.GetComponent<Trace>();
        GetDecalChunk(trace.transform);
        traceScript.Initialize(Trace.SplatLoacation.Foreground, currentLayer, traceColor);
    }
    
    public void PlaceOnFallTrace()
    {
        Color traceColor = paintColor; // Default color is the single paint color

        switch (colorOption)
        {
            case ColorOption.OneColor:
                // Use the single paint color
                traceColor = paintColor;
                break;
            case ColorOption.RandomBetweenTwoColors:
                // Use a random color between color1 and color2
                traceColor = Color.Lerp(color1, color2, Random.Range(0f, 1f));
                break;
        }

        GameObject trace = Instantiate(tracePrefab, onFallTraceSpawnPosition.position, Quaternion.identity);
        Trace traceScript = trace.GetComponent<Trace>();
        GetDecalChunk(trace.transform);
        traceScript.Initialize(Trace.SplatLoacation.Foreground, currentLayer, traceColor);
    }
    
    public void PlaceOnExpandedTrace()
    {
        Color traceColor = paintColor; // Default color is the single paint color

        switch (colorOption)
        {
            case ColorOption.OneColor:
                // Use the single paint color
                traceColor = paintColor;
                break;
            case ColorOption.RandomBetweenTwoColors:
                // Use a random color between color1 and color2
                traceColor = Color.Lerp(color1, color2, Random.Range(0f, 1f));
                break;
        }

        GameObject trace = Instantiate(tracePrefab, onFallTraceSpawnPosition.position, Quaternion.identity);
        Trace traceScript = trace.GetComponent<Trace>();
        GetDecalChunk(trace.transform);
        traceScript.Initialize(Trace.SplatLoacation.Foreground, currentLayer, expandedPaintColor);
    }
    
    public void PlaceBackgroundTrace(Vector3 position)
    {
        Color traceColor = paintColor; // Default color is the single paint color

        switch (colorOption)
        {
            case ColorOption.OneColor:
                // Use the single paint color
                traceColor = paintColor;
                break;
            case ColorOption.RandomBetweenTwoColors:
                // Use a random color between color1 and color2
                traceColor = Color.Lerp(color1, color2, Random.Range(0f, 1f));
                break;
        }

        GameObject trace = Instantiate(backgroundTracePrefab, position, Quaternion.identity);
        Trace traceScript = trace.GetComponent<Trace>();
        GetDecalChunk(trace.transform);
        Color color = traceColor;
        Color backgroundColor = new Color(color.r / 2f, color.g / 2f, color.b / 2f, 1f);
        
        traceScript.Initialize(Trace.SplatLoacation.Background, currentLayer, backgroundColor);
    }
    
    public void PlaceDashingTrace(Vector3 position)
    {
        Color traceColor = paintColor; // Default color is the single paint color

        switch (colorOption)
        {
            case ColorOption.OneColor:
                // Use the single paint color
                traceColor = paintColor;
                break;
            case ColorOption.RandomBetweenTwoColors:
                // Use a random color between color1 and color2
                traceColor = Color.Lerp(color1, color2, Random.Range(0f, 1f));
                break;
        }

        GameObject trace = Instantiate(tracePrefab, position, Quaternion.identity);
        Trace traceScript = trace.GetComponent<Trace>();
        GetDecalChunk(trace.transform);
        traceScript.Initialize(Trace.SplatLoacation.Foreground, currentLayer, dashPaintColor);
    }
    
    public void PlaceBackgroundDashingTrace(Vector3 position)
    {
        Color traceColor = paintColor; // Default color is the single paint color

        switch (colorOption)
        {
            case ColorOption.OneColor:
                // Use the single paint color
                traceColor = paintColor;
                break;
            case ColorOption.RandomBetweenTwoColors:
                // Use a random color between color1 and color2
                traceColor = Color.Lerp(color1, color2, Random.Range(0f, 1f));
                break;
        }

        GameObject trace = Instantiate(backgroundTracePrefab, position, Quaternion.identity);
        Trace traceScript = trace.GetComponent<Trace>();
        GetDecalChunk(trace.transform);
        Color color = dashPaintColor;
        Color backgroundColor = new Color(color.r / 2f, color.g / 2f, color.b / 2f, 1f);
        
        traceScript.Initialize(Trace.SplatLoacation.Background, currentLayer, backgroundColor);
    }
    
    public void PlaceBackgroundExpandedTrace(Vector3 position)
    {
        Color traceColor = paintColor; // Default color is the single paint color

        switch (colorOption)
        {
            case ColorOption.OneColor:
                // Use the single paint color
                traceColor = paintColor;
                break;
            case ColorOption.RandomBetweenTwoColors:
                // Use a random color between color1 and color2
                traceColor = Color.Lerp(color1, color2, Random.Range(0f, 1f));
                break;
        }

        GameObject trace = Instantiate(backgroundTracePrefab, position, Quaternion.identity);
        Trace traceScript = trace.GetComponent<Trace>();
        GetDecalChunk(trace.transform);
        Color color = expandedPaintColor;
        Color backgroundColor = new Color(color.r / 2f, color.g / 2f, color.b / 2f, 1f);
        
        traceScript.Initialize(Trace.SplatLoacation.Background, currentLayer, backgroundColor);
    }
    
    public void PlaceBackgroundBubbleTrace(Vector3 position)
    {
        Color traceColor = paintColor; // Default color is the single paint color

        switch (colorOption)
        {
            case ColorOption.OneColor:
                // Use the single paint color
                traceColor = paintColor;
                break;
            case ColorOption.RandomBetweenTwoColors:
                // Use a random color between color1 and color2
                traceColor = Color.Lerp(color1, color2, Random.Range(0f, 1f));
                break;
        }

        GameObject trace = Instantiate(backgroundTracePrefab, position, Quaternion.identity);
        Trace traceScript = trace.GetComponent<Trace>();
        GetDecalChunk(trace.transform);
        Color color = bubblePaintColor;
        Color backgroundColor = new Color(color.r / 2f, color.g / 2f, color.b / 2f, 1f);
        
        traceScript.Initialize(Trace.SplatLoacation.Background, currentLayer, backgroundColor);
    }

    public void EmitJumpParticles()
    {
        jumpParticleSystem.Emit(3);   
    }
    
    public void EmitDashParticles()
    {
        dashParticleSystem.Emit(5);   
    }
    
    public void EmitExpandedParticles()
    {
        expandedParticleSystem.Emit(5);   
    }
    
    public void EmitBubbleParticles()
    {
        bubbleParticleSystem.transform.position = bubbleTransform.position;
        bubbleParticleSystem.Emit(5);   
    }

    public void PlaceSplat(Vector3 position, Vector3 normal, Color color)
    {
        GameObject splat =
            Instantiate(splatPrefab, position, Quaternion.identity) as GameObject;
        GetDecalChunk(splat.transform);
        Splat splatScript = splat.GetComponent<Splat>();
        splatScript.Initialize(Splat.SplatLoacation.Foreground, currentLayer, color, normal);

    }
    
    private void UpdateDebugText()
    {
        if (debugText != null)
        {
            int totalObjects = (FindObjectsOfType<GameObject>().Length-37)/2; // Count total objects

            float fps = 1f / Time.deltaTime; // Calculate FPS
            debugText.text = string.Format("FPS: {0:F2}\nCurrent Layer: {1}\nLayers Per Second: {2}\nTotal Splats:{3}",
                fps, currentLayer, CalculateLayersPerSecond(), totalObjects);
        }
    }
    
    private float CalculateLayersPerSecond()
    {
        float layersPerSecond = 0f;

        // Check if enough time has passed since the last calculation
        if (Time.time - timeSinceLastLayersPerSecondCalculation >= layersPerSecondInterval)
        {
            // Calculate the change in currentLayer since the last calculation
            int layerChange = currentLayer - previousLayer;

            // Calculate layers per second
            layersPerSecond = (float)layerChange / layersPerSecondInterval;

            // Update previousLayer with the current value
            previousLayer = currentLayer;

            // Reset the timer for the next calculation
            timeSinceLastLayersPerSecondCalculation = Time.time;
        }

        return layersPerSecond;
    }

    public void SetPaintingState(PlayerPaintingState paintingState)
    {
        playerPaintingState = paintingState;
    }

    public void GetDecalChunk(Transform decalTransform)
    {
        Transform parent = DecalManager.Instance.GetActiveChunk(decalTransform).transform;
        if (parent != null)
        {
            decalTransform.SetParent(parent,true);
        }
        
    }

    public void InstanceExplosion(Vector3 position, PlayerController.AbilityType ability)
    {
        switch (ability)
        {
            case PlayerController.AbilityType.Bubble:
                bubbleParticleSystem.transform.position = position;
                bubbleParticleSystem.Emit(5);   
                break;
            case PlayerController.AbilityType.Dash:
                Transform dashParticlesTransform = dashParticleSystem.transform;
                dashParticleSystem.transform.position = position;
                dashParticleSystem.Emit(5);
                dashParticleSystem.transform.position = dashParticlesTransform.position;
                break;
            case PlayerController.AbilityType.Expand:
                Transform expandedParticlesTransform = expandedParticleSystem.transform;
                expandedParticleSystem.transform.position = position;
                expandedParticleSystem.Emit(5);
                expandedParticleSystem.transform.position = expandedParticlesTransform.position;
                break;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PaintManager))]
public class PaintManagerEditor : Editor
{
    SerializedProperty tracePrefabProp;
    SerializedProperty backgroundTracePrefabProp;
    SerializedProperty traceSpawnPositionProp;
    SerializedProperty colorOptionProp;
    SerializedProperty paintColorProp;
    SerializedProperty color1Prop;
    SerializedProperty color2Prop;
    SerializedProperty jumpParticleSystemProp;
    SerializedProperty dashParticleSystemProp;

    void OnEnable()
    {
        // Initialize serialized properties
        tracePrefabProp = serializedObject.FindProperty("tracePrefab");
        backgroundTracePrefabProp = serializedObject.FindProperty("backgroundTracePrefab");
        traceSpawnPositionProp = serializedObject.FindProperty("traceSpawnPosition");
        colorOptionProp = serializedObject.FindProperty("colorOption");
        paintColorProp = serializedObject.FindProperty("paintColor");
        color1Prop = serializedObject.FindProperty("color1");
        color2Prop = serializedObject.FindProperty("color2");
        jumpParticleSystemProp = serializedObject.FindProperty("jumpParticleSystem");
        dashParticleSystemProp = serializedObject.FindProperty("dashParticleSystem");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("On Move - Trace", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(tracePrefabProp, new GUIContent("Front Trace Prefab"));
        EditorGUILayout.PropertyField(backgroundTracePrefabProp, new GUIContent("Background Trace Prefab"));
        
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(colorOptionProp, new GUIContent("Front Trace Color Option"));

        PaintManager.ColorOption option = (PaintManager.ColorOption)colorOptionProp.enumValueIndex;
        switch (option)
        {
            case PaintManager.ColorOption.OneColor:
                EditorGUILayout.PropertyField(paintColorProp, new GUIContent("Front Trace Color"));
                break;
            case PaintManager.ColorOption.RandomBetweenTwoColors:
                EditorGUILayout.PropertyField(color1Prop, new GUIContent("Front Trace Color 1"));
                EditorGUILayout.PropertyField(color2Prop, new GUIContent("Front Trace Color 2"));
                break;
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(traceSpawnPositionProp, new GUIContent("Trace Spawn Position"));
        
        //EditorGUILayout.PropertyField(traceSpawnPositionProp, new GUIContent("On Fall Trace Spawn Position"));
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("On Jump - Splat", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(jumpParticleSystemProp, new GUIContent("Jump Particle System"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("On Dash", EditorStyles.boldLabel);
        // Add properties related to "On Dash" event here if needed

        serializedObject.ApplyModifiedProperties();

        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Other Inspector Elements");
        DrawDefaultInspector();
    }
}
#endif
