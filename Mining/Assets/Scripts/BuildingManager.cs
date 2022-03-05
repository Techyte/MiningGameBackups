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
    Vector3 buildOfsetDown = new Vector3(0, 1, 0), buildOfsetUp = new Vector3(0, -1, 0), buildOfsetRight = new Vector3(-1, 0, 0), buildOfsetLeft = new Vector3(1, 0, 0);
    [SerializeField] GameObject BuildHolder, mousePos;
    [SerializeField] int buildLimit;
    public static GameObject currentBuildingBlock;
    #endregion

    #region Update
    void Update()
    {
        obj = currentBuildingBlock;
        //To make sure that we have a block selected as the building block (to avoid nullreference exeption error)
        if (currentBuildingBlock != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                #region Building
                //Send a Raycast in the mouse direction to see if we can place a block
                mousePos.transform.position = MainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = (mousePos.transform.position - player.transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, MiningManager.realReach, block);
                //If we can place a block then run this code
                if (hit)
                {
                    #region Second Raycast
                    //Set BlockHit equal to whatever block we hit so we can use its position later
                    GameObject blockHit = hit.transform.gameObject;

                    //Send a ray from the block that we hit in the players direction to see what side of the block we should place the block on
                    Vector2 directionsecond = (player.transform.position - blockHit.transform.position).normalized;
                    RaycastHit2D hitsecond = Physics2D.Raycast(blockHit.transform.position, direction, 0.5f, buildPoint);
                    if (hitsecond)
                    {
                        #region What side
                        //If we hit the left side of the block then place it on the left side
                        if (hitsecond.collider.name == "left")
                        {
                            Vector3 finalBlockPos = blockHit.transform.position - buildOfsetLeft;
                            //Make sure that when we place the block it wont be above the build limit
                            if (finalBlockPos.y <= buildLimit)
                            {
                                obj = Instantiate(obj, blockHit.transform.position - buildOfsetRight, Quaternion.identity);
                                obj.transform.parent = BuildHolder.transform;
                                return;
                            }
                        }

                        //If we hit the right side of the block then place it on the right side
                        if (hitsecond.collider.name == "right")
                        {
                            Vector3 finalBlockPos = blockHit.transform.position - buildOfsetLeft;
                            //Make sure that when we place the block it wont be above the build limit
                            if (finalBlockPos.y <= buildLimit)
                            {
                                obj = Instantiate(obj, blockHit.transform.position - buildOfsetLeft, Quaternion.identity);
                                obj.transform.parent = BuildHolder.transform;
                                return;
                            }
                        }

                        //If we hit the upper side of the block then place it on the upper side
                        if (hitsecond.collider.name == "up")
                        {
                            Vector3 finalBlockPos = blockHit.transform.position - buildOfsetDown;
                            //Make sure that when we place the block it wont be above the build limit
                            if (finalBlockPos.y <= buildLimit)
                            {
                                obj = Instantiate(obj, blockHit.transform.position - buildOfsetDown, Quaternion.identity);
                                obj.transform.parent = BuildHolder.transform;
                                return;
                            }
                        }

                        //If we hit the bottom of the block then place it on the bottom
                        if (hitsecond.collider.name == "down")
                        {
                            Vector3 finalBlockPos = blockHit.transform.position - buildOfsetUp;
                            //Make sure that when we place the block it wont be above the build limit
                            if (finalBlockPos.y <= buildLimit)
                            {
                                obj = Instantiate(obj, blockHit.transform.position - buildOfsetUp, Quaternion.identity);
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

        obj = currentBuildingBlock;
    }
    #endregion
}
