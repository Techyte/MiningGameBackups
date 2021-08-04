using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    #region Variables
    //Variables
    public Transform player;
    public LayerMask block;
    public Camera MainCamera;
    public GameObject mousePos;
    public static GameObject obj;
    public GameObject[] blocks;
    public GameObject stoneHolder;
    public Vector3 buildOfsetleft;
    public Vector3 buildOfsetright;
    public Vector3 buildOfsetup;
    public Vector3 buildOfsetdown;
    public LayerMask buildPoint;
    [SerializeField] GameObject objPlaceholder;
    #endregion

    private void Start()
    {
        obj = objPlaceholder;
    }

    #region Update
    void Update()
    {
        //Set the object we want to be building with
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            obj = blocks[0];
            print(obj);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            obj = blocks[1];
            print(obj);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            obj = blocks[2];
            print(obj);
        }

        if (Input.GetMouseButtonDown(1))
        {
            #region Building
            //Send a Raycast in the mouse direction to see if we can place a block
            mousePos.transform.position = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos.transform.position - player.transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, Player.realReach, block);
            Debug.DrawRay(player.transform.position, direction, Color.red, 1);
            //If we can place a block then run this code
            if (hit)
            {
                #region Second Raycase
                //Set BlockHit equal to whatever block we hit so we can use its position later
                GameObject blockHit = hit.transform.gameObject;

                //Send a ray from the block that we hit in the players direction to see what side of the block we should place the block on
                Vector2 directionsecond = (player.transform.position - blockHit.transform.position).normalized;
                RaycastHit2D hitsecond = Physics2D.Raycast(blockHit.transform.position, direction, 0.5f, buildPoint);
                Debug.DrawRay(blockHit.transform.position, directionsecond, Color.red, 1);
                if (hitsecond)
                {
                    #region What side
                    //If we hit the left side of the block then place it on the left side
                    if (hitsecond.collider.name == "left")
                    {
                        obj = Instantiate(obj, blockHit.transform.position - buildOfsetright, Quaternion.identity);
                        obj.transform.parent = stoneHolder.transform;
                        return;
                    }

                    //If we hit the right side of the block then place it on the right side
                    if (hitsecond.collider.name == "right")
                    {
                        obj = Instantiate(obj, blockHit.transform.position - buildOfsetleft, Quaternion.identity);
                        obj.transform.parent = stoneHolder.transform;
                        return;
                    }

                    //If we hit the upper side of the block then place it on the upper side
                    if (hitsecond.collider.name == "up")
                    {
                        obj = Instantiate(obj, blockHit.transform.position - buildOfsetdown, Quaternion.identity);
                        obj.transform.parent = stoneHolder.transform;
                        return;
                    }

                    //If we hit the bottom of the block then place it on the bottom
                    if (hitsecond.collider.name == "down")
                    {
                        obj = Instantiate(obj, blockHit.transform.position - buildOfsetup, Quaternion.identity);
                        obj.transform.parent = stoneHolder.transform;
                        return;
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
        }
    }
    #endregion
}
