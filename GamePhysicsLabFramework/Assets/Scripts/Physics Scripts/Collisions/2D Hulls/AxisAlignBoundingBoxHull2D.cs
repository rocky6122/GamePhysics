using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignBoundingBoxHull2D : CollisionHull2D
{
    public AxisAlignBoundingBoxHull2D() : base(CollisionHullType2D.hull_aabb) {}

    public float xLength, yLength;

    public Transform cornerRef;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    public override bool TestCollisionVSCircle(CircleCollisionHull2D other, ref Collision c)
    {
        //See circle
        return other.TestCollisionVSAABB(this, ref c);
    }

    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other, ref Collision c)
    {
        //For each dimension, max extent of A is greater than min extent of B

        //1. Get centers of each AABB
        Vector2 thisCenter = particle.position;
        Vector2 otherCenter = other.particle.position;
        //1. Check distance in the x axis based on AABB bounds
        float deltaX = (thisCenter - otherCenter).magnitude - (xLength * 0.5f + other.xLength * 0.5f);
        //2. check distance in the y axis based on AABB bounds
        float deltaY = (thisCenter - otherCenter).magnitude - (yLength * 0.5f + other.yLength * 0.5f);


        //3. If both are less than or equal to 0, there is a collision
        bool pass = deltaX <= 0 && deltaY <= 0;

        //2.5 Fill the contact information
        

        if (pass)
        {
            c.status = pass;
            c.contactCount = 1;
            c.a = this;
            c.b = other;

            Vector2 contactPoint = other.particle.position + deltaX * new Vector2(other.xLength, 0) + deltaY * new Vector2(0, other.yLength);

            Vector2 difference = thisCenter - otherCenter;

            Vector2 normal = (particle.position - contactPoint).normalized;
            normal = new Vector2(Mathf.Round(normal.x), Mathf.Round(normal.y));

            float restitution = 0.15f;

            c.contacts[0] = new Collision.Contact(contactPoint, normal, restitution);

            c.closingVelocity = particle.velocity - other.particle.velocity;
        }


        //3. If both are less than or equal to 0, there is a collision
        return pass;
    }

    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other, ref Collision c)
    {
        bool firstTest, secondTest;

        //First, test AABB vs max extents of OBB
        //1. Get extents assuming OBB is not rotated
        Vector2 topRight = other.Particle.position + new Vector2(other.xLength * 0.5f, other.yLength * 0.5f);
        Vector2 bottomRight = other.Particle.position + new Vector2(other.xLength * 0.5f, -other.yLength * 0.5f);
        Vector2 topLeft = other.Particle.position + new Vector2(-other.xLength * 0.5f, other.yLength * 0.5f);
        Vector2 bottomLeft = other.Particle.position + new Vector2(-other.xLength * 0.5f, -other.yLength * 0.5f);

        //2. Rotate those extents with the rotation of OBB
        Matrix4x4 invMat = other.transform.worldToLocalMatrix;

        topRight = invMat.MultiplyPoint(topRight) + other.transform.position;
        bottomRight = invMat.MultiplyPoint(bottomRight) + other.transform.position;
        topLeft = invMat.MultiplyPoint(topLeft) + other.transform.position;
        bottomLeft = invMat.MultiplyPoint(bottomLeft) + other.transform.position;

        //3. Solve for new min and max points
        float otherMaxX = Mathf.Max(topRight.x, bottomRight.x, topLeft.x, bottomLeft.x);
        float otherMinX = Mathf.Min(topRight.x, bottomRight.x, topLeft.x, bottomLeft.x);
        float otherMaxY = Mathf.Max(topRight.y, bottomRight.y, topLeft.y, bottomLeft.y);
        float otherMinY = Mathf.Min(topRight.y, bottomRight.y, topLeft.y, bottomLeft.y);

        // -- The AABB's bounds
        float maxX = particle.position.x + xLength * 0.5f;
        float minX = particle.position.x - xLength * 0.5f;
        float maxY = particle.position.y + yLength * 0.5f;
        float minY = particle.position.y - yLength * 0.5f;

        Vector2 maxPoint = new Vector2(maxX, maxY);
        Vector2 minPoint = new Vector2(minX, minY);
        Vector2 otherMaxPoint = new Vector2(otherMaxX, otherMaxY);
        Vector2 otherMinPoint = new Vector2(otherMinX, otherMinY);

        //4. Use these for AABB test
        firstTest = (maxPoint.x > otherMinPoint.x && maxPoint.y > otherMinPoint.y && otherMaxPoint.x > minPoint.x && otherMaxPoint.y > minPoint.y);

        if (!firstTest) return false;

        //then, multiply by OBB inverse matrix, do test again
        //1. Get world matrix of OBB (solved above)

        //2. transform AABB into space of OBB
        // --- Getting base corners of AABB
        topRight = particle.position + new Vector2(xLength * 0.5f, yLength * 0.5f);
        bottomRight = particle.position + new Vector2(xLength * 0.5f, -yLength * 0.5f);
        topLeft = particle.position + new Vector2(-xLength * 0.5f, yLength * 0.5f);
        bottomLeft = particle.position + new Vector2(-xLength * 0.5f, -yLength * 0.5f);

        // --- get each corner in the space of the OBB object
        topRight = (Vector2)invMat.MultiplyPoint(topRight) + other.Particle.position;
        bottomRight = (Vector2)invMat.MultiplyPoint(bottomRight) + other.Particle.position;
        topLeft = (Vector2)invMat.MultiplyPoint(topLeft) + other.Particle.position;
        bottomLeft = (Vector2)invMat.MultiplyPoint(bottomLeft) + other.Particle.position;

        // --- find the max and min bounds
        maxX = Mathf.Max(topRight.x, bottomRight.x, topLeft.x, bottomLeft.x);
        minX = Mathf.Min(topRight.x, bottomRight.x, topLeft.x, bottomLeft.x);
        maxY = Mathf.Max(topRight.y, bottomRight.y, topLeft.y, bottomLeft.y);
        minY = Mathf.Min(topRight.y, bottomRight.y, topLeft.y, bottomLeft.y);

        otherMaxX = other.Particle.position.x + other.xLength * 0.5f;
        otherMinX = other.Particle.position.x - other.xLength * 0.5f;
        otherMaxY = other.Particle.position.y + other.yLength * 0.5f;
        otherMinY = other.Particle.position.y - other.yLength * 0.5f;

        // --- Create vectors of the max and mins
        maxPoint = new Vector2(maxX, maxY);
        minPoint = new Vector2(minX, minY);
        otherMaxPoint = new Vector2(otherMaxX, otherMaxY);
        otherMinPoint = new Vector2(otherMinX, otherMinY);

        if (cornerRef != null)
        {
            cornerRef.position = minPoint;
        }

        //3. Do the test above again
        secondTest = (maxPoint.x > otherMinPoint.x && maxPoint.y > otherMinPoint.y && otherMaxPoint.x > minPoint.x && otherMaxPoint.y > minPoint.y);


        bool pass = firstTest && secondTest;

        if (pass)
        {
            c.a = this;
            c.b = other;
            c.status = pass;
            c.contactCount = 1;

            //Not currently in use
            Vector2 contactPoint = Vector2.zero;

            Vector2 difference = other.Particle.position - this.particle.position;

            Vector2 contactNormal = -(particle.velocity.normalized + difference.normalized).normalized;

            float restitution = 0.15f;

            c.contacts[0] = new Collision.Contact(contactPoint, contactNormal, restitution);
        }

        return pass;
    }
}
