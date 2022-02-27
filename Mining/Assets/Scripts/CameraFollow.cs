using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //To make the camera follow the player
    public Transform followTransform;
    void Update()
    {
        this.transform.position = new Vector3(followTransform.position.x, followTransform.position.y, this.transform.position.z);
    }
}
