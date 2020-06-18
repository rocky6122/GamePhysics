using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{
    public List<CollisionHull2D> collisionsThisUpdate = new List<CollisionHull2D>();

    public class Collision
    {
        public struct Contact
        {
            public Vector2 point;
            public Vector2 normal;
            public float restitution;

            public Contact(Vector2 p, Vector2 n, float rest)
            {
                point = p;
                normal = n;
                restitution = rest;
            }
        }

        public CollisionHull2D a = null, b = null;
        public Contact[] contacts = new Contact[4];
        public int contactCount = 0;
        public bool status = false;

        public Vector2 closingVelocity;

        public void Clean()
        {
            contacts = new Contact[4];
            contactCount = 0;
            status = false;
            a = b = null;
            closingVelocity = Vector2.zero;
        }

        float CalculateSeparatingSpeed(Vector2 contactNormal)
        {
            //Return (velA - velB) * contactNormal
            Vector2 relSpeed = a.particle.velocity;

            if (b.particle)
            {
                relSpeed -= b.particle.velocity;
            }

            return Vector2.Dot(relSpeed, contactNormal);
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
            if(totalInvMass <= 0)
            {
                //Infinite Mass Detected
                //Debug.LogWarning("INFINITE MASS (ON INFINITE EARTHS)");
                return;
            }

            //Calculate the impulse to apply
            float impulse = deltaSpeed / totalInvMass;

            //Find the amount of impulse per unit of inverse Mass
            Vector2 impulsePerInvMass = contact.normal * impulse;

            //Apply change in speed (impulses) to each object in proportion to its inverse mass
            a.particle.velocity += impulsePerInvMass * a.particle.GetInvMass();

            if (b.particle)
            {
                b.particle.velocity += impulsePerInvMass * -b.particle.GetInvMass();
            }
        }
    }


    public enum CollisionHullType2D
    {
        hull_circle,
        hull_aabb,
        hull_obb,
    }
    private CollisionHullType2D Type { get; }

    public int objValue { get; private set; }

    public void SetObjValue(int val)
    {
        objValue = val;
    }

    protected CollisionHull2D(CollisionHullType2D type_set)
    {
        Type = type_set;
    }

    protected Particle2D particle;
    public Particle2D Particle { get { return particle; } }

    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b, ref Collision c)
    {
        CollisionHullType2D otherType = b.Type;

        switch (otherType)
        {
            case CollisionHullType2D.hull_circle:
                CircleCollisionHull2D circle = b as CircleCollisionHull2D;
                return a.TestCollisionVSCircle(circle, ref c);
            case CollisionHullType2D.hull_aabb:
                AxisAlignBoundingBoxHull2D aabb = b as AxisAlignBoundingBoxHull2D;
                return a.TestCollisionVSAABB(aabb, ref c);
            case CollisionHullType2D.hull_obb:
                ObjectBoundingBoxHull2D obb = b as ObjectBoundingBoxHull2D;
                 return a.TestCollisionVSOBB(obb, ref c);
            default:
                return false;
        }
    }

    public abstract bool TestCollisionVSCircle(CircleCollisionHull2D other, ref Collision c);

    public abstract bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other, ref Collision c);

    public abstract bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other, ref Collision c);
}
