using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull3D : MonoBehaviour
{
    public List<CollisionHull3D> collisionsThisUpdate = new List<CollisionHull3D>();

    public class Collision3D
    {
        public struct Contact
        {
            public Vector3 point;
            public Vector3 normal;
            public float restitution;

            public Contact(Vector3 p, Vector3 n, float rest)
            {
                point = p;
                normal = n;
                restitution = rest;
            }
        }

        public CollisionHull3D a = null, b = null;
        public Contact[] contacts = new Contact[4];
        public int contactCount = 0;
        public bool status = false;

        public Vector3 closingVelocity;

        public void Clean()
        {
            contacts = new Contact[4];
            contactCount = 0;
            status = false;
            a = b = null;
            closingVelocity = Vector3.zero;
        }

        float CalculateSeparatingSpeed(Vector3 contactNormal)
        {
            //Return (velA - velB) * contactNormal
            Vector3 relSpeed = a.particle.velocity;

            if (b.particle)
            {
                relSpeed -= b.particle.velocity;
            }

            return Vector3.Dot(relSpeed, contactNormal);
        }

        public void ResolveSpeed(Contact contact)
        {
            //Find speed in direction of contact
            float sepSpeed = CalculateSeparatingSpeed(contact.normal);

            //Check if the collision actually needs to be resolved
            if (sepSpeed > 0)
            {
                //Objects are moving apart or stationary

                return;
            }

            //Calculate the new Separating Speed
            float newSepSpeed = -sepSpeed * contact.restitution;

            //Find difference in speeds
            float deltaSpeed = newSepSpeed - sepSpeed;

            //Get total inverse mass
            float totalInvMass = a.particle.GetInvMass();

            if (b.particle)
            {
                totalInvMass += b.particle.GetInvMass();
            }

            //if all particles have infinite mass, no effect
            if (totalInvMass <= 0)
            {
                //Infinite Mass Detected
                //Debug.LogWarning("INFINITE MASS (ON INFINITE EARTHS)");
                return;
            }

            //Calculate the impulse to apply
            float impulse = deltaSpeed / totalInvMass;

            //Find the amount of impulse per unit of inverse Mass
            Vector3 impulsePerInvMass = contact.normal * impulse;

            //Apply change in speed (impulses) to each object in proportion to its inverse mass
            a.particle.velocity += impulsePerInvMass * a.particle.GetInvMass();

            if (b.particle)
            {
                b.particle.velocity += impulsePerInvMass * -b.particle.GetInvMass();
            }
        }
    }

    public enum CollisionHullType3D
    {
        hull_sphere,
        hull_aabb,
        hull_obb,
        hull_ship
    }
    private CollisionHullType3D Type { get; }

    public int objValue { get; private set; }

    public void SetObjValue(int val)
    {
        objValue = val;
    }

    protected CollisionHull3D(CollisionHullType3D type_set)
    {
        Type = type_set;
    }

    protected Particle3D particle;
    public Particle3D Particle { get { return particle; } }

    public static bool TestCollision(CollisionHull3D a, CollisionHull3D b, ref Collision3D c)
    {
        CollisionHullType3D otherType = b.Type;

        switch (otherType)
        {
            case CollisionHullType3D.hull_sphere:
                SphereCollisionHull3D sphere = b as SphereCollisionHull3D;
                return a.TestCollisionVSSphere(sphere, ref c);
            case CollisionHullType3D.hull_aabb:
                AxisAlignBoundingBoxHull3D aabb = b as AxisAlignBoundingBoxHull3D;
                return a.TestCollisionVSAABB(aabb, ref c);
            case CollisionHullType3D.hull_obb:
                ObjectBoundingBoxHull3D obb = b as ObjectBoundingBoxHull3D;
                return a.TestCollisionVSOBB(obb, ref c);
            default:
                return false;
        }
    }

    public abstract bool TestCollisionVSSphere(SphereCollisionHull3D other, ref Collision3D c);

    public abstract bool TestCollisionVSAABB(AxisAlignBoundingBoxHull3D other, ref Collision3D c);

    public abstract bool TestCollisionVSOBB(ObjectBoundingBoxHull3D other, ref Collision3D c);

    public abstract bool TestCollisionVSShip(ShipCollisionHull3D other, ref Collision3D c);
}