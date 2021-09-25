using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    #region Variables
    //Variables
    [SerializeField] Transform player;
    [SerializeField] LayerMask block, buildPoint;
    [SerializeField] Camera MainCamera;
    [SerializeField] GameObject obj;
    [SerializeField] GameObject[] hotbarBlocks;
    [SerializeField] Vector3 buildOfsetdown, buildOfsetup, buildOfsetright, buildOfsetleft;
    [SerializeField] GameObject BuildHolder, mousePos;
    [SerializeField] int buildLimit;
    public static GameObject currentBuildingBlock;
    #endregion

    private void Start()
    {
        currentBuildingBlock = hotbarBlocks[0];
    }

    #region Update
    void Update()
    {

        obj = currentBuildingBlock;

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
                #region Second Raycast
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
                        Vector3 finalBlockPos = blockHit.transform.position - buildOfsetright;
                        //Make sure that when we place the block it wont be above the build limit
                        if (finalBlockPos.y <= buildLimit)
                        {
                            obj = Instantiate(obj, blockHit.transform.position - buildOfsetright, Quaternion.identity);
                            obj.transform.parent = BuildHolder.transform;
                            return;
                        }
                    }

                    //If we hit the right side of the block then place it on the right side
                    if (hitsecond.collider.name == "right")
                    {
                        Vector3 finalBlockPos = blockHit.transform.position - buildOfsetleft;
                        //Make sure that when we place the block it wont be above the build limit
                        if (finalBlockPos.y <= buildLimit)
                        {
                            obj = Instantiate(obj, blockHit.transform.position - buildOfsetleft, Quaternion.identity);
                            obj.transform.parent = BuildHolder.transform;
                            return;
                        }
                    }

                    //If we hit the upper side of the block then place it on the upper side
                    if (hitsecond.collider.name == "up")
                    {
                        Vector3 finalBlockPos = blockHit.transform.position - buildOfsetdown;
                        //Make sure that when we place the block it wont be above the build limit
                        if (finalBlockPos.y <= buildLimit)
                        {
                            obj = Instantiate(obj, blockHit.transform.position - buildOfsetdown, Quaternion.identity);
                            obj.transform.parent = BuildHolder.transform;
                            return;
                        }
                    }

                    //If we hit the bottom of the block then place it on the bottom
                    if (hitsecond.collider.name == "down")
                    {
                        Vector3 finalBlockPos = blockHit.transform.position - buildOfsetup;
                        //Make sure that when we place the block it wont be above the build limit
                        if (finalBlockPos.y <= buildLimit)
                        {
                            obj = Instantiate(obj, blockHit.transform.position - buildOfsetup, Quaternion.identity);
                            obj.transform.parent = BuildHolder.transform;
                            return;
                        }
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
