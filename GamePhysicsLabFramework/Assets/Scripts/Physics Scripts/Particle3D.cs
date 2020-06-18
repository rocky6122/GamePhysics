using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Written by John Imgrund and Parker Staszkiewicz

public enum Shape3D {  solidSphere, hollowSphere, solidCube, hollowCube, solidCylinder, solidCone };

public class Particle3D : MonoBehaviour
{
    //Step 1 (Define Particles)
    public Vector3 position, velocity, acceleration;

    public Quaternion rotation = Quaternion.identity;

    public Vector3 angularVelocity, angularAcceleration, torque;

    public Shape3D shape;

    //Shape variables
    public float radius;
    public Vector3 cuboidDimensions;
    public float shapeHeight;

    private Vector3 forcePosVec = Vector3.zero, forceVec = Vector3.zero;

    //Lab 2 - Step 1
    public float startingMass = 1.0f;
    float mass, massInv;

    //Lab 7
    Matrix4x4 worldTransformMatrix = Matrix4x4.identity;
    Matrix4x4 invWorldTransformMatrix = Matrix4x4.identity;

    Vector3 worldCenterOfMass, localCenterOfMass;

    Matrix4x4 invLocalInertiaTensor = Matrix4x4.identity;

    Matrix4x4 localInertiaTensor = Matrix4x4.identity;

    Matrix4x4 WorldInertiaTensor = Matrix4x4.identity;

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
    public Vector3 force;

    public void AddForce(Vector3 newForce)
    {
        //D'Alembert
        force += newForce;
    }

    public void AddGravity()
    {
        //force += ForceGenerator.GenerateForce_Gravity(mass, -1.62f, Vector3.up);
    }

    void UpdateAcceleration()
    {
        //Newton 2
        acceleration = massInv * force;

        force.Set(0.0f, 0.0f, 0.0f);
    }

    //step 2 (Integration Algorithms)
    void UpdatePositionExplicitEuler(float dt)
    {
        //2D
        // x(t + dt) = x(t) + v(t) * dt
        //Euler's Method:
        //F(t + dt) = F(t) + f(t) * dt
        //                 + (dF / dt) * dt
        //position += velocity * dt;

        ////v(t + dt) = v + a(d) * dt
        //velocity += acceleration * dt;

        Vector3[] parameters = new Vector3[3] { position, velocity, acceleration };
        PhysicsDLL.UpdatePositionExplicitEuler(parameters, dt);
        position = parameters[0];
        velocity = parameters[1];
        acceleration = parameters[2];
    }

    void UpdatePositionKinematic(float dt)
    {
        ////x(t + dt) = x(t) + v(t) * dt + .5 * a(t) * (dt * dt)
        //position += velocity * dt + .5f * acceleration * (dt * dt);

        ////v(t + dt) = v + a(d) * dt
        //velocity += acceleration * dt;

        Vector3[] parameters = new Vector3[3] { position, velocity, acceleration };
        PhysicsDLL.UpdatePositionKinematic(parameters, dt);
        position = parameters[0];
        velocity = parameters[1];
        acceleration = parameters[2];
    }

    void UpdateRotationEuler(float dt)
    {
        //Old 2D
        //rotation += angularVelocity * dt;
        //angularVelocity += angularAcceleration * dt;

        //3D
        //F(qt + qdt) = 1/2 * qt * wt * dt

        //First step of the equation while in vector form
        Vector3 scaledVelocity = angularVelocity * 0.5f * dt; 

        //Turn scaledVelocity into a Quaternion and multiply it by current rotation
        Quaternion quat = multiplyQuaternionByVector(scaledVelocity, rotation);

        //Add the new rotation into the old one
        rotation = AddQuaternions(rotation, quat);

        //Normalize to avoid scaling issues
        rotation.Normalize();

        //Integrate angularVelocity
        //angularVelocity += angularAcceleration * dt;

        Vector3[] parameters = new Vector3[2] { angularVelocity, angularAcceleration };
        PhysicsDLL.IntegrateAngularVelocity(parameters, dt);
        angularVelocity = parameters[0];
        angularAcceleration = parameters[1];
    }

    void UpdateRotationKinematic(float dt)
    {
        //Old 2D
        //rotation += angularVelocity * dt + .5f * angularAcceleration * (dt * dt);
        //angularVelocity += angularAcceleration * dt;
    }

    public void SetRotation(float r)
    {
        //transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, r));
    }

    //Lab 3 Functions
    public void UpdateAngularAcceleration()
    {
        //Old 2D
        //angularAcceleration = invInertia * torque;
        //reset torque
        //torque = 0;

        //Temp 3D Solution
        angularAcceleration = WorldInertiaTensor * torque;
        torque = Vector3.zero;
    }

