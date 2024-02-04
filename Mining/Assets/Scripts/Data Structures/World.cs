using System;
using System.Collections.Generic;
using Data_Structures;
using UnityEngine;

[Serializable]
public class World
{
    public Dictionary<int, ChunkData> chunks = new Dictionary<int, ChunkData>();
    public Vector3 playerPos;
    public int seed;
    public int playerChunkId;

    public void Init(int seed, Vector3 playerPos, Dictionary<int, ChunkData> chunks, int playerChunkId)
    {
        this.seed = seed;
        this.playerPos = playerPos;
        this.chunks = chunks;
        this.playerChunkId = playerChunkId;
    }

    public void AddGeneratedChunk(ChunkData data)
    {
        chunks.Add(data.id, data);
    }
}
