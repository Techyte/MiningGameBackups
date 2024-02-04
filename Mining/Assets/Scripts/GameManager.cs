using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int chunkWidth = 10;
    [SerializeField] private int chunkHeight = -10;
    [SerializeField] private float detailModifier = 10;
    [SerializeField] private float heightModifier = 10;
    [SerializeField] private float heightExageration = 2;
    
    public int ChunkWidth => chunkWidth;
    public int ChunkHeight => chunkHeight;
    public float DetailModifier => detailModifier;
    public float HeightModifier => heightModifier;
    public float HeightExageration => heightExageration;

    private void Awake()
    {
        Instance = this;
    }
}
