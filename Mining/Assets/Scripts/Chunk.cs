using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Tile dirt;
    [SerializeField] private Tile grass;
    [SerializeField] private Tile stone;

    [SerializeField] private int id = 0;

    private void Start()
    {
        Vector3 pos = transform.position;
        pos.x = id * GameManager.Instance.ChunkWidth;
        transform.position = pos;
        
        GenerateTerrain();
    }

    private void Update()
    {
        GenerateTerrain();
    }

    private void GenerateTerrain()
    {
        _tilemap.ClearAllTiles();
        for (int x = 0; x < GameManager.Instance.ChunkWidth; x++)
        {
            int newX = x + (id * GameManager.Instance.ChunkWidth);

            float height = Mathf.PerlinNoise(newX/GameManager.Instance.DetailModifier + 0.1f, newX/GameManager.Instance.DetailModifier + 0.1f) * GameManager.Instance.HeightExageration;

            height *= GameManager.Instance.HeightModifier;

            height = Mathf.RoundToInt(height);
            
            _tilemap.SetTile(new Vector3Int(x, (int)height, 0), grass);

            for (int y = (int)height-1; y > GameManager.Instance.ChunkHeight; y--)
            {
                if (y == GameManager.Instance.ChunkHeight+1)
                {
                    _tilemap.SetTile(new Vector3Int(x, y, 0), stone);
                }
                else
                {
                    _tilemap.SetTile(new Vector3Int(x, y, 0), dirt);   
                }
            }
        }
    }
}
