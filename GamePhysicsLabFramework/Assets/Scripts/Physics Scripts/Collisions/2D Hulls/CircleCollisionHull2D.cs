using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollisionHull2D : CollisionHull2D
{
    public CircleCollisionHull2D() : base(CollisionHullType2D.hull_circle)
    {
        
    }

    [Range( 0.0f, 100.0f)]
    public float radius;
    public Transform circleRef;
    public Transform obbRef;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    public override bool TestCollisionVSCircle(CircleCollisionHull2D other, ref Collision c)
    {
        bool collisionPass = false;

        //pass if distance between centers <= sum of radii
        //optimized collision passes if (distance between centers) squared <= (sum of radii) squared

        //1. Get center of each particle
        Vector2 thisCenter = Particle.position;
        Vector2 otherCenter = other.particle.position;

        //2. Difference between centers
        Vector2 difference = thisCenter - otherCenter;

        //3. Distance squared = dot(difference, difference)
        float distSquared = Vector2.Dot(difference, difference);

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
            Vector2 pointOfContact = difference.normalized * thisRadius + thisCenter;
            //2. Find normal
            Vector2 normalContact = difference * ((1.0f / difference.magnitude));

            //3. Set restitution
            float restitution = 0.15f;
            c.contacts[0] = new Collision.Contact(pointOfContact, normalContact, restitution);

            //4. Get Closing Velocity (Function for closing velocity found at https://mathhelpboards.com/calculus-10/calculate-closing-velocities-19402.html

            c.closingVelocity = particle.velocity - other.particle.velocity;
        }

        return collisionPass;
    }

    private bool TestCollisionVsPoint(Vector3 center, Vector2 point, ref Collision c)
    {
        bool pass = false;

        //1. Get center of this particle
        Vector2 thisCenter = center;

        //2. Difference between center an point
        Vector2 difference = thisCenter - point;

        //3. Distance squared = dot(difference, difference)
        float distSquared = Vector2.Dot(difference, difference);

        float radiusSquared = radius * radius;

        //6. DO THE TEST: distanceSquared <= radiusSquared
        if (distSquared <= radiusSquared)
        {
            pass = true;
        }

        if (pass)
        {
            c.status = pass;
            c.contactCount = 1;

            //1. Find point
            Vector2 pointOfContact = difference.normalized * radius + thisCenter;
            //2. Find normal
            Vector2 normalContact = difference * ((1.0f / difference.magnitude));


            //3. Set restitution
            float restitution = 0.15f;
            c.contacts[0] = new Collision.Contact(pointOfContact, normalContact, restitution);
        }

        return pass;
    }
           
    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other, ref Collision c)
    {
        //Calculate closest point by clamping circle center on each dimension
        //passes if closest point vs circle passes

        //1. Get Circle Center
        Vector2 circleCenter = particle.position;
        //2. Get center of AABB
        Vector2 aabbCenter = other.Particle.position;
        //3. Get Both bounds of AABB
        Vector2 xBounds = new Vector2(aabbCenter.x - (other.xLength * 0.5f), aabbCenter.x + (other.xLength * 0.5f));
        Vector2 yBounds = new Vector2(aabbCenter.y - (other.yLength * 0.5f), aabbCenter.y + (other.yLength * 0.5f));
        //4. Closest point is clamped circle center vs AABB
        float pointX = Mathf.Clamp(circleCenter.x, xBounds.x, xBounds.y);
        float pointY = Mathf.Clamp(circleCenter.y, yBounds.x, yBounds.y);

        // Setting up collision reference
        c.a = this;
        c.b = other;

        //5. Do test (Circle vs Point)
        return TestCollisionVsPoint(circleCenter, new Vector2(pointX, pointY), ref c);
    }

    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other, ref Collision c)
    {
        //Same as above, but first....
        //transform circle position by multiplying by box world matrix inverse

        //1. Get world matrix of OBB
        Matrix4x4 invMat = other.transform.worldToLocalMatrix;

        Vector2 otherPos = invMat.MultiplyPoint(other.Particle.position);

        //2. transform circle into OBB space, getting new position
        //  -- invMat is in world space, so we add the OBB's position
        Vector2 pos = invMat.MultiplyPoint(particle.position);

        // for visuals
        if (circleRef != null)
        {
            circleRef.position = pos;
        }

        if (obbRef != null)
        {
            obbRef.position = otherPos;
        }

        // Now do Test Vs AABB

        //1. Get Circle Center
        Vector2 circleCenter = pos;
        //2. Get center of AABB
        Vector2 aabbCenter = Vector2.zero;
        //3. Get Both bounds of AABB
        Vector2 xBounds = new Vector2(aabbCenter.x - (other.xLength * 0.5f), aabbCenter.x + (other.xLength * 0.5f));
        Vector2 yBounds = new Vector2(aabbCenter.y - (other.yLength * 0.5f), aabbCenter.y + (other.yLength * 0.5f));
        //4. Closest point is clamped circle center vs AABB
        float pointX = Mathf.Clamp(circleCenter.x, xBounds.x, xBounds.y);
        float pointY = Mathf.Clamp(circleCenter.y, yBounds.x, yBounds.y);

        // Setting up collision reference
        c.a = this;
        c.b = other;

        //5. Do test (Circle vs Point)
        return TestCollisionVsPoint(circleCenter, new Vector2(pointX, pointY), ref c);
    }
}
