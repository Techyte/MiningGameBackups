using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private void Update()
    {
        if (Player.player.transform.position.x - gameObject.transform.position.x == 32)
        {
            print("Chunk is ready to be unloaded");
        }
    }
}
