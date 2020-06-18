#ifndef PHYSICSDLL_H
#define PHYSICSDLL_H

#include "lib.h"
#include <math.h>

#pragma pack (push, 1)

struct Vector3
{
	float x;
	float y;
	float z;

	Vector3 operator*(const float a)
	{
		Vector3 scaled;

		scaled.x = x * a;
		scaled.y = y * a;
		scaled.z = z * a;

		return scaled;
	}

	Vector3 operator/(const float a)
	{
		Vector3 div;

		div.x = x / a;
		div.y = y / a;
		div.z = z / a;

		return div;
	}

	Vector3 operator+(const Vector3 vec)
	{
		Vector3 add;

		add.x = x + vec.x;
		add.y = y + vec.y;
		add.z = z + vec.z;

		return add;
	}

	Vector3 operator-(const Vector3 vec)
	{
		Vector3 sub;

		sub.x = x - vec.x;
		sub.y = y - vec.y;
		sub.z = z - vec.z;

		return sub;
	}

	float magnitude()
	{
		float temp = (x * x) + (y * y) + (z * z);
		float result = sqrtf(temp);
		return result;
	}

	Vector3 normalized()
	{
		Vector3 newVec;

		float mag = magnitude();
	
		newVec.x = x / mag;
		newVec.y = y / mag;
		newVec.z = z / mag;

		return newVec;
	}

	Vector3 operator-() const
	{
		Vector3 vec;
		vec.x = -x;
		vec.y = -y;
		vec.z = -z;

		return vec;
	}
};

#pragma pack (pop)

float Dot(Vector3 a, Vector3 b)
{
	return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
}

Vector3 Project(Vector3 a, Vector3 b)
{
	float bLengthSquared = Dot(b, b);
	return b * (Dot(b, a) / bLengthSquared);
}



#ifdef __cplusplus
extern "C"
{
#else //!__cplusplus

#endif //__cplusplus

PHYSICSDLL_SYMBOL
void GenerateForce_Gravity(float particleMass, float gravitationalConstant, Vector3* vec);

PHYSICSDLL_SYMBOL
void GenerateForce_normal(Vector3* vectors);

PHYSICSDLL_SYMBOL
void GenerateForce_sliding(Vector3* vectors);
PHYSICSDLL_SYMBOL
void GenerateForce_friction_static(Vector3* vectors, float frictionCoefficient_static);

PHYSICSDLL_SYMBOL
void GenerateForce_friction_kinetic(Vector3* vectors, float frictionCoefficient_kinetic);

PHYSICSDLL_SYMBOL
void  GenerateForce_drag(Vector3* vectors, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient);

PHYSICSDLL_SYMBOL
void GenerateForce_spring(Vector3* vectors, float springRestingLength, float springStiffnessCoefficient);

PHYSICSDLL_SYMBOL
void UpdatePositionExplicitEuler(Vector3* vectors, float dt);

PHYSICSDLL_SYMBOL
void UpdatePositionKinematic(Vector3* vectors, float dt);

PHYSICSDLL_SYMBOL
void IntegrateAngularVelocity(Vector3* vectors, float dt);

PHYSICSDLL_SYMBOL
int GetTest() { return 5; };

PHYSICSDLL_SYMBOL
int InitFoo(int f_new);

PHYSICSDLL_SYMBOL
int DoFoo(int bar);

PHYSICSDLL_SYMBOL
int KungFoo();

#ifdef __cplusplus
}
#else //!__cplusplus

#endif //__cplusplus

#endif //!PHYSICSDLL_H