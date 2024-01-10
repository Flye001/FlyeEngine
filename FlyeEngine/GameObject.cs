using FlyeEngine.GraphicsEngine;
using OpenTK.Mathematics;

namespace FlyeEngine
{
    public class GameObject
    {
        private Vector3 _position;
        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                UpdateModelMatrix();
            }
        }

        private Vector3 _rotation;
        public Vector3 Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                UpdateModelMatrix();
            }
        }

        private Vector3 _scale;
        public Vector3 Scale
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
        public ShaderTypeEnum ShaderType { get; }

        public GameObject(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;

            UpdateModelMatrix();
        }
        
        public GameObject(Vector3 position, Vector3 rotation, Vector3 scale, Mesh mesh, ShaderTypeEnum shader)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
            _mesh = mesh;
            ShaderType = shader;

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

        public Dictionary<string, int>? GetTextures()
        {
            return _mesh?.GetTextures();
        }

        public void Render()
        {
            _mesh?.Draw();
        }
    }
}
