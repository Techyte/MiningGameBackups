using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlockManager : MonoBehaviour
{
    //Variables
    public SpriteRenderer overlay;
    public SpriteRenderer sr;
    public Block block;
    public float durability;
    float maxDurability;
    Sprite[] breakTextures;
    GameObject[] Posdrops;
    bool mouseIsOnBlock;
    [SerializeField] SpriteRenderer breakingOverlay;

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
        sr.sprite = block.texture;
        durability = block.durability;
        breakTextures = GameManager.blockBreakingTextures;
    }

    //for when the mouse clicks on the block
    private void OnMouseDown()
    {
        gameObject.tag = "Minable";
        MiningManager.target = this.gameObject;
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
        if (MiningManager.focus == this.gameObject)
        {
            overlay.sprite = block.selectedTexture;
        }

        //If the player is NOT focusing on the block and the mouse is not hovering over it
        if (MiningManager.focus != this.gameObject && mouseIsOnBlock == false)
        {
            overlay.sprite = null;
        }

        //If the durability gets to 0
        if (durability <= 0)
        {
            Drop();
            Destroy(gameObject);
        }

        int percentageDurability = Mathf.RoundToInt(PercentageOf(durability, maxDurability));
        if(percentageDurability < 25)
        {
            breakingOverlay.sprite = breakTextures[0];
        }
        else if(percentageDurability < 50)
        {
            breakingOverlay.sprite = breakTextures[1];
        }
        else if(percentageDurability < 75)
        {
            breakingOverlay.sprite = breakTextures[2];
        }
        #endregion
    }
    #endregion

    float PercentageOf(float smaller, float max)
    {
        float resault = max / smaller;
        return resault;
    }

    #region IEnumerators
    IEnumerator wait()
    {
        yield return new WaitForSeconds((float)0.01);
        gameObject.tag = "Block";
    }
    #endregion

}
