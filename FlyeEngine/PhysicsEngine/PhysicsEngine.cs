using OpenTK.Mathematics;

namespace FlyeEngine.PhysicsEngine
{
    internal class PhysicsEngine
    {
        public void CheckCollisions(IEnumerable<GameObject> gameObjectsWithRigidBodies, IEnumerable<GameObject> gameObjectsWithColliders)
        {
            Queue<GameObject> objectsToCheck = new(gameObjectsWithRigidBodies);
            var objectsWithColliders = gameObjectsWithColliders as GameObject[] ?? gameObjectsWithColliders.ToArray();

            while (objectsToCheck.Count > 0)
            {
                var a = objectsToCheck.Dequeue();
                if (a.BoxCollider == null) continue;

                foreach (var b in objectsWithColliders)
                {
                    if (a == b) continue;
                    if (b.BoxCollider == null) continue;

                    if (a.BoxCollider.MinX <= b.BoxCollider.MaxX &&
                        a.BoxCollider.MaxX >= b.BoxCollider.MinX &&
                        a.BoxCollider.MinY <= b.BoxCollider.MaxY &&
                        a.BoxCollider.MaxY >= b.BoxCollider.MinY &&
                        a.BoxCollider.MinZ <= b.BoxCollider.MaxZ &&
                        a.BoxCollider.MaxZ >= b.BoxCollider.MinZ)
                    {
                        // Calculate the overlap along each axis
                        float overlapX = Math.Min(a.BoxCollider.MaxX - b.BoxCollider.MinX, b.BoxCollider.MaxX - a.BoxCollider.MinX);
                        float overlapY = Math.Min(a.BoxCollider.MaxY - b.BoxCollider.MinY, b.BoxCollider.MaxY - a.BoxCollider.MinY);
                        float overlapZ = Math.Min(a.BoxCollider.MaxZ - b.BoxCollider.MinZ, b.BoxCollider.MaxZ - a.BoxCollider.MinZ);

                        if (overlapX < overlapY && overlapX < overlapZ)
                        {
                            // Collision in X axis!
                            a.RigidBody.AddForce(new Vector3(-a.RigidBody.Velocity.X, 0, 0));
                        }
                        else if (overlapY < overlapZ)
                        {
                            // Collision in Y axis!
                            //a.RigidBody.AddForce(new Vector3(0, -a.RigidBody.Velocity.Y, 0));
                        }
                        else
                        {
                            // Collision in Z axis!
                            a.RigidBody.AddForce(new Vector3(0, 0, -a.RigidBody.Velocity.Z));
                        }
                    }
                }
            }
        }
    }
}
