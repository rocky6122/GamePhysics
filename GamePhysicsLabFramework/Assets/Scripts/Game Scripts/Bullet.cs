using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 10f;
    CollisionManager manager;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnDestroy()
    {
        manager.RemoveCollider(GetComponent<SphereCollisionHull3D>());
    }

    public void HandleACollision(CollisionHull3D col)
    {
        if (col.gameObject.CompareTag("Asteroid"))
        {
            Destroy(gameObject, .5f);

            ShipController.instance.ChangePoints(PointManager.AddPoints(200));
        }
    }

    public void Initialize(Vector3 currentSpeed, Vector3 pos, Transform spawn, CollisionManager mang)
    {
        GetComponent<Particle3D>().position = pos;

        Vector3 direction = spawn.forward;

        GetComponent<Particle3D>().velocity = currentSpeed + direction.normalized * speed;

        manager = mang;

        manager.AddCollider(GetComponent<SphereCollisionHull3D>());
    }
}
