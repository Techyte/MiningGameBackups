using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningManager : MonoBehaviour
{
    [SerializeField] public Transform playerHeadLower, playerHead, playerMiddle;
    [SerializeField] public static GameObject target, focus;
    public float gameReach, blockSize;
    public Animator anim;
    public static float realReach;
    public LayerMask block;

    private void Start()
    {
        realReach = gameReach *= blockSize;
    }

    private void Update()
    {
        //if the player is locked on to a block then run the code
        if (target != null)
        {
            //If the block is above the player
            if (target.transform.position.y >= playerMiddle.transform.position.y)
            {
                //Send a RayCast out to see if we can mine the block
                Vector2 direction = (target.transform.position - playerHead.position).normalized;

                RaycastHit2D hit = Physics2D.Raycast(playerHead.transform.position, direction, realReach, block);

                Debug.DrawRay(playerHead.transform.position, direction * 100f, Color.red);
                //If the block is out of range
                if (!hit)
                {
                    print("Out Of Range");
                    target = null;
                    return;
                }

                //If we can mine the block
                if (hit.transform.gameObject.tag == "Minable")
                {
                    anim.SetBool("PickSwing", true);
                    focus = target;
                    target.GetComponent<BlockManager>().Mine();
                }
                target = null;
            }
            //If the block is below the player
            else if (target.transform.position.y <= playerMiddle.transform.position.y)
            {
                //Send a RayCast out to see if we can mine the block
                Vector2 direction = (target.transform.position - playerHeadLower.position).normalized;

                RaycastHit2D hit = Physics2D.Raycast(playerHeadLower.transform.position, direction, realReach, block);

                Debug.DrawRay(playerHeadLower.transform.position, direction * realReach, Color.red);
                //If the block is out of reach
                if (!hit)
                {
                    print("Out Of Range");
                    target = null;
                    return;
                }

                //If we can mine the block
                if (hit.transform.gameObject.tag == "Minable")
                {
                    anim.SetBool("PickSwing", true);
                    focus = target;
                    target.GetComponent<BlockManager>().Mine();
                }
                target = null;
            }
        }
    }
}
