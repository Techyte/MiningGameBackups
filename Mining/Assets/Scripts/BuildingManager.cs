using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    #region Variables
    //Variables
    [SerializeField] Transform player;
    [SerializeField] LayerMask block;
    [SerializeField] Camera MainCamera;
    [SerializeField] GameObject obj;
    [SerializeField] GameObject mousePos;
    [SerializeField] int buildLimit;
    public static GameObject currentBuildingBlock;
    #endregion

    #region Update
    void Update()
    {
        obj = currentBuildingBlock;
        //To make sure that we have a block selected as the building block (to avoid nullreference exeption error)
        if (currentBuildingBlock)
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
                    PlaceBlock((Vector2)hit.transform.position + hit.normal, hit.transform.parent);
                }
                #endregion
            }
        }

        obj = currentBuildingBlock;
    }
    #endregion

    public void PlaceBlock(Vector3 finalBlockPos, Transform chunk)
    {
        //Make sure that when we place the block it wont be above the build limit
        if (finalBlockPos.y <= buildLimit)
        {
            obj = Instantiate(obj, finalBlockPos, Quaternion.identity);
            obj.transform.parent = chunk;
            return;
        }
    }
}
