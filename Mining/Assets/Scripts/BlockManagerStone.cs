using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlockManagerStone : MonoBehaviour
{
    public SpriteRenderer overlay;
    public SpriteRenderer sr;
    public Block block;
    public float durability;
    Sprite[] breakTextures;
    GameObject[] Posdrops;
    bool mouseIsOnBlock;
    public LayerMask player;
    public Transform plya;
    
    void Start()
    {
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


    private void Update()
    {
        plya = Player.playerPos.transform;

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
            Destroy(gameObject);
        }

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

    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds((float)0.01);
        gameObject.tag = "Block";
    }

    public void Mine()
    {
        durability--;
    }
}
