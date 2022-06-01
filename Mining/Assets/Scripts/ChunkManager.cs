using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    [SerializeField] private int currentPlayerChunk;
    [SerializeField] private Transform chunkHolder;
    public WorldData currentWorldData;
    [SerializeField] private int renderDistance;
    
    [SerializeField] int minStoneheight, maxStoneHeight;
    [SerializeField] private Block dirtBlock, grassBlock, stoneBlock;

    [Range(0, 100)]
    [SerializeField] float heightValue, smoothness;

    [SerializeField] private float seed;

    private void Start()
    {
        seed = Random.Range(-1000000, 1000000);
        UpdateChunks();
    }

    private void SaveData()
    {
        SaveAndLoad<WorldData>.SaveJson(currentWorldData, Application.persistentDataPath + "/world.json");
    }

    private void UpdateChunks()
    {
        SaveAndLoad<WorldData>.LoadJson(Application.persistentDataPath + "/world.json");
        
        ChunkData currentChunkData = new ChunkData();
        
        for (int i = currentPlayerChunk - (renderDistance/2); i < currentPlayerChunk + (renderDistance/2); i++)
        {
            if (currentWorldData.chunks.TryGetValue(i, out ChunkData chunkData))
            {
                Debug.Log("Chunk was already there so we can load from it");
                currentChunkData = chunkData;
            }
            else
            {
                Debug.Log("Chunk was not there so we need to generate a new one");
                currentChunkData = GenerateNewChunk(i);
        
                currentWorldData.chunks.Add(i, currentChunkData);
            }

            CreateChunkFromData(currentChunkData, i);
        }
    }

    private ChunkData GenerateNewChunk(int chunkId)
    {
        ChunkData chunkData = new ChunkData();
        
        for (int x = 0; x < 16; x++)
        {
            int trueBuildId = x +(chunkId * 16);
            int height = Mathf.RoundToInt(heightValue * Mathf.PerlinNoise(trueBuildId / smoothness, seed));
            int minStoneSpawnDistance = height - minStoneheight;
            int maxStoneSpawnDistance = height - maxStoneHeight;
            int totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);

            //Perlin noise.
            for (int y = 0; y < height; y++)
            {
                if (y < totalStoneSpawnDistance)
                {
                    chunkData.AddBlock(new Vector2(x, y), new BlockRepresentation(stoneBlock, new Vector2(x, height), 5));
                    Debug.Log("Added a stone block");
                }
                else
                {
                    chunkData.AddBlock(new Vector2(x, y), new BlockRepresentation(dirtBlock, new Vector2(x, height), 5));
                    Debug.Log("Added a dirt block");
                }
            }

            if (totalStoneSpawnDistance == height)
            {
                chunkData.AddBlock(new Vector2(x, height), new BlockRepresentation(stoneBlock, new Vector2(x, height), 5));
            }
            else
            {
                chunkData.AddBlock(new Vector2(x, height), new BlockRepresentation(grassBlock, new Vector2(x, height), 5));
                int randomInt = Random.Range(0, 25);
                if (randomInt == 1)
                {
                    // spawnObj(tree, x, height + 1, currentChunk);
                }
            }   
        }
        
        return chunkData;
    }

    private void CreateChunkFromData(ChunkData sourceData, int x)
    {
        GameObject newChunk = new GameObject("Chunk");
        newChunk.transform.parent = chunkHolder;

        newChunk.AddComponent<Chunk>();
        newChunk.AddComponent<TilemapRenderer>();

        newChunk.GetComponent<Chunk>().Blocks = sourceData.Blocks;

        Vector3 newPos = new Vector3(x * 16, 0, 0);
        newChunk.transform.position = newPos;
        newChunk.GetComponent<Chunk>().UpdateChunk();
    }
}

[System.Serializable]
public class WorldData
{
    public Dictionary<int, ChunkData> chunks = new Dictionary<int, ChunkData>();
}
