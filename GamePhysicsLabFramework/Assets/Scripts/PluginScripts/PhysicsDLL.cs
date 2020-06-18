using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class PhysicsDLL
{
    [DllImport("PhysicsDLL")]
    public static extern int InitFoo(int f_new = 0);

    [DllImport("PhysicsDLL")]
    public static extern int DoFo(int bar = 0);

    [DllImport("PhysicsDLL")]
    public static extern int KungFoo();

    [DllImport("PhysicsDLL")]
    public static extern void GenerateForce_Gravity(float particleMass, float gravitationalConstant, [In, Out] Vector3[] vec);

    [DllImport("PhysicsDLL")]
    public static extern void GenerateForce_normal([In, Out] Vector3[] vectors);

    [DllImport("PhysicsDLL")]
    public static extern void GenerateForce_sliding([In, Out] Vector3[] vectors);
    [DllImport("PhysicsDLL")]
    public static extern void GenerateForce_friction_static([In, Out] Vector3[] vectors, float frictionCoefficient_static);

    [DllImport("PhysicsDLL")]
    public static extern void GenerateForce_friction_kinetic([In, Out] Vector3[] vectors, float frictionCoefficient_kinetic);

    [DllImport("PhysicsDLL")]
    public static extern void GenerateForce_drag([In, Out] Vector3[] vectors, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient);

    [DllImport("PhysicsDLL")]
    public static extern void GenerateForce_spring([In, Out] Vector3[] vectors, float springRestingLength, float springStiffnessCoefficient);

    [DllImport("PhysicsDLL")]
    public static extern void UpdatePositionExplicitEuler([In, Out] Vector3[] vectors, float dt);

    [DllImport("PhysicsDLL")]
    public static extern void UpdatePositionKinematic([In, Out] Vector3[] vectors, float dt);

    [DllImport("PhysicsDLL")]
    public static extern void IntegrateAngularVelocity([In, Out] Vector3[] vectors, float dt);

    // USED FOR ENSURING THE NEW DLL IS CORRECT
    [DllImport("PhysicsDLL")]
    public static extern int GetTest();
}
