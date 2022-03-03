using UnityEngine;

public class Hotbar : MonoBehaviour
{
    public static int currentSelectedSlot = 1;
    int nextSlot = 0;
    public static GameObject emptyBlock;
    [SerializeField] Block emptyBlockSRC;

    private void Update()
    {
        #region Inputs

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            nextSlot = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            nextSlot = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            nextSlot = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            nextSlot = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            nextSlot = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            nextSlot = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            nextSlot = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            nextSlot = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            nextSlot = 8;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            nextSlot = 9;
        }

        #endregion

        if (nextSlot != currentSelectedSlot)
        {
            currentSelectedSlot = nextSlot;
        }
    }
}
