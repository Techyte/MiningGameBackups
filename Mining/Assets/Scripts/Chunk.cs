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

    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ChunkManager.Singleton.currentPlayerChunk = Mathf.RoundToInt(transform.position.x / 16);
    }

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
            Vector3Int SpawnPos = Vector3Int.RoundToInt(block.cords);

            chunkMap.SetTile(SpawnPos, block.block.tile);
        }
        _collider2D.ProcessTilemapChanges();
    }

    public void AddBlockToChunk(Block block, Vector2 cords)
    {
        Blocks.Add(cords, new BlockRepresentation(block, cords, 5));
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

public class ChunkData
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
}
