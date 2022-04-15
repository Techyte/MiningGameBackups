using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    #region variables
    [SerializeField] int initialChunkGenAmount;
    [SerializeField] int minStoneheight, maxStoneHeight;
    [SerializeField] GameObject chunkHolder;
    [SerializeField] GameObject dirt, grass, stone, tree;

    [Range(0, 100)]
    [SerializeField] float heightValue, smoothness;

    [SerializeField] float seed;
    #endregion

    #region Start
    void Start()
    {
        seed = Random.Range(-1000000, 1000000);
        Generation();
    }
    #endregion

    #region Generation of blocks
    void Generation()
    {
        GameObject currentChunk = CreatNewChunk();
        for (int x = 0; x < initialChunkGenAmount * 16; x++)//This will help spawn a tile on the x axis
        {
            if (x != 0)
            {
                if (IsDivisibleBy(x))
                {
                    currentChunk = CreatNewChunk();
                }
            }

            int height = Mathf.RoundToInt(heightValue * Mathf.PerlinNoise(x / smoothness, seed));
            int minStoneSpawnDistance = height - minStoneheight;
            int maxStoneSpawnDistance = height - maxStoneHeight;
            int totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);

            //Perlin noise.
            for (int y = 0; y < height; y++)//This will help spawn a tile on the y axis
            {
                if (y < totalStoneSpawnDistance)
                {
                    spawnObj(stone, x, y, currentChunk);
                }
                else
                {
                    spawnObj(dirt, x, y, currentChunk);
                }
            }

            if (totalStoneSpawnDistance == height)
            {
                spawnObj(stone, x, height, currentChunk);
            }
            else
            {
                spawnObj(grass, x, height, currentChunk);
                int randomInt = Random.Range(0, 25);
                if (randomInt == 1)
                {
                    // spawnObj(tree, x, height + 1, currentChunk);
                }
            }
        }
    }
    #endregion

    bool IsDivisibleBy(int n)
    {
        if ((n & 15) == 0)
            return true;

        return false;
    }

    GameObject CreatNewChunk()
    {
        GameObject newChunk = new GameObject("Chunk");
        newChunk.transform.parent = chunkHolder.transform;

        return newChunk;
    }

    #region Spawn function
    void spawnObj(GameObject obj, int width, int height, GameObject holder)//What ever we spawn will be a child of our procedural generation gameObj
    {
        GameObject objHolder = obj;
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);

        obj.transform.parent = holder.transform;
    }
    #endregion

}
