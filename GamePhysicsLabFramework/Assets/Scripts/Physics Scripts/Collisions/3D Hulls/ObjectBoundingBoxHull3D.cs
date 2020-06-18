using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxHull3D : CollisionHull3D
{
    public ObjectBoundingBoxHull3D() : base(CollisionHullType3D.hull_obb) { }

    public float xLength, yLength, zLength;
    private Vector3 max;
    private Vector3 min;

    public Vector3[] vertices;

    public void InitValues(Vector3 lengths)
    {
        xLength = lengths.x;
        yLength = lengths.y;
        zLength = lengths.z;
    }

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle3D>();

        max = new Vector3(xLength * 0.5f, yLength * 0.5f, zLength *0.5f);
        min = -max;

        //Set starting vertices
        vertices = new Vector3[]
        {
            particle.position + particle.rotation * min,
            particle.position + particle.rotation * new Vector3(max.x, min.y, min.z),
            particle.position + particle.rotation * new Vector3(min.x, max.y, min.z),
            particle.position + particle.rotation * new Vector3(max.x, max.y, min.z),
            particle.position + particle.rotation * new Vector3(min.x, min.y, max.z),
            particle.position + particle.rotation * new Vector3(max.x, min.y, max.z),
            particle.position + particle.rotation * new Vector3(min.x, max.y, max.z),
            particle.position + particle.rotation * max
        };
    }

    private void Update()
    {
        UpdateVertices();
    }

    public override bool TestCollisionVSSphere(SphereCollisionHull3D other, ref Collision3D c)
    {
        return other.TestCollisionVSOBB(this, ref c);
    }

    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull3D other, ref Collision3D c)
    {
        return other.TestCollisionVSOBB(this, ref c);
    }

    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull3D other, ref Collision3D c)
    {
        //Grab the faces of each object
        Vector3[] thisFaces = { particle.rotation * Vector3.right, particle.rotation * Vector3.up , particle.rotation * Vector3.forward };
        Vector3[] otherFaces = { other.Particle.rotation * Vector3.right, other.Particle.rotation * Vector3.up, other.Particle.rotation * Vector3.forward };

        //Check collision on the faces of each object (6)
        for (int i = 0; i < thisFaces.Length; ++i)
        {
            if (Separated(vertices, other.vertices, thisFaces[i]))
                return false;

            if (Separated(vertices, other.vertices, otherFaces[i]))
                return false;
        }

        //Check the edges (9)
        for (int i = 0; i < thisFaces.Length; ++i)
        {
            for (int j = 0; j < otherFaces.Length; ++j)
            {
                if (Separated(vertices, other.vertices, Vector3.Cross(thisFaces[i], otherFaces[j])))
                    return false;
            }
        }

        //Collision must be true so get contact for resolving later
        c.a = this;
        c.b = other;
        c.status = true;
        c.contactCount = 1;

        //Not currently in use
        Vector3 contactPoint = Vector3.zero;

        Vector3 difference = other.Particle.position - this.particle.position;

        Vector3 contactNormal = -(particle.velocity.normalized + difference.normalized).normalized;

        float restitution = 0.15f;

        c.contacts[0] = new Collision3D.Contact(contactPoint, contactNormal, restitution);

        c.closingVelocity = particle.velocity - other.Particle.velocity;

        return true;
    }

    public static bool Separated(Vector3[] vertsA, Vector3[] vertsB, Vector3 axis)
    {
        //collision occured
        if (axis == Vector3.zero)
            return false;

        float aMin = float.MaxValue;
        float aMax = float.MinValue;
        float bMin = float.MaxValue;
        float bMax = float.MinValue;

        //Find the min and max distance from the axis for both objects edges
        for (int i = 0; i < 8; ++i)
        {
            float aDist = Vector3.Dot(vertsA[i], axis);
            aMin = aDist < aMin ? aDist : aMin;
            aMax = aDist > aMax ? aDist : aMax;

            float bDist = Vector3.Dot(vertsB[i], axis);
            bMin = bDist < bMin ? bDist : bMin;
            bMax = bDist > bMax ? bDist : bMax;
        }

        //Test if the closest edges are intersecting
        float longSpan = Mathf.Max(aMax, bMax) - Mathf.Min(aMin, bMin);
        float sumSpan = aMax - aMin + bMax - bMin;
        return longSpan >= sumSpan;
    }

    public override bool TestCollisionVSShip(ShipCollisionHull3D other, ref Collision3D c)
    {
        return other.TestCollisionVSOBB(this, ref c);
    }

    public void UpdateVertices()
    {
        //Update vertices this frame
        vertices = new Vector3[]
        {
            particle.position + particle.rotation * min,
            particle.position + particle.rotation * new Vector3(max.x, min.y, min.z),
            particle.position + particle.rotation * new Vector3(min.x, max.y, min.z),
            particle.position + particle.rotation * new Vector3(max.x, max.y, min.z),
            particle.position + particle.rotation * new Vector3(min.x, min.y, max.z),
            particle.position + particle.rotation * new Vector3(max.x, min.y, max.z),
            particle.position + particle.rotation * new Vector3(min.x, max.y, max.z),
            particle.position + particle.rotation * max
        };
    }
}
