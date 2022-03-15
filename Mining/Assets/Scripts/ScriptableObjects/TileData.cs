using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile Data", menuName = "TileData")]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;
    public float durability;
}
