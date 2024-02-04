using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data_Structures
{
    [Serializable]
    public class ChunkData
    {
        /// <summary>
        /// Record of every block in the chunk
        /// Position is relative to the chunk
        /// </summary>
        public Dictionary<Vector2Int, BlockInstance> blocks = new Dictionary<Vector2Int, BlockInstance>();
        public int id;
        public int seed;

        public void AddBlock(Block block, Vector2Int position)
        {
            int offset = id * WorldManager.Instance.ChunkWidth;

            Vector2Int localPos = position;
            localPos.x += offset;
            
            BlockInstance instance = new BlockInstance(block, position, localPos);
            
            blocks.Add(localPos, instance);
        }
    }
}