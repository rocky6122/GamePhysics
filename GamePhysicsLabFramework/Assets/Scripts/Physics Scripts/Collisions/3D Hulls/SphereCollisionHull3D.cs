using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollisionHull3D : CollisionHull3D
{
    public SphereCollisionHull3D() : base(CollisionHullType3D.hull_sphere)
    {}

    [Range(0.0f, 100.0f)]
    public float radius;
    public Transform sphereRef;
    public Transform obbRef;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle3D>();
    }

    public override bool TestCollisionVSSphere(SphereCollisionHull3D other, ref Collision3D c)
    {
        bool collisionPass = false;

        //pass if distance between centers <= sum of radii
        //optimized collision passes if (distance between centers) squared <= (sum of radii) squared

        //1. Get center of each particle
        Vector3 thisCenter = Particle.position;
        Vector3 otherCenter = other.particle.position;

        //2. Difference between centers
        Vector3 difference = thisCenter - otherCenter;

        //3. Distance squared = dot(difference, difference)
        float distSquared = Vector3.Dot(difference, difference);

        //4. Sum of radii
        float thisRadius = radius;
        float otherRadius = other.radius;
        float sumOfRadii = thisRadius + otherRadius;

        //5. sqaure sum of radii
        float radiiSumSquared = sumOfRadii * sumOfRadii;

        //6. DO THE TEST: distanceSquared <= SumOfRadiiSquared
        if (distSquared <= radiiSumSquared)
        {
            collisionPass = true;
        }

        if (collisionPass)
        {
            // Setting up collision reference
            c.a = this;
            c.b = other;
            c.status = collisionPass;
            c.contactCount = 1;

            //1. Find point
            Vector3 pointOfContact = difference.normalized * thisRadius + thisCenter;
            //2. Find normal
            Vector3 normalContact = difference * ((1.0f / difference.magnitude));

            //3. Set restitution
            float restitution = 0.15f;
            c.contacts[0] = new Collision3D.Contact(pointOfContact, normalContact, restitution);

            //4. Get Closing Velocity (Function for closing velocity found at https://mathhelpboards.com/calculus-10/calculate-closing-velocities-19402.html

            c.closingVelocity = particle.velocity - other.particle.velocity;
        }

        return collisionPass;
    }

    private bool TestCollisionVsPoint(Vector3 center, Vector3 point, ref Collision3D c)
    {
        bool pass = false;

        //1. Get center of this particle
        Vector3 thisCenter = center;

        //2. Difference between center an point
        Vector3 difference = thisCenter - point;

        //3. Distance squared = dot(difference, difference)
        float distSquared = Vector3.Dot(difference, difference);

        float radiusSquared = radius * radius;

        //DO THE TEST: distanceSquared <= radiusSquared
        if (distSquared <= radiusSquared)
        {
            pass = true;
        }

        if (pass)
        {
            c.status = pass;
            c.contactCount = 1;

            //1. Find point
            Vector3 pointOfContact = difference.normalized * radius + thisCenter;
            //2. Find normal
            Vector3 normalContact = difference * ((1.0f / difference.magnitude));


            //3. Set restitution
            float restitution = 0.15f;
            c.contacts[0] = new Collision3D.Contact(pointOfContact, normalContact, restitution);
        }

        return pass;
    }

    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull3D other, ref Collision3D c)
    {
        //Calculate closest point by clamping circle center on each dimension
        //passes if closest point vs circle passes

        //1. Get Sphere Center
        Vector3 sphereCenter = particle.position;
        //2. Get center of AABB
        Vector3 aabbCenter = other.Particle.position;
        //3. Get the bounds of AABB
        Vector2 xBounds = new Vector2(aabbCenter.x - (other.xLength * 0.5f), aabbCenter.x + (other.xLength * 0.5f));
        Vector2 yBounds = new Vector2(aabbCenter.y - (other.yLength * 0.5f), aabbCenter.y + (other.yLength * 0.5f));
        Vector2 zBounds = new Vector2(aabbCenter.z - (other.zLength * 0.5f), aabbCenter.z + (other.zLength * 0.5f));
        //4. Closest point is clamped sphere center vs AABB
        float pointX = Mathf.Clamp(sphereCenter.x, xBounds.x, xBounds.y);
        float pointY = Mathf.Clamp(sphereCenter.y, yBounds.x, yBounds.y);
        float pointZ = Mathf.Clamp(sphereCenter.z, zBounds.x, zBounds.y);

        // Setting up collision reference
        c.a = this;
        c.b = other;

        //5. Do test (Sphere vs Point)
        return TestCollisionVsPoint(sphereCenter, new Vector3(pointX, pointY, pointZ), ref c);
    }

    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull3D other, ref Collision3D c)
    {
        //Get other position relative inv world matrix
        Vector3 otherPos = other.Particle.GetInvWorldMatrix().MultiplyPoint(other.Particle.position);

        //Transform Sphere in OBB Space
        Vector3 pos = other.Particle.GetInvWorldMatrix().MultiplyPoint(particle.position);

        // for visuals
        if (sphereRef != null)
        {
            sphereRef.position = pos;
        }

        if (obbRef != null)
        {
            obbRef.position = otherPos;
        }

        //1. Get Sphere Center
        Vector3 sphereCenter = pos;
        //2. Get center of AABB
        Vector3 aabbCenter = otherPos;
        //3. Get the bounds of AABB
        Vector2 xBounds = new Vector2(aabbCenter.x - (other.xLength * 0.5f), aabbCenter.x + (other.xLength * 0.5f));
        Vector2 yBounds = new Vector2(aabbCenter.y - (other.yLength * 0.5f), aabbCenter.y + (other.yLength * 0.5f));
        Vector2 zBounds = new Vector2(aabbCenter.z - (other.zLength * 0.5f), aabbCenter.z + (other.zLength * 0.5f));
        //4. Closest point is clamped sphere center vs AABB
        float pointX = Mathf.Clamp(sphereCenter.x, xBounds.x, xBounds.y);
        float pointY = Mathf.Clamp(sphereCenter.y, yBounds.x, yBounds.y);
        float pointZ = Mathf.Clamp(sphereCenter.z, zBounds.x, zBounds.y);

        // Setting up collision reference
        c.a = this;
        c.b = other;

        //5. Do test (Sphere vs Point)
        return TestCollisionVsPoint(sphereCenter, new Vector3(pointX, pointY, pointZ), ref c);
    }

    public override bool TestCollisionVSShip(ShipCollisionHull3D other, ref Collision3D c)
    {
        return other.TestCollisionVSSphere(this, ref c);
    }
}
