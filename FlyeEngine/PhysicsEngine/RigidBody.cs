using OpenTK.Mathematics;

namespace FlyeEngine.PhysicsEngine
{
    public class RigidBody
    {
        public float Mass { get; private set; }
        public Vector3 Velocity { get; private set; } = Vector3.Zero;
        public Vector3 Acceleration { get; private set; } = new Vector3(0f, -9.8f, 0f);
        public List<Vector3> CollisionNormal { get; set; } = new();

        public RigidBody(float mass)
        {
            Mass = mass;
        }

        public void AddForce(Vector3 force)
        {
            Velocity += force;
        }
        
        public void Update(Action<Vector3> updatePosition, Vector3 currentPosition, float deltaTime)
        {
            // s = ut + 1/2 * a * t^2

            foreach (var normal in CollisionNormal)
            {
                if (Vector3.Dot(Velocity, normal) > 0)
                {
                    Velocity -= Vector3.Dot(Velocity, normal) * normal;
                }
            }

            // Check collisions
            var movement = Velocity * deltaTime + 0.5f * Acceleration * deltaTime * deltaTime;

            foreach (var normal in CollisionNormal)
            {
                // If the movement is in the direction of the collision, don't move
                if (Vector3.Dot(movement, normal) > 0)
                {
                    movement -= Vector3.Dot(movement, normal) * normal;
                }
            }
            

            // Update Position
            var newPosition = currentPosition;
            newPosition += movement;
            updatePosition(newPosition);

            // Update Velocity
            Velocity += Acceleration * deltaTime;

        }
    }
}
