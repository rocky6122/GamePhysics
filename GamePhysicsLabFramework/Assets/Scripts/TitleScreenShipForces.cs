using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenShipForces : MonoBehaviour
{
    [SerializeField] private Particle3D ship;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ship.AddForce(ForceGenerator.GenerateForce_sliding(new Vector3(0f, -9.8f, 0f), Vector3.up));

        Vector3 grav = ForceGenerator.GenerateForce_Gravity(1f, -4.2f, Vector3.up);

        ship.AddForce(grav);
        ship.AddForce(ForceGenerator.GenerateForce_normal(grav, Vector3.up));
    }
}
