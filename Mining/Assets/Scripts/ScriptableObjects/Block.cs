using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Block", menuName = "Block")]
public class Block : ScriptableObject
{
    public Sprite selectedTexture;
    public new string name;
    public float durability;
    public Sprite texture;
    public TileBase tile;
    public GameObject[] drops;
    public Sprite hoverTexture;
}
