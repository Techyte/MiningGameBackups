using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    #region variables
    [SerializeField] int width;
    [SerializeField] int minStoneheight, maxStoneHeight;
    //[SerializeField] GameObject dirt, grass, stone;
    [SerializeField] GameObject dirtHolder, grassHolder, stoneHolder;
    [SerializeField] GameObject dirt, grass, stone;
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
            for (int x = 0; x < width; x++)//This will help spawn a tile on the x axis
            {
                int height = Mathf.RoundToInt(heightValue * Mathf.PerlinNoise(x / smoothness, seed));
                int minStoneSpawnDistance = height - minStoneheight;
                int maxStoneSpawnDistance = height - maxStoneHeight;
                int totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);
                //Perlin noise.
                for (int y = 0; y < height; y++)//This will help spawn a tile on the y axis
                {
                    if (y < totalStoneSpawnDistance)
                    {
                        //spawnObj(stone, x, y);
                        spawnObj(stone, x, y);
                    }
                    else
                    {
                        // spawnObj(dirt, x, y);
                        spawnObj(dirt, x, y);
                    }

                }
                if (totalStoneSpawnDistance == height)
                {
                    // spawnObj(stone, x, height);
                    spawnObj(stone, x, height);
                }
                else
                {
                    //spawnObj(grass, x, height);
                    spawnObj(grass, x, height);
                }

            }
    }
    #endregion

    #region Spawn function
    void spawnObj(GameObject obj, int width, int height)//What ever we spawn will be a child of our procedural generation gameObj
    {
            obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
            GameObject objectToBeSpawned = obj;

            if (objectToBeSpawned = grass)
            {
                gameObject.transform.parent = grassHolder.transform;
            }

            if (objectToBeSpawned = dirt)
            {
                gameObject.transform.parent = dirtHolder.transform;
            }

            if (objectToBeSpawned = stone)
            {
                gameObject.transform.parent = stoneHolder.transform;
            }
    }
    #endregion

}
