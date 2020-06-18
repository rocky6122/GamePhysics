using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CollisionManager : MonoBehaviour
{
    public Material greenMat;
    public Material whiteMat;


    [SerializeField] private List<CollisionHull3D> colliders = null;

    private void LateUpdate()
    {
        TestAllCollisions();
    }

    private void TestAllCollisions()
    {
        CollisionHull3D.Collision3D collision = new CollisionHull3D.Collision3D();

        foreach (CollisionHull3D col in colliders)
        {
            if (col == null)
                continue;

            foreach (CollisionHull3D otherCol in colliders)
            {
                if (otherCol == null || col == null)
                    continue;

                if (col.collisionsThisUpdate.Contains(otherCol))
                    continue;

                otherCol.collisionsThisUpdate.Add(col);

                if (otherCol != col)
                {
                    CollisionHull3D.TestCollision(col, otherCol, ref collision);

                    //Lab 5: Handle Collision based on Collision data
                    if (collision.status)
                    {
                        HandleCollision(ref collision);
                        Debug.Log("Collision");
                    }

                    collision.Clean();
                }
            }

            col.collisionsThisUpdate.Clear();
        }
    }


    void HandleCollision(ref CollisionHull3D.Collision3D col)
    {
        //Resolve each contact point
        for (int i = 0; i < col.contactCount; ++i)
        {
            col.ResolveSpeed(col.contacts[i]);
        }

        col.a.BroadcastMessage("HandleACollision", col.b);
        col.b.BroadcastMessage("HandleACollision", col.a);
    }

    public void AddCollider(CollisionHull3D col)
    {
        colliders.Add(col);
    }

    public void RemoveCollider(CollisionHull3D col)
    {
        colliders.Remove(col);
    }
}
