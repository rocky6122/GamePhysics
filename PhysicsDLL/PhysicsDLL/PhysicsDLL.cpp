#include "PhysicsDLL.h"

#include "Foo.h"

Foo* instance = 0;

void GenerateForce_Gravity(float particleMass, float gravitationalConstant, Vector3* vec)
{
	Vector3& result = vec[0];
	Vector3& worldUp = vec[1];

	result = worldUp * particleMass * gravitationalConstant;
}

// f_normal = proj(-f_gravity, surfaceNormal_unit)
void GenerateForce_normal(Vector3* vectors)
{
	Vector3& result = vectors[0];
	Vector3& f_gravity = vectors[1];
	Vector3& surfaceNormal_unit = vectors[2];

	result = Project(f_gravity, surfaceNormal_unit);

	result = -result;
}

// f_sliding = f_gravity + f_normal
void GenerateForce_sliding(Vector3* vectors)
{
	Vector3& result = vectors[0];
	Vector3& f_gravity = vectors[1];
	Vector3& f_normal = vectors[2];

	result = f_gravity + f_normal;
}

// f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)
void GenerateForce_friction_static(Vector3* vectors, float frictionCoefficient_static)
{
	Vector3& result = vectors[0];
	Vector3& f_normal = vectors[1];
	Vector3& f_opposing = vectors[2];

	float max = f_normal.magnitude() * frictionCoefficient_static;
	float magOpposing = f_opposing.magnitude();

	result = magOpposing < max ? -f_opposing : -f_opposing * max / magOpposing;
}

// f_friction_k = -coeff*|f_normal| * unit(vel)
void GenerateForce_friction_kinetic(Vector3* vectors, float frictionCoefficient_kinetic)
{
	Vector3& result = vectors[0];
	Vector3& f_normal = vectors[1];
	Vector3& particleVelocity = vectors[2];


	result = particleVelocity.normalized() * f_normal.magnitude() * -frictionCoefficient_kinetic;
}

// f_drag = (p * u^2 * area * coeff)/2
void  GenerateForce_drag(Vector3* vectors, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
{
	Vector3& result = vectors[0];
	Vector3& particleVelocity = vectors[1];
	Vector3& fluidVelocity = vectors[2];

	//calculate u
	Vector3 particleSpeed = fluidVelocity - particleVelocity;

	result = ((particleSpeed * particleSpeed.magnitude()) * fluidDensity * objectArea_crossSection * objectDragCoefficient) * 0.5f;
}

// f_spring = -coeff*(spring length - spring resting length)
void GenerateForce_spring(Vector3* vectors, float springRestingLength, float springStiffnessCoefficient)
{
	Vector3& result = vectors[0];
	Vector3& particlePosition = vectors[1];
	Vector3& anchorPosition = vectors[2];

	Vector3 springForce = anchorPosition - particlePosition;

	//calc magnitude
	float springMagnitude = springForce.magnitude();

	springMagnitude *= springStiffnessCoefficient;

	result = springForce * springMagnitude;
}

void UpdatePositionExplicitEuler(Vector3* vectors, float dt)
{
	Vector3& position = vectors[0];
	Vector3& velocity = vectors[1];
	Vector3& acceleration = vectors[2];

	position =  position + velocity * dt;
	velocity = velocity + acceleration * dt;
}

void UpdatePositionKinematic(Vector3* vectors, float dt)
{
	Vector3& position = vectors[0];
	Vector3& velocity = vectors[1];
	Vector3& acceleration = vectors[2];

	//x(t + dt) = x(t) + v(t) * dt + .5 * a(t) * (dt * dt)
	position = position + velocity * dt + (acceleration * 0.5f) * (dt * dt);

	//v(t + dt) = v + a(d) * dt
	velocity = velocity + acceleration * dt;
}

void IntegrateAngularVelocity(Vector3* vectors, float dt)
{
	Vector3& angularVelocity = vectors[0];
	Vector3& angularAcceleration = vectors[1];

	angularVelocity = angularVelocity + angularAcceleration * dt;
}

int InitFoo(int f_new)
{
	if (!instance)
	{
		instance = new Foo(f_new);
		return 1;
	}

	return 0;
}

int DoFoo(int bar)
{
	if (instance)
	{
		int result = instance->foo(bar);
		return result;
	}

	return 0;
}

int KungFoo()
{
	if (instance)
	{
		delete instance;
		instance = 0;
		return 1;
	}

	return 0;
}