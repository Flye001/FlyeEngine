using FlyeEngine.GraphicsEngine;
using OpenTK.Mathematics;
using System.Reflection;

namespace FlyeEngine
{
    internal class GameObject
    {
        private Vector3 _position;
        private Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                UpdateModelMatrix();
            }
        }

        private Vector3 _rotation;
        private Vector3 Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                UpdateModelMatrix();
            }
        }

        private Vector3 _scale;
        private Vector3 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                UpdateModelMatrix();
            }
        }

        public Matrix4 ModelMatrix { get; private set; }
        public Matrix3 ModelNormalMatrix { get; private set; }


        private readonly Mesh? _mesh;
        private readonly Texture? _texture;
        public ShaderTypeEnum ShaderType { get; }
        public Vector3 Color { get; }

        public GameObject(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;

            UpdateModelMatrix();
        }

        public GameObject(Vector3 position, Vector3 rotation, Vector3 scale, Mesh mesh, ShaderTypeEnum shader, Vector3 color)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
            _mesh = mesh;
            ShaderType = shader;
            Color = color;

            UpdateModelMatrix();
        }

        public GameObject(Vector3 position, Vector3 rotation, Vector3 scale, Mesh mesh, Texture texture, ShaderTypeEnum shader)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
            _mesh = mesh;
            _texture = texture;
            ShaderType = shader;
            Color = Vector3.One;

            UpdateModelMatrix();
        }

        private void UpdateModelMatrix()
        {
            ModelMatrix =
                Matrix4.CreateScale(Scale)

                * Matrix4.CreateRotationX(Rotation.X)
                * Matrix4.CreateRotationY(Rotation.Y)
                * Matrix4.CreateRotationZ(Rotation.Z)
                
                * Matrix4.CreateTranslation(Position);
            ModelNormalMatrix = new Matrix3(Matrix4.Transpose(Matrix4.Invert(ModelMatrix)));
        }

        public void Render()
        {
            _texture?.Use();
            _mesh?.Draw();
        }
    }
}
