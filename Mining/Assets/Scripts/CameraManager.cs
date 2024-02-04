using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float followSpeed;
    [SerializeField] private float yOffset = 3f;

    private void Update()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y + yOffset, transform.position.z);
        float targetSpeed = followSpeed * Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, targetPos, targetSpeed);
    }
}
