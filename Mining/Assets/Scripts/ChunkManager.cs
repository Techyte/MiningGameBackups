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
    public WorldData currentWorldData = new WorldData();
    [SerializeField] private int renderDistance;
    
    [SerializeField] int minStoneheight, maxStoneHeight;
    [SerializeField] private Block dirtBlock, grassBlock, stoneBlock;

    [Range(0, 100)]
    [SerializeField] float heightValue, smoothness;

    private void Start()
    {
        UpdateChunks();
    }

    public void SaveWorldData()
    {
        SaveAndLoad<WorldData>.SaveJson(currentWorldData, Application.persistentDataPath  + "/world.json");
    }

    public void UpdateChunks()
    {
        WorldData newWorldData = SaveAndLoad<WorldData>.LoadJson(Application.persistentDataPath  + "/world.json");

        for (int i = currentPlayerChunk - (renderDistance/2); i < currentPlayerChunk + (renderDistance/2); i++)
        {
            if (currentWorldData.chunks.TryGetValue(i, out Chunk chunk))
            {
                CreatNewChunk(chunk);
            }
            else
            {
                GenerateNewChunk(i);
            }
        }
    }

    public void GenerateNewChunk(int xOffset)
    {
        Chunk currentChunk = new GameObject("Chunk").AddComponent<Chunk>();
        currentChunk.transform.parent = chunkHolder;

        Vector3 newPos = new Vector3(xOffset, 0, 0);
        
        currentChunk.transform.position = newPos;
        
        currentChunk.Initilize();
        currentChunk.gameObject.AddComponent<TilemapRenderer>();
        
        for (int x = 0; x < 16; x++)
        {
            int trueBuildId = x + xOffset;
            int height = Mathf.RoundToInt(ProceduralGeneration.seed * Mathf.PerlinNoise(trueBuildId / smoothness, ProceduralGeneration.seed));
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
    }
    
    void spawnObj(Block obj, int width, int height, Chunk holder)//What ever we spawn will be a child of our procedural generation gameObj
    {
        holder.AddBlockToChunk(obj, new Vector2(width, height));
    }
    
    Chunk CreatNewChunk(Chunk chunkSRC)
    {
        Chunk newChunk = new GameObject("Chunk").AddComponent<Chunk>();
        newChunk.transform.parent = chunkHolder;

        newChunk.transform.position = chunkSRC.transform.position;
        
        newChunk.Initilize();
        newChunk.gameObject.AddComponent<TilemapRenderer>();

        return newChunk;
    }
}

[System.Serializable]
public struct WorldData
{
    public Dictionary<int, Chunk> chunks;
}
