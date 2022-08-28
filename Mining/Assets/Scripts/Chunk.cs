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
        foreach(BlockRepresentation blockRepresentation in Blocks.Values)
        {
            Vector3Int SpawnPos = Vector3Int.RoundToInt(blockRepresentation.cords);

            chunkMap.SetTile(SpawnPos, blockRepresentation.GetCorrectSprite());
        }
        _collider2D.ProcessTilemapChanges();
    }

    public void AddBlockToChunk(Block block, Vector2 cords)
    {
        Blocks.Add(cords, new BlockRepresentation(block, cords, block.damage));
    }

    public void DamageBlock(Vector2 destroyCords, int damage)
    {
        if (Blocks.TryGetValue(destroyCords, out BlockRepresentation block))
        {
            block.damage+=damage;
            if(block.damage >= block.block.Tiles.Length)
            {
                Blocks.Remove(destroyCords);
            }
            UpdateChunk();
        }
    }

    public void LoadAdditions(Dictionary<Vector2, BlockRepresentation> additions)
    {
        foreach (var addition in additions)
        {
            Blocks.Add(addition.Key, addition.Value);
        }
    }
    
    public void LoadDeletions(Vector2[] deletions)
    {
        foreach (var deletion in deletions)
        {
            Blocks.Remove(deletion);
        }
    }
}

[Serializable]
public class BlockRepresentation
{
    public Vector2 cords;
    public Block block;
    public int damage;

    public BlockRepresentation(Block block, Vector2 cords, int damage)
    {
        this.block = block;
        this.cords = cords;
        this.damage = damage;
    }
    
    public TileBase GetCorrectSprite()
    {
        for (int i = 0; i < block.Tiles.Length; i++)
        {
            if (damage == i)
            {
                return block.Tiles[i];
            }
        }

        return block.Tiles[0];
    }
}

[Serializable]
public class ChunkData : ISerializationCallbackReceiver
{
    public Dictionary<Vector2, BlockRepresentation> additions;
    public Vector2[] deletions;

    public ChunkData()
    {
        additions = new Dictionary<Vector2, BlockRepresentation>();
        deletions = new Vector2[0];
    }

    public void AddBlock(Vector2 cords, BlockRepresentation blockRepresentation)
    {
        additions.Add(cords, blockRepresentation);
    }

    public List<Vector2> _keys = new List<Vector2>();
    public List<BlockRepresentation> _values = new List<BlockRepresentation>();

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (var kvp in additions)
        {
            _values.Add(kvp.Value);
            _keys.Add(kvp.Key);
        }
    }

    public void OnAfterDeserialize()
    {
        additions = new Dictionary<Vector2, BlockRepresentation>();

        for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            additions.Add(_keys[i], _values[i]);
    }
}
