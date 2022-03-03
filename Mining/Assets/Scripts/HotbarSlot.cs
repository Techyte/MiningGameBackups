using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    [SerializeField] int slotID;
    [SerializeField] GameObject blockInSlot;
    [SerializeField] Image blockRenderer, selectedRenderer;
    private void Update()
    {
        if (blockInSlot != Hotbar.emptyBlock)
        {
            blockRenderer.sprite = blockInSlot.GetComponent<BlockManager>().block.texture;
        }

        if (Hotbar.currentSelectedSlot == slotID)
        {
            selectedRenderer.gameObject.SetActive(true);

            if (blockInSlot != Hotbar.emptyBlock)
            {
                blockRenderer.sprite = blockInSlot.GetComponent<BlockManager>().block.texture;
                BuildingManager.currentBuildingBlock = blockInSlot;
            }
        }
        else
        {
            selectedRenderer.gameObject.SetActive(false);
        }
    }
}
