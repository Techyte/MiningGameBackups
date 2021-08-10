using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassToDirtManager : MonoBehaviour
{
    [SerializeField] GameObject dirt, pointOfFire;
    [SerializeField] LayerMask blockLayer;

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, Vector2.up, 1f, blockLayer);
        if (hit)
        {
            SpawnObj(dirt, pointOfFire.transform.position);
            Destroy(this.gameObject);
        }
    }

    private void SpawnObj(GameObject obj, Vector3 pos)
    {
        obj = Instantiate(obj, pos, Quaternion.identity);
    }
}