    public void ApplyTorque(Vector3 applicationPoint, Vector3 appliedForce)
    {
        //Old 2D
        //Vector2 forceDistance = new Vector2(applicationPoint.x - position.x, applicationPoint.y - position.y);
        //Using Dans 2D Torque formula
        //float newTorque = forceDistance.x * appliedForce.y - forceDistance.y - appliedForce.x;
        //torque += newTorque;

        //new 3D
        Vector3 forceDistance = applicationPoint - localCenterOfMass;

        torque += Vector3.Cross(forceDistance, appliedForce);
    }

    public void UpdateTorqueForces()
    {
        //Was set in scene via text boxes

        //SetForcePositonVector(new Vector2(float.Parse(forcePosX.text), float.Parse(forcePosY.text)));
        //SetForceVector(new Vector2(float.Parse(forceVecX.text), float.Parse(forceVecY.text)));
    }

    public void SetInertia()
    {
        switch (shape)
        {
            case Shape3D.solidSphere:
                localInertiaTensor = new Matrix4x4(
                     new Vector4(0.4f * mass * radius * radius, 0, 0, 0),
                     new Vector4(0, 0.4f * mass * radius * radius, 0, 0),
                     new Vector4(0, 0, 0.4f * mass * radius * radius, 0),
                     new Vector4(0, 0, 0, 1)
                );
                    
                break;
            case Shape3D.hollowSphere:
                localInertiaTensor = new Matrix4x4 (
                    new Vector4(0.66f * mass * radius * radius, 0, 0, 0),
                    new Vector4(0, 0.66f * mass * radius * radius, 0, 0),
                    new Vector4(0 , 0, 0.66f * mass * radius * radius, 0), 
                    new Vector4(0, 0, 0, 1)
                );
                break;
            case Shape3D.solidCube:
                localInertiaTensor = new Matrix4x4 (
                    new Vector4( 0.0833f * mass * ((cuboidDimensions.y * cuboidDimensions.y) + (cuboidDimensions.z * cuboidDimensions.z)), 0, 0, 0),
                    new Vector4( 0, 0.0833f * mass * ((cuboidDimensions.x * cuboidDimensions.x) + (cuboidDimensions.z * cuboidDimensions.z)), 0, 0),
                    new Vector4( 0, 0, 0.0833f * mass * ((cuboidDimensions.y * cuboidDimensions.y) + (cuboidDimensions.y * cuboidDimensions.y)), 0),
                    new Vector4(0, 0, 0, 1)
                );
                break;
            case Shape3D.hollowCube:
                localInertiaTensor = new Matrix4x4 (
                    new Vector4(1.66f * mass * ((cuboidDimensions.y * cuboidDimensions.y) + (cuboidDimensions.z * cuboidDimensions.z)), 0, 0, 0),
                    new Vector4(0, 1.66f * mass * ((cuboidDimensions.x * cuboidDimensions.x) + (cuboidDimensions.z * cuboidDimensions.z)), 0, 0),
                    new Vector4(0, 0, 1.66f * mass * ((cuboidDimensions.y * cuboidDimensions.y) + (cuboidDimensions.y * cuboidDimensions.y)), 0),
                    new Vector4(0, 0, 0, 1)
                );
                break;
            case Shape3D.solidCylinder:
                localInertiaTensor = new Matrix4x4 (
                    new Vector4(0.0833f * mass * (shapeHeight * shapeHeight) + .25f * (radius * radius), 0, 0, 0),
                    new Vector4(0, 0.5f * mass * (radius * radius), 0, 0),
                    new Vector4(0, 0, 0.0833f * mass * (shapeHeight * shapeHeight) + .25f * (radius * radius), 0),
                    new Vector4(0, 0, 0, 1)
                );
                break;
            case Shape3D.solidCone:
                localInertiaTensor = new Matrix4x4 (
                    new Vector4( 0.0375f * mass * (shapeHeight * shapeHeight) + 0.15f * mass * (radius * radius), 0, 0, 0),
                    new Vector4( 0, 0.3f * mass * (radius * radius), 0, 0),
                    new Vector4( 0 , 0, 0.6f * mass * (shapeHeight * shapeHeight) + 0.15f * mass * (radius * radius), 0),
                    new Vector4(0 , 0 , 0, 1)
                    );
                break;
            default:
                Debug.LogWarning("ERROR! UKNOWN SHAPE");
                break;
        }

        //Set the inverse Inertia Tensor
        invLocalInertiaTensor = localInertiaTensor.inverse;
    }

    //Lab 6: Particle 3D

