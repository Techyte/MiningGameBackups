using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlockManager : MonoBehaviour
{
    #region Variables
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

    public void Mine()
    {
        durability--;
    }

    void Start()
    {
        Posdrops = block.drops;
        breakTextures = block.breakTextures;
        sr.sprite = block.texture;
        durability = block.durability;
    }
    private void OnMouseDown()
    {
        gameObject.tag = "Minable";
        Player.target = this.gameObject;
        StartCoroutine(wait());
    }

    private void OnMouseEnter()
    {
        mouseIsOnBlock = true;
        overlay.sprite = block.hoverTexture;
    }

    private void OnMouseExit()
    {
        mouseIsOnBlock = false;
        overlay.sprite = null;
    }

    void Drop()
    {
        if (Posdrops.Length == 0)
        {
            print("nothing to drop");
            return;
        }
        Instantiate(Posdrops[0], gameObject.transform.position, Quaternion.identity);
    }

    #endregion

    #region Update
    private void Update()
    {
        #region Logic ifs

        if (Player.focus == this.gameObject)
        {
            overlay.sprite = block.selectedTexture;
        }

        if (Player.focus != this.gameObject && mouseIsOnBlock == false)
        {
            overlay.sprite = null;
        }

        if (durability <= 0)
        {
            Drop();
            Destroy(gameObject);
        }
        #endregion

        #region Durability switch
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
