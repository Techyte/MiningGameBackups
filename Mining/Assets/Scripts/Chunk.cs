using Data_Structures;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    private Tilemap _tilemap;

    private int id => GetData().id;

    private ChunkData _data;

    public ChunkData GetData()
    {
        return _data;
    }
    
    public void Init(ChunkData data)
    {
        _data = data;
        _tilemap = GetComponent<Tilemap>();
        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        foreach (var block in _data.blocks.Values)
        {
            _tilemap.SetTile((Vector3Int)block.LocalPosition, block.Block.tile);
        }
    }
}