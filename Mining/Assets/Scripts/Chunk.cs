using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapCollider2D))]
public class Chunk : MonoBehaviour
{
    public Dictionary<Vector2, BlockRepresentation> Blocks = new Dictionary<Vector2, BlockRepresentation>();
    private Tilemap chunkMap;
    private TilemapCollider2D _collider2D;
    private Transform player;

    public void UpdateChunk()
    {   
        if (!_collider2D)
            _collider2D = GetComponent<TilemapCollider2D>();
        if (!chunkMap)
            chunkMap = GetComponent<Tilemap>();
        if (!player)
            player = transform.Find("Player");
        
        chunkMap.ClearAllTiles();
        foreach(BlockRepresentation block in Blocks.Values)
        {
            if (Blocks.TryGetValue(block.cords, out BlockRepresentation blockRepresentation))
            {
                Vector3Int SpawnPos = Vector3Int.RoundToInt(blockRepresentation.cords);

                chunkMap.SetTile(SpawnPos, blockRepresentation.block.tile);
            }
        }
        _collider2D.ProcessTilemapChanges();
    }

    public void AddBlockToChunk(Block block, Vector2 cords)
    {
        Blocks.Add(cords, new BlockRepresentation(block, cords, (int)block.durability));
    }

    public void DamageBlock(Vector2 destroyCords, int damage)
    {
        if (Blocks.TryGetValue(destroyCords, out BlockRepresentation block))
        {
            block.durability--;
            if(block.durability <= 0)
            {
                Blocks.Remove(destroyCords);
                UpdateChunk();
            }
        }
    }

    public static ChunkData ConvertToChunkData(Chunk chunkToConvert)
    {
        ChunkData chunkData = new ChunkData(chunkToConvert.Blocks);
        return chunkData;
    }
}

[Serializable]
public class BlockRepresentation
{
    public Vector2 cords;
    public Block block;
    public int durability;

    public BlockRepresentation(Block block, Vector2 cords, int durability)
    {
        this.block = block;
        this.cords = cords;
        this.durability = durability;
    }
}

[Serializable]
public class ChunkData : ISerializationCallbackReceiver
{
    public Dictionary<Vector2, BlockRepresentation> Blocks;

    public ChunkData(Dictionary<Vector2, BlockRepresentation> blocks)
    {
        this.Blocks = blocks;
    }

    public ChunkData()
    {
        Blocks = new Dictionary<Vector2, BlockRepresentation>();
    }

    public void AddBlock(Vector2 cords, BlockRepresentation blockRepresentation)
    {
        Blocks.Add(cords, blockRepresentation);
    }

    public List<Vector2> _keys = new List<Vector2>();
    public List<BlockRepresentation> _values = new List<BlockRepresentation>();

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (var kvp in Blocks)
        {
            _values.Add(kvp.Value);
            _keys.Add(kvp.Key);
        }
    }

    public void OnAfterDeserialize()
    {
        Blocks = new Dictionary<Vector2, BlockRepresentation>();

        for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            Blocks.Add(_keys[i], _values[i]);
    }
}
