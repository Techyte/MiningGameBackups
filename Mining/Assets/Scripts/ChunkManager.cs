using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class ChunkManager : MonoBehaviour
{
    #region Singleton
    private static ChunkManager _singleton;
    public static ChunkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(ChunkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }
    #endregion

    public int currentPlayerChunk;
    [SerializeField] private Transform chunkHolder;
    public WorldData currentWorldData;
    [SerializeField] private int renderDistance;
    
    [SerializeField] int minStoneheight, maxStoneHeight;
    [SerializeField] private Block dirtBlock, grassBlock, stoneBlock;

    [Range(0, 100)]
    [SerializeField] float heightValue, smoothness;

    private int lastFrameChunk;

    private List<GameObject> currentylyLoadedChunks = new List<GameObject>();

    private void Start()
    {
        UpdateChunks();
    }

    private void Update()
    {
        if (currentPlayerChunk != lastFrameChunk)
        {
            UpdateChunks();
        }
        
        lastFrameChunk = currentPlayerChunk;
    }

    private void SaveData()
    {
        SaveAndLoad<WorldData>.SaveJson(currentWorldData, Application.persistentDataPath + "/world.json");
    }

    private void UpdateChunks()
    {
        SaveAndLoad<WorldData>.LoadJson(Application.persistentDataPath + "/world.json");

        if (currentWorldData.seed == 0)
        {
            currentWorldData.seed = Random.Range(-1000000, 1000000);
        }
        
        ChunkData currentChunkData = new ChunkData();

        for (int i = 0; i < currentylyLoadedChunks.Count; i++)
        {
            Destroy(currentylyLoadedChunks[i]);
        }
        
        for (int i = currentPlayerChunk - (renderDistance/2); i < currentPlayerChunk + (renderDistance/2); i++)
        {
            if (currentWorldData.chunks.TryGetValue(i, out ChunkData chunkData))
            {
                currentChunkData = chunkData;
            }
            else
            {
                currentChunkData = GenerateNewChunk(i);
        
                currentWorldData.chunks.Add(i, currentChunkData);
            }

            GameObject chunk = CreateChunkFromData(currentChunkData, i);
            currentylyLoadedChunks.Add(chunk);
        }
        
        SaveData();
    }

    private ChunkData GenerateNewChunk(int chunkId)
    {
        ChunkData chunkData = new ChunkData();
        
        for (int x = 0; x < 16; x++)
        {
            int trueBuildId = x +(chunkId * 16);
            int height = Mathf.RoundToInt(heightValue * Mathf.PerlinNoise(trueBuildId / smoothness, currentWorldData.seed));
            int totalStoneSpawnDistance = Random.Range(height - minStoneheight, height - maxStoneHeight);

            for (int y = 0; y < height; y++)
            { 
                if (y < totalStoneSpawnDistance)
                { 
                    chunkData.AddBlock(new Vector2(x, y), new BlockRepresentation(stoneBlock, new Vector2(x, y), 5));
                }else
                {
                    chunkData.AddBlock(new Vector2(x, y), new BlockRepresentation(dirtBlock, new Vector2(x, y), 5));
                }
            }
            
            if (totalStoneSpawnDistance == height)
            {
                chunkData.AddBlock(new Vector2(x, height), new BlockRepresentation(stoneBlock, new Vector2(x, height), 5));
            }
            else
            {
                chunkData.AddBlock(new Vector2(x, height), new BlockRepresentation(grassBlock, new Vector2(x, height), 5));
            }
        }
        
        return chunkData;
    }

    private GameObject CreateChunkFromData(ChunkData sourceData, int x)
    {
        GameObject newChunk = new GameObject("Chunk");
        newChunk.transform.parent = chunkHolder;

        newChunk.AddComponent<Chunk>();
        newChunk.AddComponent<TilemapRenderer>();

        newChunk.GetComponent<Chunk>().Blocks = sourceData.Blocks;

        Vector3 newPos = new Vector3(x * 16, 0, 0);
        newChunk.transform.position = newPos;
        newChunk.GetComponent<Chunk>().UpdateChunk();
        return newChunk;
    }
}

[Serializable]
public class WorldData
{
    public Dictionary<int, ChunkData> chunks = new Dictionary<int, ChunkData>();
    public int seed;
}
