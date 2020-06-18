using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Transform player;

    public Transform spawnNode;

    public CollisionManager manager;

    Asteroid temp;
    SwingingThing tempSwing;

    const float RADIUS = 4f;

    const float ASTEROID_TIME = 1.5f;

    float timer;
    int asteroidType;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= ASTEROID_TIME)
        {
            asteroidType = Random.Range(0, 3);

            Vector3 pos = spawnNode.position + (Vector3)Random.insideUnitCircle.normalized * RADIUS;

            switch (asteroidType)
            {
                case 0: //Sphere
                    temp = Instantiate((GameObject)Resources.Load("Asteroid"), pos, Quaternion.identity).GetComponentInChildren<Asteroid>();

                    temp.Initialize(pos, player.position, manager);

                    manager.AddCollider(temp.GetComponent<SphereCollisionHull3D>());
                    break;

                case 1: // Cube
                    temp = Instantiate((GameObject)Resources.Load("Cube"), pos, Quaternion.identity).GetComponentInChildren<Asteroid>();

                    temp.Initialize(pos, player.position, manager);

                    manager.AddCollider(temp.GetComponent<AxisAlignBoundingBoxHull3D>());
                    break;

                case 2: //Spring Thing
                    tempSwing = Instantiate((GameObject)Resources.Load("SpringThing"), pos, Quaternion.identity).GetComponentInChildren<SwingingThing>();

                    tempSwing.Init(pos, spawnNode.position);

                    manager.AddCollider(tempSwing.GetComponent<SphereCollisionHull3D>());
                    break;
                default:
                    break;
            }

            timer = 0;
        }
    }
}