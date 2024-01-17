using FlyeEngine.GraphicsEngine;
using FlyeEngine.PhysicsEngine;
using OpenTK.Mathematics;

namespace FlyeEngine
{
    public class GameObject
    {
        public Vector3 Position { get; private set; }
        //public Vector3 Position
        //{
        //    get => _position;
        //    set
        //    {
        //        _position = value;
        //        UpdateModelMatrix();
        //    }
        //}

        public void UpdatePosition(Vector3 position)
        {
            Position = position;
            _boxCollider?.UpdatePosition(position);
            UpdateModelMatrix();
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

        private RigidBody? _rigidBody;
        private BoxCollider? _boxCollider;
        public bool HasBoxCollider => _boxCollider != null;
        public Matrix4? ColliderModelMatrix => _boxCollider?.ModelMatrix;

        public GameObject(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Position = position;
            _rotation = rotation;
            _scale = scale;

            UpdateModelMatrix();
        }
        
        public GameObject(Vector3 position, Vector3 rotation, Vector3 scale, Mesh mesh, ShaderTypeEnum shader)
        {
            Position = position;
            _rotation = rotation;
            _scale = scale;
            _mesh = mesh;
            ShaderType = shader;

            UpdateModelMatrix();
        }

        public void AddRigidBody(float mass)
        {
            _rigidBody = new RigidBody(mass);
        }

        public void AddBoxCollider(Vector3 dimensions, Vector3 offset)
        {
            _boxCollider = new BoxCollider(dimensions, Position, offset);
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

        public void Update(float deltaTime)
        {
            _rigidBody?.Update(UpdatePosition, Position, deltaTime);
        }

        public void Render()
        {
            _mesh?.Draw();
        }

        public void RenderBoxCollider()
        {
            _boxCollider?.RenderWireframe();
        }
    }
}
