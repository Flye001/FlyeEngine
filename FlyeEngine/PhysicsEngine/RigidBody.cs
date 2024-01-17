using OpenTK.Mathematics;

namespace FlyeEngine.PhysicsEngine
{
    internal class RigidBody
    {
        public float Mass { get; private set; }
        public Vector3 Velocity { get; private set; } = Vector3.Zero;
        public Vector3 Acceleration { get; private set; } = new Vector3(0f, -9.8f, 0f);

        public RigidBody(float mass)
        {
            Mass = mass;
        }

        public void Update(Action<Vector3> updatePosition, Vector3 currentPosition, float deltaTime)
        {
            // s = ut + 1/2 * a * t^2

            // Update Position
            var newPosition = currentPosition;
            newPosition += Velocity * deltaTime + 0.5f * Acceleration * deltaTime * deltaTime;
            updatePosition(newPosition);

            // Update Velocity
            Velocity += Acceleration * deltaTime;
        }
    }
}
