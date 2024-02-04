using UnityEngine;

public class Blocks : MonoBehaviour
{
    public static Blocks Instance;

    public Block Base_Grass;
    public Block Base_Dirt;
    public Block Base_Stone;
    
    private void Awake()
    {
        Instance = this;
    }
}
