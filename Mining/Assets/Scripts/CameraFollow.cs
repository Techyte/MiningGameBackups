using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //To make the camera follow the player
    public Transform followTransform;
    void Update()
    {
        transform.position = new Vector3(followTransform.position.x, followTransform.position.y, transform.position.z);
    }
}
