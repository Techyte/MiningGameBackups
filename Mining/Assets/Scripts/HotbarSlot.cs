using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSlot : MonoBehaviour
{
    [SerializeField] GameObject block;
    [SerializeField] SpriteRenderer blockDisplay, SlotOverlay;
    [SerializeField] Sprite selected;
    [SerializeField] int slotIdentifyer;
    private void Start()
    {
        SlotOverlay.sprite = null;
    }

    private void Update()
    {
        IdentifySlot();
        if(BuildingManager.currentBuildingBlock == block)
        {
            SlotOverlay.sprite = selected;
        }
        else
        {
            SlotOverlay.sprite = null;
        }
    }

    void IdentifySlot()
    {
        switch (slotIdentifyer)
        {
            case 1:
                blockDisplay.sprite = HotbarBlockManager.block1.GetComponent<BlockManager>().block.texture;
                block = HotbarBlockManager.block1;
                break;
            case 2:
                blockDisplay.sprite = HotbarBlockManager.block2.GetComponent<BlockManager>().block.texture;
                block = HotbarBlockManager.block2;
                break;
            case 3:
                blockDisplay.sprite = HotbarBlockManager.block3.GetComponent<BlockManager>().block.texture;
                block = HotbarBlockManager.block3;
                break;
            case 4:
                blockDisplay.sprite = HotbarBlockManager.block4.GetComponent<BlockManager>().block.texture;
                block = HotbarBlockManager.block4;
                break;
            case 5:
                blockDisplay.sprite = HotbarBlockManager.block5.GetComponent<BlockManager>().block.texture;
                block = HotbarBlockManager.block5;
                break;
            case 6:
                blockDisplay.sprite = HotbarBlockManager.block6.GetComponent<BlockManager>().block.texture;
                block = HotbarBlockManager.block6;
                break;
            case 7:
                blockDisplay.sprite = HotbarBlockManager.block7.GetComponent<BlockManager>().block.texture;
                block = HotbarBlockManager.block7;
                break;
            case 8:
                blockDisplay.sprite = HotbarBlockManager.block8.GetComponent<BlockManager>().block.texture;
                block = HotbarBlockManager.block8;
                break;
            case 9:
                blockDisplay.sprite = HotbarBlockManager.block9.GetComponent<BlockManager>().block.texture;
                block = HotbarBlockManager.block9;
                break;
        }
    }
}
