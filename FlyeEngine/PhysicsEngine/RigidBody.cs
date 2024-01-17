using OpenTK.Mathematics;

namespace FlyeEngine.PhysicsEngine
{
    internal class RigidBody
    {
        public float Mass { get; private set; }
        public float Velocity { get; private set; } = 0f;
        public float Acceleration { get; private set; } = 9.8f;

        public RigidBody(float mass)
        {
            Mass = mass;
        }

        public void Update(Action<Vector3> updatePosition, Vector3 currentPosition, float deltaTime)
        {
            // s = ut + 1/2 * a * t^2

            // Update Position
            var newPosition = currentPosition;
            newPosition.Y -= Velocity * deltaTime + 0.5f * Acceleration * deltaTime * deltaTime;
            updatePosition(newPosition);

            // Update Velocity
            Velocity += Acceleration * deltaTime;
        }
    }
}
