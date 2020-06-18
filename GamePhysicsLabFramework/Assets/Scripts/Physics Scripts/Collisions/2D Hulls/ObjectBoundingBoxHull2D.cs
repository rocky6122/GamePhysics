using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxHull2D : CollisionHull2D
{
    public ObjectBoundingBoxHull2D() : base(CollisionHullType2D.hull_obb) {}

    public float xLength, yLength;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    public override bool TestCollisionVSCircle(CircleCollisionHull2D other, ref Collision c)
    {
        //see circle
        return other.TestCollisionVSOBB(this, ref c);
    }

    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other, ref Collision c)
    {
        //see AABB
        return other.TestCollisionVSOBB(this, ref c);
    }

    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other, ref Collision c)
    {
        bool pass =  (OBBvsOBB(this, other) && OBBvsOBB(other, this));

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

            c.closingVelocity = particle.velocity - other.particle.velocity;
        }

        return pass;
    }

    private bool OBBvsOBB(ObjectBoundingBoxHull2D lhs, ObjectBoundingBoxHull2D rhs)
    {
        //1. Get world matrix of lhs OBB
        Matrix4x4 lhsOBBWorldInvMat = lhs.transform.worldToLocalMatrix;

        //1.5 Get world matrix of rhs OBB
        Matrix4x4 rhsOBBWorldInvMat = rhs.transform.worldToLocalMatrix;

        //2. Get corners of rhs OBB, assuming AABB
        Vector2 topRight = rhs.Particle.position + new Vector2(rhs.xLength * 0.5f, rhs.yLength * 0.5f);
        Vector2 bottomRight = rhs.Particle.position + new Vector2(rhs.xLength * 0.5f, -rhs.yLength * 0.5f);
        Vector2 topLeft = rhs.Particle.position + new Vector2(-rhs.xLength * 0.5f, rhs.yLength * 0.5f);
        Vector2 bottomLeft = rhs.Particle.position + new Vector2(-rhs.xLength * 0.5f, -rhs.yLength * 0.5f);

        //2.5 Get corners of rhs in correct OBB location
        topRight = rhsOBBWorldInvMat.MultiplyPoint(topRight) + rhs.transform.position;
        bottomRight = rhsOBBWorldInvMat.MultiplyPoint(bottomRight) + rhs.transform.position;
        topLeft = rhsOBBWorldInvMat.MultiplyPoint(topLeft) + rhs.transform.position;
        bottomLeft = rhsOBBWorldInvMat.MultiplyPoint(bottomLeft) + rhs.transform.position;

        //3. Convert corners of rhs OBB to lhs world space
        topRight = lhsOBBWorldInvMat.MultiplyPoint(topRight) + lhs.transform.position;
        bottomRight = lhsOBBWorldInvMat.MultiplyPoint(bottomRight) + lhs.transform.position;
        topLeft = lhsOBBWorldInvMat.MultiplyPoint(topLeft) + lhs.transform.position;
        bottomLeft = lhsOBBWorldInvMat.MultiplyPoint(bottomLeft) + lhs.transform.position;

        //4. Find min and max bounds of rhs OBB 
        float rhsMaxX = Mathf.Max(topRight.x, bottomRight.x, topLeft.x, bottomLeft.x);
        float rhsMinX = Mathf.Min(topRight.x, bottomRight.x, topLeft.x, bottomLeft.x);
        float rhsMaxY = Mathf.Max(topRight.y, bottomRight.y, topLeft.y, bottomLeft.y);
        float rhsMinY = Mathf.Min(topRight.y, bottomRight.y, topLeft.y, bottomLeft.y);

        //5. Get bounds of lhs OBB
        float lhsMaxX = lhs.particle.position.x + lhs.xLength * 0.5f;
        float lhsMinX = lhs.particle.position.x - lhs.xLength * 0.5f;
        float lhsMaxY = lhs.particle.position.y + lhs.yLength * 0.5f;
        float lhsMinY = lhs.particle.position.y - lhs.yLength * 0.5f;

        //6. Create vectors
        Vector2 thisMaxPoint = new Vector2(lhsMaxX, lhsMaxY);
        Vector2 thisMinPoint = new Vector2(lhsMinX, lhsMinY);
        Vector2 otherMaxPoint = new Vector2(rhsMaxX, rhsMaxY);
        Vector2 otherMinPoint = new Vector2(rhsMinX, rhsMinY);

        //7. Do Test
        return (thisMaxPoint.x > otherMinPoint.x && thisMaxPoint.y > otherMinPoint.y && otherMaxPoint.x > thisMinPoint.x && otherMaxPoint.y > thisMinPoint.y);
    }
}
