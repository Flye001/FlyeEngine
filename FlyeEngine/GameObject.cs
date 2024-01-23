using FlyeEngine.GraphicsEngine;
using FlyeEngine.PhysicsEngine;
using OpenTK.Mathematics;

namespace FlyeEngine
{
    public class GameObject
    {
        public Vector3 Position { get; private set; }
        public Vector3 PreviousPosition { get; private set; }

        public void UpdatePosition(Vector3 position)
        {
            PreviousPosition = Position;
            Position = position;
            BoxCollider?.UpdatePosition(position);
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

        public RigidBody? RigidBody;
        public BoxCollider? BoxCollider;
        public Matrix4? ColliderModelMatrix => BoxCollider?.ModelMatrix;

        private readonly List<Script> _scripts;

        public GameObject(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Position = position;
            PreviousPosition = position;
            _rotation = rotation;
            _scale = scale;
            _scripts = new List<Script>();

            UpdateModelMatrix();
        }
        
        public GameObject(Vector3 position, Vector3 rotation, Vector3 scale, Mesh mesh, ShaderTypeEnum shader)
        {
            Position = position;
            PreviousPosition = position;
            _rotation = rotation;
            _scale = scale;
            _mesh = mesh;
            ShaderType = shader;
            _scripts = new List<Script>();

            UpdateModelMatrix();
        }

        public void AddScript(Script script)
        {
            _scripts.Add(script);            
        }

        public void AddRigidBody(float mass)
        {
            RigidBody = new RigidBody(mass);
        }

        public void AddBoxCollider(Vector3 dimensions, Vector3 offset)
        {
            BoxCollider = new BoxCollider(dimensions, Position, offset);
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
            foreach (var script in _scripts)
            {
                script.OnUpdate(deltaTime);
            }
            RigidBody?.Update(UpdatePosition, Position, deltaTime);
        }

        public void Render()
        {
            _mesh?.Draw();
        }

        public void RenderBoxCollider()
        {
            BoxCollider?.RenderWireframe();
        }
    }
}
