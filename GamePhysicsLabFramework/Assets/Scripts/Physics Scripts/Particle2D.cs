using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Written by John Imgrund and Parker Staszkiewicz

public enum Test { none, gravity, sliding, friction, drag, spring };
public enum Shape { square, circle, ring, rod };

public class Particle2D : MonoBehaviour
{
    //Step 1 (Define Particles)
    public Vector2 position, velocity, acceleration;

    public float rotation, angularVelocity, angularAcceleration;
    float inertia = 0;
    float invInertia;
    public float torque;

    public Shape shape;
    private Vector2 forcePosVec = Vector2.zero, forceVec = Vector2.zero;
    public TMP_InputField forcePosX, forcePosY, forceVecX, forceVecY;

    //Lab 2 - Step 1
    public float startingMass = 1.0f;
    float mass, massInv;

    public void SetMass(float newMass)
    {
        mass = newMass > 0.0f ? newMass : 0.0f;

        //Similar to above
        //mass = Mathf.Max(0.0f, newMass);

        massInv = newMass > 0.0f ? 1.0f / newMass : 0.0f;
    }

    public float GetMass()
    {
        return mass;
    }

    public float GetInvMass()
    {
        return massInv;
    }

    //Lab 2 - Step 2
    public Vector2 force;

    public void AddForce(Vector2 newForce)
    {
        //D'Alembert
        force += newForce;
    }

    public void AddGravity()
    {
        //force += ForceGenerator.GenerateForce_Gravity(mass, -1.62f, Vector2.up);
    }

    void UpdateAcceleration()
    {
        //Newton 2
        acceleration = massInv * force;

        force.Set(0.0f, 0.0f);
    }

    //step 2 (Integration Algorithms)
    void UpdatePositionExplicitEuler(float dt)
    {
        // x(t + dt) = x(t) + v(t) * dt
        //Euler's Method:
        //F(t + dt) = F(t) + f(t) * dt
        //                 + (dF / dt) * dt
        position += velocity * dt;

        //v(t + dt) = v + a(d) * dt
        velocity += acceleration * dt;
    }

    void UpdatePositionKinematic(float dt)
    {
        //x(t + dt) = x(t) + v(t) * dt + .5 * a(t) * (dt * dt)
        position += velocity * dt + .5f * acceleration * (dt * dt);

        //v(t + dt) = v + a(d) * dt
        velocity += acceleration * dt;
    }

    void UpdateRotationEuler(float dt)
    {
        rotation += angularVelocity * dt;
        angularVelocity += angularAcceleration * dt;
    }

    void UpdateRotationKinematic(float dt)
    {
        rotation += angularVelocity * dt + .5f * angularAcceleration * (dt * dt);

        angularVelocity += angularAcceleration * dt;
    }

    public void SetRotation(float r)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, r));
    }

    //Lab 3 Functions
    public void UpdateAngularAcceleration()
    {
        angularAcceleration = invInertia * torque;

        //reset torque
        torque = 0;
    }

    private void SetForcePositonVector(Vector2 pos)
    {
        forcePosVec = pos;
    }

    private void SetForceVector(Vector2 force)
    {
        forceVec = force;
    }

    public void ApplyTorque(Vector2 applicationPoint, Vector2 appliedForce)
    {
        Vector2 forceDistance = new Vector2(applicationPoint.x - position.x, applicationPoint.y - position.y);

        //Using Dans 2D Torque formula
        float newTorque = forceDistance.x * appliedForce.y - forceDistance.y - appliedForce.x;

        torque += newTorque;
    }

    public void UpdateTorqueForces()
    {
        SetForcePositonVector(new Vector2(float.Parse(forcePosX.text), float.Parse(forcePosY.text)));
        SetForceVector(new Vector2(float.Parse(forceVecX.text), float.Parse(forceVecY.text)));
    }

    public void SetInertia()
    {
        switch (shape)
        {
            case Shape.square:
                inertia = (1.0f / 12.0f) * mass * ((transform.localScale.x * transform.localScale.x) + (transform.localScale.y * transform.localScale.y));
                break;
            case Shape.circle:
                inertia = 0.5f * mass * transform.localScale.x * transform.localScale.x;
                break;
            case Shape.ring:
                float outer = transform.GetChild(0).transform.localScale.x * transform.GetChild(0).transform.localScale.x;
                float inner = transform.GetChild(1).transform.localScale.x * transform.GetChild(1).transform.localScale.x;
                inertia = 0.5f * mass * (outer + inner);
                break;
            case Shape.rod:
                inertia = (1.0f / 12.0f) * mass * transform.localScale.x * transform.localScale.x;
                break;
            default:
                break;
        }


        invInertia = 1.0f / inertia;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Lab 2
        SetMass(startingMass);

        //Lab 3
        //Set Starting Inertia assuming starting object is rectangle
        SetInertia();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //integrate
        //CheckInput();

        //ApplyTorque(forcePosVec, forceVec);

        UpdatePositionKinematic(Time.fixedDeltaTime);
        UpdateRotationKinematic(Time.fixedDeltaTime);
        UpdateAngularAcceleration();
        UpdateAcceleration();

        //apply to transform
        transform.position = position;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
    }
}