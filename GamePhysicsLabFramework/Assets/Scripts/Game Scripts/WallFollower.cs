using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFollower : MonoBehaviour
{

    public Transform player;
    private Particle3D particle;
    float zOffset;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle3D>();

        if (particle == null)
        {
            zOffset = transform.position.z - player.position.z;
        }
        else
        {
            zOffset = particle.position.z - player.position.z;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (particle == null)
        {
            transform.position = new Vector3(player.position.x, transform.position.y, zOffset + player.position.z);
        }
        else
        {
            particle.position = new Vector3(player.position.x, transform.position.y, zOffset + player.position.z);
        }
    }
}
