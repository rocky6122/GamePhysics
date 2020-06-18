using UnityEngine;
using System.Collections;

public class EndSceneScore : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text score;

    // Use this for initialization
    void Start()
    {
        score.text = PointManager.GetPoints().ToString();
    }

}
