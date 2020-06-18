using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollisionHull3D : CollisionHull3D
{
    public ShipCollisionHull3D() : base(CollisionHullType3D.hull_ship)
    { }

    ObjectBoundingBoxHull3D shipHull;
    ObjectBoundingBoxHull3D wingSpan;

    Collision3D collision;

    // Start is called before the first frame update
    void Start()
    {
        collision = new Collision3D();

        particle = GetComponent<Particle3D>();

        shipHull = gameObject.AddComponent<ObjectBoundingBoxHull3D>();
        shipHull.InitValues(new Vector3(0.8f, 1f, 2f));

        wingSpan = gameObject.AddComponent<ObjectBoundingBoxHull3D>();
        wingSpan.InitValues(new Vector3(2f, .5f, 1f));
    }

    public override bool TestCollisionVSSphere(SphereCollisionHull3D other, ref Collision3D c)
    {
        bool pass = false;

        pass = other.TestCollisionVSOBB(shipHull, ref c);

        if (pass)
            return pass;

        pass = other.TestCollisionVSOBB(wingSpan, ref c);

        return pass;
    }

    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull3D other, ref Collision3D c)
    {
        bool pass = false;

        pass = other.TestCollisionVSOBB(shipHull, ref c);

        if (pass)
            return pass;

        pass = other.TestCollisionVSOBB(wingSpan, ref c);

        return pass;
    }

    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull3D other, ref Collision3D c)
    {
        bool passOne = false;
        bool passTwo = false;

        passOne = other.TestCollisionVSOBB(wingSpan, ref c);

        //if (pass)
        //    return pass;

        passTwo = other.TestCollisionVSOBB(shipHull, ref collision);

        if (passOne && passTwo)
        {
            c.contactCount = 2;
            c.contacts[1] = collision.contacts[0];
            return true;
        }
        else if (passOne)
        {
            return true;
        }
        else if(passTwo)
        {
            c = collision;

            return true;
        }

        return false;
    }

    public override bool TestCollisionVSShip(ShipCollisionHull3D other, ref Collision3D c)
    {
        return true;
    }
}
