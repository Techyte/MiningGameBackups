using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapCollider2D))]
public class Chunk : MonoBehaviour
{
    private List<BlockRepresentation> Blocks = new List<BlockRepresentation>();
    private Tilemap chunkMap;
    private TilemapCollider2D _collider2D;

    public void Initilize()
    {
        if (!_collider2D)
            _collider2D = GetComponent<TilemapCollider2D>();
        if (!chunkMap)
            chunkMap = GetComponent<Tilemap>();
    }

    public void UpdateChunk()
    {
        _collider2D = GetComponent<TilemapCollider2D>();
        
        //chunkMap.ClearAllTiles();
        for (int i = 0; i < Blocks.Count; i++)
        {
            Vector3Int SpawnPos = Vector3Int.RoundToInt(Blocks[i].cords);
            
            chunkMap.SetTile(SpawnPos, Blocks[i].block.tile);
        }
        _collider2D.ProcessTilemapChanges();
    }

    public void AddBlockToChunk(Block block, Vector2 cords)
    {
        BlockRepresentation blockToAdd = new BlockRepresentation(block, cords);
        
        Blocks.Add(blockToAdd);
    }
}

public class BlockRepresentation
{
    public Vector2 cords;
    public Block block;

    public BlockRepresentation(Block block, Vector2 cords)
    {
        this.block = block;
        this.cords = cords;
    }
}
