using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockUI : MonoBehaviour
{
    [SerializeField] Image[] slots, slotHolders;
    [SerializeField] GameObject[] blocks;
    [SerializeField] Sprite idle, selected;

    private void Update()
    {
        if (BuildingManager.currentBuildingBlock == blocks[0])
        {
            slotHolders[0].sprite = selected;
        }
        else
        {
            slotHolders[0].sprite = idle;
        }

        if (BuildingManager.currentBuildingBlock == blocks[1])
        {
            slotHolders[1].sprite = selected;
        }
        else
        {
            slotHolders[1].sprite = idle;
        }

        if (BuildingManager.currentBuildingBlock == blocks[2])
        {
            slotHolders[2].sprite = selected;
        }
        else
        {
            slotHolders[2].sprite = idle;
        }
    }



}
