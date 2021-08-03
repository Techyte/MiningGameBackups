using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public Transform player;
    public LayerMask block;
    public Camera MainCamera;
    public GameObject mousePos;
    public GameObject obj;
    public GameObject[] blocks;
    public GameObject stoneHolder;
    public Vector3 buildOfsetleft;
    public Vector3 buildOfsetright;
    public Vector3 buildOfsetup;
    public Vector3 buildOfsetdown;

    void Update()
    {
        obj = blocks[0];
        if (Input.GetMouseButtonDown(1))
        {
            mousePos.transform.position = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos.transform.position - player.transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, Player.realReach, block);
            Debug.DrawRay(player.transform.position, direction, Color.red, 1);
            if (hit)
            {
                GameObject blockHit = hit.transform.gameObject;

                player.transform.parent = hit.transform;

                Vector3 distance = player.transform.position - hit.transform.position;
                player.transform.parent = null;

                if (distance.y > 0)
                {
                    print("Block was placed up");
                    obj = Instantiate(obj, blockHit.transform.position - buildOfsetup, Quaternion.identity);
                    obj.transform.parent = stoneHolder.transform;
                    return;
                }

                if (distance.y < 0)
                {
                    print("Block was placed down");
                    obj = Instantiate(obj, blockHit.transform.position - buildOfsetdown, Quaternion.identity);
                    obj.transform.parent = stoneHolder.transform;
                    return;
                }

                if (distance.x < 0)
                {
                    print("Block was placed on the right");
                    obj = Instantiate(obj, blockHit.transform.position - buildOfsetleft, Quaternion.identity);
                    obj.transform.parent = stoneHolder.transform;
                    return;
                }

                if (distance.x > 0)
                {
                    print("Block was placed on the right");
                    obj = Instantiate(obj, blockHit.transform.position - buildOfsetright, Quaternion.identity);
                    obj.transform.parent = stoneHolder.transform;
                    return;
                }
            }
        }
    }
}
