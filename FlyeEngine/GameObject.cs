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
        public ShaderTypeEnum ShaderType { get; }
        public Vector3 Color { get; }

        public GameObject(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }

        public GameObject(Vector3 position, Vector3 rotation, Vector3 scale, Mesh mesh, ShaderTypeEnum shader, Vector3 color)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
            _mesh = mesh;
            ShaderType = shader;
            Color = color;
        }

        public void Render()
        {
            _mesh?.Draw();
        }
    }
}
