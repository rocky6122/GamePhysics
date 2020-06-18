using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    float speed = 8f;
    CollisionManager manager;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 20f);
    }

    private void OnDestroy()
    {
        manager.RemoveCollider(GetComponent<SphereCollisionHull3D>());
    }

    public void HandleACollision(CollisionHull3D col)
    {
        if (col.gameObject.CompareTag("Bullet") || col.gameObject.CompareTag("Ship"))
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector3 pos, Vector3 playerPos, CollisionManager mang)
    {
        GetComponent<Particle3D>().position = pos;

        Vector3 direction = playerPos - pos;

        GetComponent<Particle3D>().velocity = direction.normalized * speed;

        manager = mang;
    }
}