using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockUI : MonoBehaviour
{
    public GameObject obj;
    public Sprite currentBuilding;
    public Image currentBuildingBlockDisplay;
    private void Update()
    {
        obj = BuildingManager.obj;
        currentBuilding = obj.GetComponent<BlockManager>().sr.sprite;
        currentBuildingBlockDisplay.sprite = currentBuilding;
    }
}
