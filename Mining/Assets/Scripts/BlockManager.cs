using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private Transform blockMousePos;

    private void Update()
    {
        blockMousePos.position = playerCam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(blockMousePos.position, .1f);

            if (colliders.Length > 0)
            {
                blockMousePos.transform.parent = colliders[0].transform;
                if (colliders[0].GetComponent<Tilemap>())
                {
                    Debug.Log(blockMousePos.transform.localPosition);
                }
            }
        }
    }
}
