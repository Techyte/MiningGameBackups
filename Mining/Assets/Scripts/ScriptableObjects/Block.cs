using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Block", menuName = "Block")]
public class Block : ScriptableObject
{
    public Sprite selectedTexture;
    public new string name;
    public int damage;
    
    public TileBase[] Tiles;
    
    public GameObject[] drops;
    public Sprite hoverTexture;
}
