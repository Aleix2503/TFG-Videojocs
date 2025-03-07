using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DecalManager : MonoBehaviour
{
    // Singleton instance
    private static DecalManager _instance;

    // Public property to access the singleton instance
    public static DecalManager Instance
    {
        get
        {
            // If the instance hasn't been set yet, find it in the scene
            if (_instance == null)
            {
                _instance = FindObjectOfType<DecalManager>();

                // If no instance exists in the scene, log a warning
                if (_instance == null)
                {
                    Debug.LogWarning("No instance of DecalManager found in the scene.");
                }
            }

            return _instance;
        }
    }

    public Transform cameraTransform;
    public Transform chunksParent; // Parent transform for organizing chunks
    public float chunkSize = 16f; // Size of each chunk in units

    Dictionary<Vector2Int, GameObject> allChunks = new Dictionary<Vector2Int, GameObject>();
    Vector2Int lastCameraChunkPos;
    List<GameObject> activeChunks = new List<GameObject>();

    // Ensure the instance is not destroyed when loading a new scene
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    Vector2Int GetChunkPosition(Vector3 position)
    {
        int xChunk = Mathf.FloorToInt(position.x / chunkSize) * Mathf.RoundToInt(chunkSize);
        int yChunk = Mathf.FloorToInt(position.y / chunkSize) * Mathf.RoundToInt(chunkSize);
        return new Vector2Int(xChunk, yChunk);
    }
    
    public GameObject GetActiveChunk(Transform transform)
    {
        Vector2Int chunkPosition = GetChunkPosition(transform.position);
        
        if (allChunks.ContainsKey(chunkPosition))
        {
            return allChunks[chunkPosition];
        }
        // Instantiate a new empty GameObject as the chunk and set its position
        GameObject newChunk = new GameObject("Chunk (" + chunkPosition.x + ", " + chunkPosition.y + ")");
        newChunk.transform.position = new Vector3(chunkPosition.x, chunkPosition.y, 0f);
        newChunk.transform.parent = chunksParent;
        
        allChunks.Add(chunkPosition, newChunk);
        newChunk.SetActive(true);
        return newChunk;
    }
}
