using OpenTK.Mathematics;

namespace FlyeEngine.PhysicsEngine
{
    internal class PhysicsEngine
    {
        private const float tolerance = 0.1f;
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
                    //if (b.BoxCollider == null) continue;

                    if (a.BoxCollider.MinX <= b.BoxCollider.MaxX &&
                        a.BoxCollider.MaxX >= b.BoxCollider.MinX &&
                        a.BoxCollider.MinY <= b.BoxCollider.MaxY &&
                        a.BoxCollider.MaxY >= b.BoxCollider.MinY &&
                        a.BoxCollider.MinZ <= b.BoxCollider.MaxZ &&
                        a.BoxCollider.MaxZ >= b.BoxCollider.MinZ)
                    {
                        // Calculate the overlap along each axis
                        //float overlapX = Math.Min(a.BoxCollider.MaxX - b.BoxCollider.MinX, b.BoxCollider.MaxX - a.BoxCollider.MinX);
                        //float overlapY = Math.Min(a.BoxCollider.MaxY - b.BoxCollider.MinY, b.BoxCollider.MaxY - a.BoxCollider.MinY);
                        //float overlapZ = Math.Min(a.BoxCollider.MaxZ - b.BoxCollider.MinZ, b.BoxCollider.MaxZ - a.BoxCollider.MinZ);

                        Vector3 collisionNormal = Vector3.Zero;

                        if (Math.Abs(a.BoxCollider.MinX - b.BoxCollider.MaxX) < tolerance)
                            collisionNormal = new Vector3(-1, 0, 0);
                        else if (Math.Abs(a.BoxCollider.MaxX - b.BoxCollider.MinX) < tolerance)
                            collisionNormal = new Vector3(1, 0, 0);
                        else if (Math.Abs(a.BoxCollider.MinY - b.BoxCollider.MaxY) < tolerance)
                            collisionNormal = new Vector3(0, -1, 0);
                        else if (Math.Abs(a.BoxCollider.MaxY - b.BoxCollider.MinY) < tolerance)
                            collisionNormal = new Vector3(0, 1, 0);
                        else if (Math.Abs(a.BoxCollider.MinZ - b.BoxCollider.MaxZ) < tolerance)
                            collisionNormal = new Vector3(0, 0, -1);
                        else if (Math.Abs(a.BoxCollider.MaxZ - b.BoxCollider.MinZ) < tolerance)
                            collisionNormal = new Vector3(0, 0, 1);

                        a.RigidBody.CollisionNormal = collisionNormal;
                    }
                }
            }
        }
    }
}
