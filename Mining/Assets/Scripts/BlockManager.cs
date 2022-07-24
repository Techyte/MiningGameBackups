using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private Transform blockMousePos;
    [SerializeField] private Transform player;
    [SerializeField] private float reach = 10f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Block grass;

    private void Update()
    {
        blockMousePos.position = playerCam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int blockPos = Vector2Int.FloorToInt(blockMousePos.transform.localPosition);
            blockPos += Vector2Int.right;
            blockPos += Vector2Int.up;

            Vector3 vector2BlockPos = new Vector3(blockPos.x, blockPos.y, 0);

            Vector2 direction = (vector2BlockPos - player.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(player.position, direction, reach);
            Debug.DrawRay(player.position, direction, Color.red, 5);
            if (hit)
            {
                Vector2Int flooredMousePos = Vector2Int.FloorToInt(blockMousePos.transform.position);
                Vector2Int floordedHitPos = Vector2Int.FloorToInt(hit.point);

                Debug.Log(flooredMousePos);
                Debug.Log(floordedHitPos);

                switch (hit.normal)
                {
                    
                }

                if (flooredMousePos == floordedHitPos)
                {
                    Tilemap tilemap = hit.transform.GetComponent<Tilemap>();
                    Chunk chunk = hit.transform.GetComponent<Chunk>();
                    if (tilemap.GetTile((Vector3Int)blockPos))
                    {
                        chunk.DamageBlock(blockPos, 1);
                    }
                }
            }

            blockMousePos.parent = transform;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 direction = (blockMousePos.position - player.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(player.position, direction, reach, layerMask);
            if (hit)
            {
                Chunk buildingChunk = hit.transform.gameObject.GetComponent<Chunk>();

                Vector2 buildPos = Vector2Int.RoundToInt(hit.point);
                buildPos += hit.normal;
                
                buildingChunk.AddBlockToChunk(grass, buildPos);
                buildingChunk.UpdateChunk();
            }
        }
    }
}
