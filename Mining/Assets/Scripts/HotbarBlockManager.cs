using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarBlockManager : MonoBehaviour
{
    public static GameObject block1;
    public static GameObject block2;
    public static GameObject block3;
    public static GameObject block4;
    public static GameObject block5;
    public static GameObject block6;
    public static GameObject block7;
    public static GameObject block8;
    public static GameObject block9;
    [SerializeField] GameObject trueBlock1, trueBlock2, trueBlock3, trueBlock4, trueBlock5, trueBlock6, trueBlock7, trueBlock8, trueBlock9;

    [Space]

    public static GameObject selectedBlock;


    private void Update()
    {
        t_MakeBlockValuesCorrect();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Select1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Select2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Select3();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Select4();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Select5();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Select6();
        }

        BuildingManager.currentBuildingBlock = selectedBlock;
    }

    public void Select1()
    {
        selectedBlock = block1;
    }
    public void Select2()
    {
        selectedBlock = block2;
    }
    public void Select3()
    {
        selectedBlock = block3;
    }
    public void Select4()
    {
        selectedBlock = block4;
    }
    public void Select5()
    {
        selectedBlock = block5;
    }
    public void Select6()
    {
        selectedBlock = block6;
    }

    void t_MakeBlockValuesCorrect()
    {
        block1 = trueBlock1;
        block2 = trueBlock2;
        block3 = trueBlock3;
        block4 = trueBlock4;
        block5 = trueBlock5;
        block6 = trueBlock6;
        block7 = trueBlock7;
        block8 = trueBlock8;
        block9 = trueBlock9;
    }
}