    //Multiply Quaternion via Vector3
    Quaternion multiplyQuaternionByVector(Vector3 vec, Quaternion quaternion)
    {
        //Can be optimized by doing quaternion * vec
        Quaternion quat = new Quaternion(vec.x, vec.y, vec.z, 0);

        return quaternion * quat;
    }

    //Add two Quaternions together
    Quaternion AddQuaternions(Quaternion a, Quaternion b)
    {
        return new Quaternion(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
    }

    //Lab7 Functions

    //Update the world tranformationMatrix every frame
    private void UpdateTransformationMatrix()
    {

        //Deck04 Slide 52 + translation deck01b Slide 34
        worldTransformMatrix = new Matrix4x4(
            new Vector4((rotation.w * rotation.w) + (rotation.x * rotation.x) - (rotation.y * rotation.y) - (rotation.z * rotation.z), 2 * ((rotation.x * rotation.y) - (rotation.w * rotation.z)), 2 * ((rotation.x * rotation.z) + (rotation.w * rotation.y)), position.x),
            new Vector4(2 * ((rotation.x * rotation.y) + (rotation.w * rotation.z)), (rotation.w * rotation.w) - (rotation.x * rotation.x) + (rotation.y * rotation.y) - (rotation.z * rotation.z), 2 * ((rotation.y * rotation.z) + (rotation.w * rotation.x)), position.y),
            new Vector4(2 * ((rotation.x * rotation.z) - (rotation.w * rotation.y)), 2 * ((rotation.y * rotation.z) + (rotation.w * rotation.x)), (rotation.w * rotation.w) - (rotation.x * rotation.x) - (rotation.y * rotation.y) + (rotation.z * rotation.z), position.z),
            new Vector4(0, 0, 0, 1)
            );

        //Update the inverse
 
        //INVERSE OF MATRIX IS EQUAL TO TRANSPOSE
        invWorldTransformMatrix.SetRow(0, worldTransformMatrix.GetColumn(0));
        invWorldTransformMatrix.SetRow(1, worldTransformMatrix.GetColumn(1));
        invWorldTransformMatrix.SetRow(2, worldTransformMatrix.GetColumn(2));

        Matrix4x4 temp = invWorldTransformMatrix;

        //Make Rotation Matrix negative for world transform
        temp.SetRow(0, -invWorldTransformMatrix.GetRow(0));
        temp.SetRow(1, -invWorldTransformMatrix.GetRow(1));
        temp.SetRow(2, -invWorldTransformMatrix.GetRow(2));

        //Last Column for Inverser World Transform is -Rotation^Transpose * Translation(Position)
        invWorldTransformMatrix.SetColumn(3, temp * worldTransformMatrix.GetColumn(3));

        //How is this more optimized than invWorldTransformMatrix = worldTransformMatrix.inverse?
    }

    void UpdateWorldCenterOfMass()
    {
        worldCenterOfMass = localCenterOfMass + transform.position;
    }

    void UpdateWorldInertiaTensor()
    {
        //Change of basis into World Space = WorldMatrix * inverse inertia tensor * Transpose of World Matrix (Also inverse seen in Deck04 Slide 8)
        WorldInertiaTensor = worldTransformMatrix * invLocalInertiaTensor * invWorldTransformMatrix;
    }

    // Start is called before the first frame update
    void Awake()
    {
        rotation = transform.rotation;

        //Lab 2
        SetMass(startingMass);

        //Lab 3
        //Set Starting Inertia based on public variable
        SetInertia();

        //Lab 7
        //Set center of mass local/world
        localCenterOfMass = Vector3.zero;
        worldCenterOfMass = localCenterOfMass + transform.position;
    }

    public void SetStartUpTorque(Vector3 pos, Vector3 force)
    {
        angularVelocity = Vector3.zero;

        ApplyTorque(pos, force);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //integrate
        //CheckInput();

        //ApplyTorque(forcePosVec, forceVec);

        //UpdatePositionExplicitEuler(Time.fixedDeltaTime);
        UpdatePositionKinematic(Time.fixedDeltaTime);
        //UpdateRotationKinematic(Time.fixedDeltaTime);

        //UpdatePositionExplicitEuler(Time.fixedDeltaTime);
        UpdateRotationEuler(Time.fixedDeltaTime);
        UpdateAngularAcceleration();
        UpdateAcceleration();

        //apply to transform
        transform.position = position;
        transform.rotation = rotation;

        //Update variables (Lab 7)
        UpdateTransformationMatrix();
        UpdateWorldCenterOfMass();
        UpdateWorldInertiaTensor();
    }

    public Matrix4x4 GetInvWorldMatrix()
    {
        return invWorldTransformMatrix;
    }
}