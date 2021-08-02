using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public Transform player;
    public LayerMask block;
    public Camera MainCamera;
    public GameObject mousePos;
    public GameObject obj;
    public GameObject stoneHolder;
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mousePos.transform.position = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos.transform.position - player.transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, 10f, block);
            Debug.DrawRay(player.transform.position, direction, Color.red, 1);
            if (hit)
            {
                GameObject blockHit = hit.transform.gameObject;

                obj = Instantiate(obj, blockHit.transform.position, Quaternion.identity);
                obj.transform.parent = stoneHolder.transform;
            }
        }
    }
}
