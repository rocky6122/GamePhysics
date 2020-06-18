using UnityEngine;
using System.Collections;
using TMPro;

public class ShapeSetter : MonoBehaviour
{
    [SerializeField] private GameObject[] shapeObjs = null;
    private int currentIndex = 0;

    TMP_InputField vector;


    public void SetShape(int value)
    {
        int offsetValue = value - 1;

        Debug.Log(offsetValue);

        if (offsetValue != -1)
        {
            shapeObjs[currentIndex].SetActive(false);

            shapeObjs[offsetValue].SetActive(true);

            Particle2D particle = shapeObjs[currentIndex].GetComponent<Particle2D>();

            particle.angularAcceleration = 0;
            particle.angularVelocity = 0;
            particle.rotation = 0;

            currentIndex = offsetValue;
        }
        else if (offsetValue == -1)
        {
            shapeObjs[currentIndex].SetActive(false);
        }
    }
}
