using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    private void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, Mathf.Deg2Rad * 10f);
        transform.LookAt(Vector3.zero);

        if (Input.anyKeyDown)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}
