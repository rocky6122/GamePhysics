using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator
{
    // f = mg
    public static Vector3 GenerateForce_Gravity(float particleMass, float gravitationalConstant, Vector3 worldUp)
    {
        if (particleMass == Mathf.Infinity)
        {
            return Vector3.zero;
        }

        //create the result and parameter array
        Vector3 result = Vector3.zero;
        Vector3[] parameters = new Vector3[2];

        //set outgoing vectors
        parameters[0] = result;
        parameters[1] = worldUp;

        PhysicsDLL.GenerateForce_Gravity(particleMass, gravitationalConstant, parameters);

        //result changed result
        return parameters[0];
    }

    // f_normal = proj(-f_gravity, surfaceNormal_unit)
    public static Vector3 GenerateForce_normal(Vector3 f_gravity, Vector3 surfaceNormal_unit)
    {
        //create the result and parameter array
        Vector3 result = Vector3.zero;
        Vector3[] parameters = new Vector3[3];

        //set outgoing vectors
        parameters[0] = result;
        parameters[1] = f_gravity;
        parameters[2] = surfaceNormal_unit;

        PhysicsDLL.GenerateForce_normal(parameters);

        //result changed result
        return parameters[0];
    }

    // f_sliding = f_gravity + f_normal
    public static Vector3 GenerateForce_sliding(Vector3 f_gravity, Vector3 f_normal)
    {
        //create result and parameters
        Vector3 result = Vector3.zero;
        Vector3[] parameters = new Vector3[3];

        //set ougoing vectors
        //set outgoing vectors
        parameters[0] = result;
        parameters[1] = f_gravity;
        parameters[2] = f_normal;

        PhysicsDLL.GenerateForce_sliding(parameters);

        return parameters[0];
    }

    // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)
    public static Vector3 GenerateForce_friction_static(Vector3 f_normal, Vector3 f_opposing, float frictionCoefficient_static)
    {
        //create result and parameters
        Vector3 result = Vector3.zero;
        Vector3[] parameters = new Vector3[3];

        //Set outgoing vectors
        parameters[0] = result;
        parameters[1] = f_normal;
        parameters[2] = f_opposing;

        PhysicsDLL.GenerateForce_friction_static(parameters, frictionCoefficient_static);

        return parameters[0];
    }

    // f_friction_k = -coeff*|f_normal| * unit(vel)
    public static Vector3 GenerateForce_friction_kinetic(Vector3 f_normal, Vector3 particleVelocity, float frictionCoefficient_kinetic)
    {
        //create result and parameters
        Vector3 result = Vector3.zero;
        Vector3[] parameters = new Vector3[3];

        //Set outgoing vectors
        parameters[0] = result;
        parameters[1] = f_normal;
        parameters[2] = particleVelocity;

        PhysicsDLL.GenerateForce_friction_kinetic(parameters, frictionCoefficient_kinetic);

        return parameters[0];
    }

    // f_drag = (p * u^2 * area * coeff)/2
    public static Vector3 GenerateForce_drag(Vector3 particleVelocity, Vector3 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        ////create result and parameters
        //Vector3 result = Vector3.zero;
        //Vector3[] parameters = new Vector3[3];

        ////Set outgoing vectors
        //parameters[0] = result;
        //parameters[1] = particleVelocity;
        //parameters[2] = fluidVelocity;

        //PhysicsDLL.GenerateForce_drag(parameters, fluidDensity, objectArea_crossSection, objectDragCoefficient);

        Vector3 particleSpeed = fluidVelocity - particleVelocity;

        Vector3 f_drag = (fluidDensity * (particleSpeed.magnitude * particleSpeed) * objectArea_crossSection * objectDragCoefficient) * 0.5f;

        return f_drag;
    }

    // f_spring = -coeff*(spring length - spring resting length)
    public static Vector3 GenerateForce_spring(Vector3 particlePosition, Vector3 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        ////create result and parameters
        //Vector3 result = Vector3.zero;
        //Vector3[] parameters = new Vector3[3];

        ////Set outgoing vectors
        //parameters[0] = result;
        //parameters[1] = particlePosition;
        //parameters[2] = anchorPosition;

        //PhysicsDLL.GenerateForce_spring(parameters, springRestingLength, springStiffnessCoefficient);

        //return parameters[0];

        Vector3 springForce = (particlePosition - anchorPosition);
        float springLength = springForce.magnitude;
        Vector3 f_spring = (springForce * -springStiffnessCoefficient * (springLength - springRestingLength)) / springLength;
        return f_spring;
    }
}
