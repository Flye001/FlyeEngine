using FlyeEngine.GraphicsEngine;
using OpenTK.Mathematics;

namespace FlyeEngine
{
    internal class GameObject
    {
        private Vector3 _position;
        private Vector3 _rotation;
        private Vector3 _scale;

        private readonly Mesh? _mesh;

        public GameObject(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }

        public GameObject(Vector3 position, Vector3 rotation, Vector3 scale, Mesh mesh)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
            _mesh = mesh;
        }

        public void Render()
        {
            if (_mesh == null) return;

            _mesh.Draw();
        }
    }
}
