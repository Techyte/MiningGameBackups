using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapCollider2D))]
public class Chunk : MonoBehaviour
{
    public Dictionary<Vector2, BlockRepresentation> Blocks = new Dictionary<Vector2, BlockRepresentation>();
    private Tilemap chunkMap;
    private TilemapCollider2D _collider2D;

    public void UpdateChunk()
    {
        if (!_collider2D)
            _collider2D = GetComponent<TilemapCollider2D>();
        if (!chunkMap)
            chunkMap = GetComponent<Tilemap>();
        
        chunkMap.ClearAllTiles();
        foreach(BlockRepresentation block in Blocks.Values)
        {
            Vector3Int SpawnPos = Vector3Int.RoundToInt(block.cords);

            chunkMap.SetTile(SpawnPos, block.block.tile);
        }
        _collider2D.ProcessTilemapChanges();
    }

    public void AddBlockToChunk(Block block, Vector2 cords)
    {
        BlockRepresentation blockToAdd = new BlockRepresentation(block, cords, 5);
        
        Blocks.Add(cords, blockToAdd);
    }

    public void DamageBlock(Vector2 destroyCords, int damage)
    {
        if (Blocks.TryGetValue(destroyCords, out BlockRepresentation block))
        {
            block.durability--;
            if(block.durability <= 0)
            {
                Debug.Log("Block Broke");
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

public struct ChunkData
{
    public Dictionary<Vector2, BlockRepresentation> Blocks;

    public ChunkData(Dictionary<Vector2, BlockRepresentation> blocks)
    {
        this.Blocks = blocks;
    }
}
