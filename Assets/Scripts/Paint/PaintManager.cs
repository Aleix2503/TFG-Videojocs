using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public enum PlayerPaintingState { def, moving, dashing, expanded }

public class PaintManager : MonoBehaviour
{
    //MonoBehaviour. Es un gameobject presente en la escena.
    
    //Singleton, para que solo haya uno activo a la vez
    public static PaintManager _instance;

    public bool _shadersEnabled = true;
    public Tilemap tilemap;

    private GameObject player;


    #region vars
    public PlayerPaintingState playerPaintingState = PlayerPaintingState.def;
    public PlayerPaintingState lastState = PlayerPaintingState.def;
    
    public enum ColorOption { OneColor, RandomBetweenTwoColors }

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
    #endregion

    #region UnityFunctions
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

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        distanceMoved = (playerTransform.position - lastPosition);

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
    #endregion

    #region PlaceTraces
    public void PlaceTrace(Vector3 position)
    {
        if (!_shadersEnabled)
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
        else
        {
            Vector3 playerWorldPos = player.transform.position;
            Vector3Int playerCell = tilemap.WorldToCell(playerWorldPos);

            Vector3Int cellPos = tilemap.WorldToCell(position);

            Color color = player.GetComponentInChildren<SpriteRenderer>().color;

            TilemapMaskController._instance.PaintStamp(cellPos, playerCell, color);
        }
    }
    
    public void PlaceOnFallTrace()
    {
        if (!_shadersEnabled)
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
        else
        {
            Vector3 playerWorldPos = player.transform.position;

            Vector3Int playerCell = tilemap.WorldToCell(playerWorldPos);
            Vector3Int cellPos = tilemap.WorldToCell(onFallTraceSpawnPosition.position);

            Color color = player.GetComponentInChildren<SpriteRenderer>().color;

            TilemapMaskController._instance.PaintStamp(cellPos, playerCell, color);
        }
    }
    
    public void PlaceOnExpandedTrace()
    {
        if (!_shadersEnabled)
        {
            GameObject trace = Instantiate(tracePrefab, onFallTraceSpawnPosition.position, Quaternion.identity);
            Trace traceScript = trace.GetComponent<Trace>();
            GetDecalChunk(trace.transform);
            traceScript.Initialize(Trace.SplatLoacation.Foreground, currentLayer, expandedPaintColor);
        }
        else
        {
            Vector3 playerWorldPos = player.transform.position;

            Vector3Int playerCell = tilemap.WorldToCell(playerWorldPos);
            Vector3Int cellPos = tilemap.WorldToCell(onFallTraceSpawnPosition.position);

            Color color = player.GetComponentInChildren<SpriteRenderer>().color;

            TilemapMaskController._instance.PaintStamp(cellPos, playerCell, color);
        }
    }
    
    public void PlaceDashingTrace(Vector3 position)
    {
        if (!_shadersEnabled)
        {
            GameObject trace = Instantiate(tracePrefab, position, Quaternion.identity);
            Trace traceScript = trace.GetComponent<Trace>();
            GetDecalChunk(trace.transform);
            traceScript.Initialize(Trace.SplatLoacation.Foreground, currentLayer, dashPaintColor);
        }
        else
        {
            Vector3 playerWorldPos = player.transform.position;

            Vector3Int playerCell = tilemap.WorldToCell(playerWorldPos);
            Vector3Int cellPos = tilemap.WorldToCell(position);

            Color color = player.GetComponentInChildren<SpriteRenderer>().color;

            TilemapMaskController._instance.PaintStamp(cellPos, playerCell, color);
        }
    }
    #endregion

    #region EmitPartciles
    public void EmitJumpParticles()
    {
        //jumpParticleSystem.Emit(3);   
    }
    
    public void EmitDashParticles()
    {
        //dashParticleSystem.Emit(5);   
    }
    
    public void EmitExpandedParticles()
    {
        //expandedParticleSystem.Emit(5);   
    }
    
    public void EmitBubbleParticles()
    {
        /*bubbleParticleSystem.transform.position = bubbleTransform.position;
        bubbleParticleSystem.Emit(5);*/   
    }
    #endregion

    public void PlaceSplat(Vector3 position, Vector3 normal, Color color)
    {
        if (!_shadersEnabled)
        {
            GameObject splat =
            Instantiate(splatPrefab, position, Quaternion.identity) as GameObject;
            GetDecalChunk(splat.transform);
            Splat splatScript = splat.GetComponent<Splat>();
            splatScript.Initialize(Splat.SplatLoacation.Foreground, currentLayer, color, normal);
        }
        else
        {
            Vector3 playerWorldPos = player.transform.position;
            Vector3Int playerCell = tilemap.WorldToCell(playerWorldPos);

            Vector3Int cellPos = tilemap.WorldToCell(position);

            Color colorSplat = player.GetComponentInChildren<SpriteRenderer>().color;

            TilemapMaskController._instance.PaintStamp(cellPos, playerCell, colorSplat);
        }
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
}
