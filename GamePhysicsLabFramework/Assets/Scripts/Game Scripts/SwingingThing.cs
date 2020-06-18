using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingThing : MonoBehaviour
{
    Vector3 anchor;
    Vector3 spawnPoint;

    private Particle3D particle;

    private void Start()
    {
        Destroy(gameObject, 20f);
    }

    public void Init(Vector3 spawn, Vector3 anchorPoint)
    {
        particle = GetComponent<Particle3D>();

        particle.position = spawn;
        anchor = anchorPoint;
    }

    public void HandleACollision(CollisionHull3D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        particle.AddForce(ForceGenerator.GenerateForce_spring(particle.position, anchor, 8f, 0.5f));
        //particle.AddForce(ForceGenerator.GenerateForce_Gravity(particle.GetMass(), -5f, Vector3.up));
    }
}
