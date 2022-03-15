using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block", menuName = "Block")]
public class Block : ScriptableObject
{
    public Sprite selectedTexture;
    public new string name;
    public float durability;
    public Sprite texture;
    public GameObject[] drops;
    public Sprite hoverTexture;
}
