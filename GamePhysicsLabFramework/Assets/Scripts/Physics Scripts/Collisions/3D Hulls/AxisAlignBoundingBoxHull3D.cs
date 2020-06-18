using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignBoundingBoxHull3D : CollisionHull3D
{
    public AxisAlignBoundingBoxHull3D() : base(CollisionHullType3D.hull_aabb)
    { }

    public float xLength, yLength, zLength;
    private Vector3 max;
    private Vector3 min;

    public Transform cornerRef;

    public Vector3[] vertices;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle3D>();

        max = new Vector3(xLength * 0.5f, yLength * 0.5f, zLength * 0.5f);
        min = -max;

        //Set all the vertices in their starting positions
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
        //See Sphere
        return other.TestCollisionVSAABB(this, ref c);
    }

    public override bool TestCollisionVSAABB(AxisAlignBoundingBoxHull3D other, ref Collision3D c)
    {
        //For each dimension, max extent of A is greater than min extent of B

        //1. Get centers of each AABB
        Vector3 thisCenter = particle.position;
        Vector3 otherCenter = other.particle.position;

        //1. Check distance in the x axis based on AABB bounds
        float deltaX = (thisCenter - otherCenter).magnitude - (xLength * 0.5f + other.xLength * 0.5f);
        //2. check distance in the y axis based on AABB bounds
        float deltaY = (thisCenter - otherCenter).magnitude - (yLength * 0.5f + other.yLength * 0.5f);
        //3. Check distance in the z axis based on AABB bounds
        float deltaZ = (thisCenter - otherCenter).magnitude - (zLength * 0.5f + other.zLength * 0.5f);

        //4. If all are less than or equal to 0, there is a collision
        bool pass = deltaX <= 0 && deltaY <= 0 && deltaZ <= 0;

        //If Collision has occured
        if (pass)
        {
            //set collision items
            c.status = pass;
            c.contactCount = 1;
            c.a = this;
            c.b = other;

            Vector3 contactPoint = other.particle.position + deltaX * new Vector3(other.xLength, 0, 0) + deltaY * new Vector3(0, other.yLength, 0) + deltaZ * new Vector3(0, 0, other.zLength);

            Vector3 difference = thisCenter - otherCenter;

            Vector3 normal = (particle.position - contactPoint).normalized;
            normal = new Vector3(Mathf.Round(normal.x), Mathf.Round(normal.y), Mathf.Round(normal.z));

            float restitution = 0.15f;

            c.contacts[0] = new Collision3D.Contact(contactPoint, normal, restitution);

            c.closingVelocity = particle.velocity - other.particle.velocity;
        }


        //If both are less than or equal to 0, there is a collision
        return pass;
    }

    public override bool TestCollisionVSOBB(ObjectBoundingBoxHull3D other, ref Collision3D c)
    {
        //Grab the faces of each object
        Vector3[] thisFaces = { particle.rotation * Vector3.right, particle.rotation * Vector3.up, particle.rotation * Vector3.forward };
        Vector3[] otherFaces = { other.Particle.rotation * Vector3.right, other.Particle.rotation * Vector3.up, other.Particle.rotation * Vector3.forward };

        //Check collision on the faces of each object (6)
        for (int i = 0; i < thisFaces.Length; ++i)
        {
            if (ObjectBoundingBoxHull3D.Separated(vertices, other.vertices, thisFaces[i]))
                return false;

            if (ObjectBoundingBoxHull3D.Separated(vertices, other.vertices, otherFaces[i]))
                return false;
        }

        //Check the edges (9)
        for (int i = 0; i < thisFaces.Length; ++i)
        {
            for (int j = 0; j < otherFaces.Length; ++j)
            {
                if (ObjectBoundingBoxHull3D.Separated(vertices, other.vertices, Vector3.Cross(thisFaces[i], otherFaces[j])))
                        return false;
            }
        }

        //Collision must be true so get contact for resolving later
        c.a = this;
        c.b = other;
        c.status = true;
        c.contactCount = 1;

        Vector3 contactPoint = Vector3.zero;

        Vector3 difference = other.Particle.position - this.particle.position;

        Vector3 contactNormal = -(particle.velocity.normalized + difference.normalized).normalized;

        float restitution = 0.15f;

        c.contacts[0] = new Collision3D.Contact(contactPoint, contactNormal, restitution);

        c.closingVelocity = particle.velocity - other.Particle.velocity;

        return true;
    }

    public override bool TestCollisionVSShip(ShipCollisionHull3D other, ref Collision3D c)
    {
        return other.TestCollisionVSAABB(this, ref c);
    }

    public void UpdateVertices()
    {
        //Make sure verticies are up to date
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
