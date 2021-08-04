using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlockManager : MonoBehaviour
{
    #region Variables
    //Variables
    public SpriteRenderer overlay;
    public SpriteRenderer sr;
    public Block block;
    public float durability;
    Sprite[] breakTextures;
    GameObject[] Posdrops;
    bool mouseIsOnBlock;
    public LayerMask player;
    #endregion

    #region Main functions

    //Mining function
    public void Mine()
    {
        durability--;
    }

    //Start function
    void Start()
    {
        Posdrops = block.drops;
        breakTextures = block.breakTextures;
        sr.sprite = block.texture;
        durability = block.durability;
    }
    //for when the mouse clicks on the block
    private void OnMouseDown()
    {
        gameObject.tag = "Minable";
        Player.target = this.gameObject;
        StartCoroutine(wait());
    }
    //For when the mouse hovers over the block
    private void OnMouseEnter()
    {
        mouseIsOnBlock = true;
        overlay.sprite = block.hoverTexture;
    }
    
    //For when the mouse goes to another block
    private void OnMouseExit()
    {
        mouseIsOnBlock = false;
        overlay.sprite = null;
    }

    //For dropping items
    void Drop()
    {
        if (Posdrops.Length == 0)
        {
            return;
        }
        Instantiate(Posdrops[0], gameObject.transform.position, Quaternion.identity);
    }

    #endregion

    #region Update
    private void Update()
    {
        #region Logic ifs

        //If the play is currently focusing on the block
        if (Player.focus == this.gameObject)
        {
            overlay.sprite = block.selectedTexture;
        }

        //If the player is NOT focusing on the block and the mouse is not hovering over it
        if (Player.focus != this.gameObject && mouseIsOnBlock == false)
        {
            overlay.sprite = null;
        }

        //If the durability gets to 0
        if (durability <= 0)
        {
            Drop();
            Destroy(gameObject);
        }
        #endregion

        #region Durability switch
        //Checks the current durability of the block and changes its texture accordingly
        switch (durability)
        {
            case 1:
                sr.sprite = breakTextures[0];
                break;
            case 2:
                sr.sprite = breakTextures[1];
                break;
            case 3:
                sr.sprite = breakTextures[2];
                break;
            case 4:
                sr.sprite = breakTextures[3];
                break;
        }
        #endregion

    }
    #endregion

    #region IEnumerators
    IEnumerator wait()
    {
        yield return new WaitForSeconds((float)0.01);
        gameObject.tag = "Block";
    }
    #endregion

}
