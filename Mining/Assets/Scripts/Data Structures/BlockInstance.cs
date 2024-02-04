using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockInstance
{
    public Block Block { get;}
    public Vector2Int WorldPosition { get;}
    public Vector2Int LocalPosition { get;}

    public BlockInstance(Block block, Vector2Int worldPosition, Vector2Int localPosition)
    {
        Block = block;
        WorldPosition = worldPosition;
        LocalPosition = localPosition;
    }
}
