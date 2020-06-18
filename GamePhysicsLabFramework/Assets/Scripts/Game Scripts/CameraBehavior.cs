using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    Vector3 cameraOffset;
    public Transform following;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = new Vector3(following.position.x, following.position.y + 1, following.position.z - 4f) - following.position;
    }


    void LateUpdate()
    {
        transform.position = following.position + cameraOffset;
    }
}