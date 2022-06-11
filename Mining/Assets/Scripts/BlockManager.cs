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
            Collider2D[] colliders = Physics2D.OverlapCircleAll(blockMousePos.position, .1f);

            if (colliders.Length > 0)
            {
                blockMousePos.transform.parent = colliders[0].transform;
                if (colliders[0].GetComponent<Tilemap>())
                {
                    Vector2Int blockPos = new Vector2Int();
                    blockPos.x = Mathf.FloorToInt(blockMousePos.transform.localPosition.x);
                    blockPos.y = Mathf.FloorToInt(blockMousePos.transform.localPosition.y);

                    Vector3 vector2BlockPos = new Vector3(blockPos.x, blockPos.y, 0);

                    Vector2 direction = (vector2BlockPos - player.position).normalized;

                    RaycastHit2D hit = Physics2D.Raycast(player.position, direction, reach);
                    if (hit)
                    {
                        Vector2Int flooredMousePos = Vector2Int.RoundToInt(blockMousePos.transform.position);
                        Vector2Int floordedHitPos = Vector2Int.RoundToInt(hit.point);
                        floordedHitPos.y -= 1;

                        if (flooredMousePos == floordedHitPos)
                        {
                            Tilemap tilemap = colliders[0].GetComponent<Tilemap>();
                            Chunk chunk = colliders[0].GetComponent<Chunk>();
                            if (tilemap.GetTile((Vector3Int)blockPos))
                            {
                                chunk.DamageBlock(blockPos, 1);
                            }
                        }
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
