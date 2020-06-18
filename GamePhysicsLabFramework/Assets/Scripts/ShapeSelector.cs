using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSelector : MonoBehaviour
{
    public static ShapeSelector instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject[] shapes;
    private int currentIndex = 0;

    private Vector3[] vectors;

    public void Start()
    {
        vectors = new Vector3[2];
    }

    public void ChangeVector(int vector, int field, float value)
    {
        switch (field)
        {
            case 0:
                vectors[vector].x = value;
                break;
            case 1:
                vectors[vector].y = value;
                break;
            case 2:
                vectors[vector].z = value;
                break;
            default:
                break;
        }
    }

    public void ChangeActiveShape(TMPro.TMP_Dropdown dropdown)
    {
        if (currentIndex != dropdown.value)
        {
            shapes[currentIndex].SetActive(false);
            currentIndex = dropdown.value;

            if (currentIndex != 0)
            {
                shapes[currentIndex].SetActive(true);

                shapes[currentIndex].transform.rotation = Quaternion.identity;

                shapes[currentIndex].GetComponent<Particle3D>().SetStartUpTorque(vectors[0], vectors[1]);
            }
        }
    }
}
