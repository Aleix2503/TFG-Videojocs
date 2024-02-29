using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Transform playerTransform;
    public Transform chunksParent; // Parent transform for organizing chunks
    public float chunkSize = 16f; // Size of each chunk in units

    Dictionary<Vector2Int, GameObject> allChunks = new Dictionary<Vector2Int, GameObject>();
    Vector2Int lastPlayerChunkPos;
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

    void Update()
    {
        // Check which chunk the player is in
        Vector2Int playerChunkPos = GetChunkPosition(playerTransform.position);

        if (playerChunkPos != lastPlayerChunkPos)
        {
            // Activate the new chunk and deactivate the previous ones
            DeactivateChunks(activeChunks, playerChunkPos);
            List<GameObject> newActiveChunks = ActivateNeighboringChunks(playerChunkPos);
            activeChunks = newActiveChunks;

            // Update the last player chunk position
            lastPlayerChunkPos = playerChunkPos;
        }
    }

    Vector2Int GetChunkPosition(Vector3 position)
    {
        int xChunk = Mathf.FloorToInt(position.x / chunkSize) * Mathf.RoundToInt(chunkSize);
        int yChunk = Mathf.FloorToInt(position.y / chunkSize) * Mathf.RoundToInt(chunkSize);
        return new Vector2Int(xChunk, yChunk);
    }

    List<GameObject> ActivateNeighboringChunks(Vector2Int chunkPosition)
    {
        List<GameObject> newActiveChunks = new List<GameObject>();

        // Activate the current chunk
        GameObject currentChunk = ActivateSingleChunk(chunkPosition);
        newActiveChunks.Add(currentChunk);

        // Activate the neighboring chunks
        newActiveChunks.Add(ActivateSingleChunk(chunkPosition + new Vector2Int((int)chunkSize, 0))); // Right
        newActiveChunks.Add(ActivateSingleChunk(chunkPosition - new Vector2Int((int)chunkSize, 0))); // Left
        newActiveChunks.Add(ActivateSingleChunk(chunkPosition + new Vector2Int(0, (int)chunkSize))); // Top
        newActiveChunks.Add(ActivateSingleChunk(chunkPosition - new Vector2Int(0, (int)chunkSize))); // Bottom
        newActiveChunks.Add(ActivateSingleChunk(chunkPosition + new Vector2Int((int)chunkSize, (int)chunkSize))); // Top-right
        newActiveChunks.Add(ActivateSingleChunk(chunkPosition + new Vector2Int(-(int)chunkSize, (int)chunkSize))); // Top-left
        newActiveChunks.Add(ActivateSingleChunk(chunkPosition + new Vector2Int((int)chunkSize, -(int)chunkSize))); // Bottom-right
        newActiveChunks.Add(ActivateSingleChunk(chunkPosition + new Vector2Int(-(int)chunkSize, -(int)chunkSize))); // Bottom-left

        return newActiveChunks;
    }


    GameObject ActivateSingleChunk(Vector2Int chunkPosition)
    {
        // Check if the chunk exists in the dictionary
        if (allChunks.ContainsKey(chunkPosition))
        {
            // Activate the existing chunk
            allChunks[chunkPosition].SetActive(true);
            return allChunks[chunkPosition];
        }
        else
        {
            // Instantiate a new empty GameObject as the chunk and set its position
            GameObject newChunk = new GameObject("Chunk (" + chunkPosition.x + ", " + chunkPosition.y + ")");
            newChunk.transform.position = new Vector3(chunkPosition.x, chunkPosition.y, 0f);
            newChunk.transform.parent = chunksParent;

            // Add the new chunk to the dictionary of active chunks and activate it
            allChunks.Add(chunkPosition, newChunk);
            newChunk.SetActive(true);
            return newChunk;
        }
    }

    void DeactivateChunks(List<GameObject> chunks, Vector2Int newChunkPos)
    {
        // Deactivate all chunks in the list except those neighboring the new chunk
        foreach (GameObject chunk in chunks)
        {
            Vector2Int chunkPos = GetChunkPosition(chunk.transform.position);
            if (chunkPos != newChunkPos && !IsNeighbor(chunkPos, newChunkPos))
            {
                chunk.SetActive(false);
            }
        }
    }

    bool IsNeighbor(Vector2Int chunkPos, Vector2Int newChunkPos)
    {
        int dx = Mathf.Abs(chunkPos.x - newChunkPos.x);
        int dy = Mathf.Abs(chunkPos.y - newChunkPos.y);
    
        // Check if the chunks are adjacent horizontally, vertically, or diagonally
        return (dx == chunkSize && dy == 0) || (dy == chunkSize && dx == 0) ||
               (dx == chunkSize && dy == chunkSize);
    }



    public GameObject GetActiveChunk()
    {
        if (activeChunks.Count == 0)
        {
            Vector2Int playerChunkPos = GetChunkPosition(playerTransform.position);

            GameObject preloadedChunk = ActivateSingleChunk(playerChunkPos);

            activeChunks.Add(preloadedChunk);
        }
        // Return the last chunk in the list (which is the current chunk)
        return activeChunks[0];
    }
}
