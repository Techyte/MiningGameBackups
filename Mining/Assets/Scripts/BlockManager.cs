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
                    Tilemap tilemap = colliders[0].GetComponent<Tilemap>();
                    Chunk chunk = colliders[0].GetComponent<Chunk>();
                    Vector2Int blockPos = new Vector2Int();
                    blockPos.x = Mathf.FloorToInt(blockMousePos.transform.localPosition.x);
                    blockPos.y = Mathf.FloorToInt(blockMousePos.transform.localPosition.y);

                    if (tilemap.GetTile((Vector3Int)blockPos))
                    {
                        Debug.Log(tilemap.GetTile((Vector3Int)blockPos));
                        chunk.DamageBlock(blockPos, 1);
                    }
                }
            }
        }
    }
}
