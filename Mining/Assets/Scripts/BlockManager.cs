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

            Vector3 vector3BlockPos = new Vector3(blockPos.x, blockPos.y, 0);

            Vector2 direction = (vector3BlockPos - player.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(player.position, direction, reach, layerMask);

            if (hit)
            {
                Tilemap tilemap = hit.transform.GetComponent<Tilemap>();
                Chunk chunk = hit.transform.GetComponent<Chunk>();
                if (chunk && tilemap)
                {
                    blockMousePos.parent = hit.transform;
                    blockPos = Vector2Int.FloorToInt(blockMousePos.transform.localPosition);
                }
                if (tilemap.GetTile((Vector3Int)blockPos))
                {
                    Debug.Log("Damaging");
                    chunk.DamageBlock(blockPos, 1);
                }
                
                blockMousePos.parent = transform;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {   
            Vector2Int blockPos = Vector2Int.FloorToInt(blockMousePos.transform.localPosition);

            Vector3 vector3BlockPos = new Vector3(blockPos.x, blockPos.y, 0);

            Vector2 direction = (vector3BlockPos - player.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(player.position, direction, reach, layerMask);

            if (hit)
            {
                Tilemap tilemap = hit.transform.GetComponent<Tilemap>();
                Chunk chunk = hit.transform.GetComponent<Chunk>();
                if (chunk && tilemap)
                {
                    blockMousePos.parent = hit.transform;
                    blockPos = Vector2Int.FloorToInt(blockMousePos.transform.localPosition);
                }
                if (tilemap.GetTile((Vector3Int)blockPos))
                {
                    Debug.Log("Placing");
                
                    Vector2 buildPos = Vector2Int.RoundToInt(hit.point);
                    buildPos += hit.normal;
                    
                    chunk.AddBlockToChunk(grass, buildPos);
                    chunk.UpdateChunk();
                }
                
                blockMousePos.parent = transform;
            }
            
            /*
            Vector2 direction = (blockMousePos.position - player.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(player.position, direction, reach, layerMask);
            if (hit)
            {
                Chunk buildingChunk = hit.transform.gameObject.GetComponent<Chunk>();

                Vector2 buildPos = Vector2Int.RoundToInt(hit.point);
                buildPos += hit.normal;
                
                buildingChunk.AddBlockToChunk(grass, buildPos);
                buildingChunk.UpdateChunk();
            }*/
        }
    }
}
