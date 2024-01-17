namespace FlyeEngine.PhysicsEngine
{
    internal class PhysicsEngine
    {
        public void CheckCollisions(IEnumerable<GameObject> gameObjects)
        {
            Queue<GameObject> objectsToCheck = new(gameObjects);

            while (objectsToCheck.Count > 0)
            {
                var a = objectsToCheck.Dequeue();
                if (!a.HasBoxCollider) continue;

                foreach (var b in objectsToCheck)
                {
                    if (!b.HasBoxCollider) continue;

                    if (a.BoxCollider.MinX <= b.BoxCollider.MaxX &&
                        a.BoxCollider.MaxX >= b.BoxCollider.MinX &&
                        a.BoxCollider.MinY <= b.BoxCollider.MaxY &&
                        a.BoxCollider.MaxY >= b.BoxCollider.MinY &&
                        a.BoxCollider.MinZ <= b.BoxCollider.MaxZ &&
                        a.BoxCollider.MaxZ >= b.BoxCollider.MinZ)
                    {
                        if (a.RigidBody != null) a.RigidBody.IsColliding = true;
                        if (b.RigidBody != null)  b.RigidBody.IsColliding = true;
                    }
                }
            }
        }
    }
}
