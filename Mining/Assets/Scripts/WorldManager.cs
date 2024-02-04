using System;
using System.Collections.Generic;
using Data_Structures;
using Techyte.General;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using Random = UnityEngine.Random;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    public World world;

    public Dictionary<int, Chunk> chunks = new Dictionary<int, Chunk>();

    [SerializeField] private int chunkWidth = 10;
    [SerializeField] private int chunkHeight = -10;
    [SerializeField] private float detailModifier = 10;
    [SerializeField] private float heightModifier = 10;
    [SerializeField] private float heightExageration = 2;
    [SerializeField] private int playerChunkId = 0;
    [SerializeField] private int viewDistance = 4;

    [SerializeField] [Range(-999, 999)]
    private float seed;
    
    public int ChunkWidth => chunkWidth;
    public int ChunkHeight => chunkHeight;
    public float DetailModifier => detailModifier;
    public float HeightModifier => heightModifier;
    public float HeightExageration => heightExageration;
    public float Seed => seed;
    public int PlayerChunkId => playerChunkId;
    public int ViewDistance => viewDistance;

    public int testX = 0;

    public void Test()
    {
        float height = Mathf.PerlinNoise(testX/DetailModifier + 0.1f * world.seed, world.seed * world.seed + 0.1f) * HeightExageration;
    
        height *= HeightModifier;
    
        height = Mathf.RoundToInt(height);
        Debug.Log(height);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        WorldGeneration();
    }

    private void WorldGeneration()
    {
        if (File.Exists(Application.persistentDataPath + "/world.json"))
        {
            world = SaveAndLoad<World>.LoadJson(Application.persistentDataPath + "/world.json");
        }
        else
        {
            GenerateNewWorld();
        }
        
        LoadWorldStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
        }
    }

    private void GenerateNewWorld()
    {
        World newWorld = new World();
        
        int seed = Random.Range(-999999999, 999999999);

        Dictionary<int, ChunkData> initialChunks = new Dictionary<int, ChunkData>();

        ChunkData chunk0 = GenerateChunk(-1, seed);
        ChunkData chunk1 = GenerateChunk(0, seed);
        ChunkData chunk2 = GenerateChunk(1, seed);
        
        initialChunks.Add(chunk0.id, chunk0);
        initialChunks.Add(chunk1.id, chunk1);
        initialChunks.Add(chunk2.id, chunk2);
        
        newWorld.Init(seed, new Vector3(0, 10, 0), initialChunks, 0);

        world = newWorld;
    }

    private ChunkData GenerateChunk(int id, int seed)
    {
        ChunkData data = new ChunkData();
        data.id = id;
        data.seed = seed;
        
        for (int x = 0; x < ChunkWidth; x++)
        {
            int newX = x + (id * ChunkWidth);
    
            float height = Mathf.PerlinNoise(newX/DetailModifier + 0.1f * seed, seed * seed + 0.1f) * HeightExageration;
    
            height *= HeightModifier;
    
            height = Mathf.RoundToInt(height);
            
            data.AddBlock(Blocks.Instance.Base_Grass, new Vector2Int(x, (int)height));
    
            for (int y = (int)height-1; y > ChunkHeight; y--)
            {
                if (y == ChunkHeight+1)
                {
                    data.AddBlock(Blocks.Instance.Base_Stone, new Vector2Int(x, y));
                }
                else
                {
                    data.AddBlock(Blocks.Instance.Base_Dirt, new Vector2Int(x, y));
                }
            }
        }

        return data;
    }

    private void LoadWorldStart()
    {
        playerChunkId = world.playerChunkId;

        if (world.chunks.TryGetValue(-1, out ChunkData chunk0))
        {
            LoadChunk(chunk0);
        }
        
        if (world.chunks.TryGetValue(0, out ChunkData chunk1))
        {
            LoadChunk(chunk1);
        }
        
        if (world.chunks.TryGetValue(1, out ChunkData chunk2))
        {
            LoadChunk(chunk2);
        }
    }

    private void LoadChunk(ChunkData data)
    {
        Chunk chunk = new GameObject($"Chunk ({data.id})").AddComponent<Chunk>();
        chunk.transform.parent = transform;

        chunk.gameObject.AddComponent<Tilemap>();
        chunk.gameObject.AddComponent<TilemapRenderer>();
        chunk.gameObject.AddComponent<TilemapCollider2D>();
        
        chunk.Init(data);
    }
}
