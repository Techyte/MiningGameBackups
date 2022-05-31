using System.Collections.Generic;
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

    public void SaveWorldData()
    {
        SaveAndLoad<WorldData>.SaveJson(currentWorldData, Application.persistentDataPath  + "/world.json");
    }

    public void UpdateChunks()
    {
        Chunk currentChunk = new GameObject("Chunk").AddComponent<Chunk>();
        
        for (int i = currentPlayerChunk - (renderDistance/2); i < currentPlayerChunk + (renderDistance/2); i++)
        {
            if (currentWorldData.chunks.TryGetValue(i, out ChunkData chunk))
            {
                Debug.Log("Already Generated Chunk");
                currentChunk = CreatNewChunk(chunk);
            }
            else
            {
                Debug.Log("Generating A New Chunk");
                currentChunk = GenerateNewChunk(i);
                Debug.Log(currentChunk.transform.position.x / 16);
                //currentWorldData.chunks.Add((int)currentChunk.transform.position.x/16, Chunk.ConvertToChunkData(currentChunk));
            }

            currentChunk.transform.position = new Vector3(i * 16, 0, 0);
            currentChunk.UpdateChunk();
        }
    }

    public Chunk GenerateNewChunk(int xOffset)
    {
        Chunk currentChunk = new GameObject("Chunk").AddComponent<Chunk>();
        currentChunk.transform.parent = chunkHolder;
        
        currentChunk.gameObject.AddComponent<TilemapRenderer>();
        
        for (int x = 0; x < 16; x++)
        {
            int trueBuildX = x + xOffset;
            int height = Mathf.RoundToInt(seed * Mathf.PerlinNoise(trueBuildX / smoothness, seed));
            int minStoneSpawnDistance = height - minStoneheight;
            int maxStoneSpawnDistance = height - maxStoneHeight;
            int totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);

            //Perlin noise.
            for (int y = 0; y < height; y++)//This will help spawn a tile on the y axis
            {
                if (y < totalStoneSpawnDistance)
                {
                    spawnObj(stoneBlock, x, y, currentChunk);
                }
                else
                {
                    spawnObj(dirtBlock, x, y, currentChunk);
                }
            }

            if (totalStoneSpawnDistance == height)
            {
                spawnObj(stoneBlock, x, height, currentChunk);
            }
            else
            {
                spawnObj(grassBlock, x, height, currentChunk);
                int randomInt = Random.Range(0, 25);
                if (randomInt == 1)
                {
                    // spawnObj(tree, x, height + 1, currentChunk);
                }
            } 
        }
        
        return currentChunk;
    }
    
    void spawnObj(Block obj, int width, int height, Chunk holder)
    {
        holder.AddBlockToChunk(obj, new Vector2(width, height));
    }
    
    Chunk CreatNewChunk(ChunkData chunkSRC)
    {
        Chunk newChunk = new GameObject("Chunk").AddComponent<Chunk>();
        newChunk.transform.parent = chunkHolder;

        newChunk.Blocks = chunkSRC.Blocks;
        
        newChunk.gameObject.AddComponent<TilemapRenderer>();

        return newChunk;
    }
}

[System.Serializable]
public class WorldData
{
    public Dictionary<int, ChunkData> chunks = new Dictionary<int, ChunkData>();
}
