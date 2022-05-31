using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    #region variables
    [SerializeField] int initialChunkGenAmount;
    [SerializeField] int minStoneheight, maxStoneHeight;
    [SerializeField] GameObject chunkHolder;
    [SerializeField] private Block dirtBlock, grassBlock, stoneBlock;
    [SerializeField] private int chunkWidth = 16;
    [SerializeField] private ChunkManager _chunkManager;

    [Range(0, 100)]
    [SerializeField] float heightValue, smoothness;

    public static float seed;
    #endregion

    #region Start
    void Start()
    {
        //seed = Random.Range(-1000000, 1000000);
        //Generation();
    }
    #endregion

    #region Generation of blocks
    void Generation()
    {
        int currentChunkId = 0;
        GameObject currentChunk = CreatNewChunk(currentChunkId);
        for (int i = 0; i < initialChunkGenAmount; i++)
        {
            for (int x = 0; x < chunkWidth; x++)
            {
                int trueBuildId = x +(currentChunkId * chunkWidth);
                int height = Mathf.RoundToInt(heightValue * Mathf.PerlinNoise(trueBuildId / smoothness, seed));
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
            currentChunk.GetComponent<Chunk>().UpdateChunk();
            currentChunkId++;
            
            Debug.Log(_chunkManager.currentWorldData);

            currentChunk = CreatNewChunk(currentChunkId);
        }
    }
    #endregion

    bool IsDivisibleBy(int n)
    {
        if ((n & chunkWidth-1) == 0)
            return true;

        return false;
    }

    GameObject CreatNewChunk(int currentChunkId)
    {
        GameObject newChunk = new GameObject("Chunk " + currentChunkId);
        newChunk.transform.parent = chunkHolder.transform;

        Vector2 newPos = new Vector2(currentChunkId * chunkWidth, 0);
        newChunk.transform.position = newPos;
        
        newChunk.AddComponent<Chunk>().Initilize();
        newChunk.AddComponent<TilemapRenderer>();

        return newChunk;
    }

    #region Spawn function
    void spawnObj(Block obj, int width, int height, GameObject holder)//What ever we spawn will be a child of our procedural generation gameObj
    {/*
        GameObject objHolder = obj;
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);

        obj.transform.parent = holder.transform;*/
        holder.GetComponent<Chunk>().AddBlockToChunk(obj, new Vector2(width, height));
    }
    #endregion

}
