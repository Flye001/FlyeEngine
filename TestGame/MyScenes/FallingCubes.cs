using FlyeEngine.GraphicsEngine;
using FlyeEngine;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TestGame.MyScenes
{
    internal class FallingCubes : Scene
    {
        private readonly GameObject _plane;
        private const float Speed = 15f;

        public FallingCubes(FlyeEngine.FlyeEngine engine) : base(engine)
        {
            Engine.LightPosition = new Vector3(-10f, 15f, -20f);

            var HP = Engine.AddGameObjectFromWavefront(new Transform() { Rotation = new(0f, -float.Pi / 2f, 0f) }, "MyObjects\\harrypotter\\hp", ShaderTypeEnum.SingleColorWithLight);
            HP.AddBoxCollider(new Vector3(25f, 0.5f, 25f), new Vector3(5f, -0.5f, 0f));

            // Falling cubes
            for (int i = 0; i < 6; i++)
            {
                var transform = new Transform()
                {
                    Position = new(15 + -5f * i, 10f + i * 5, -3f)
                };
                var cube = Engine.AddGameObjectFromWavefront(transform, "MyObjects\\cube\\cube", ShaderTypeEnum.SingleColorWithLight);
                cube.AddRigidBody(2f);
                cube.AddBoxCollider(new Vector3(2.25f), Vector3.Zero);
            }

            _plane =
                Engine.AddGameObjectFromWavefront(new Transform() { Position = new Vector3(-5f, 5f, 0f) }, "MyObjects\\cube\\cube", ShaderTypeEnum.SingleColorWithLight);
            _plane.AddBoxCollider(new Vector3(3f), Vector3.Zero);
            _plane.AddRigidBody(5);
            //solidCube.AddBoxCollider(new Vector3(2f), Vector3.Zero);

            // SUN
            Transform sunTransform = new()
            {
                Position = Engine.LightPosition,
                Scale = new Vector3(0.25f)
            };
            Engine.AddGameObjectFromWavefront(sunTransform, "MyObjects\\cube\\cube", ShaderTypeEnum.SingleColor);
        }

        public override void OnUpdate(float deltaTime, KeyboardState kb)
        {
            if (kb.IsKeyDown(Keys.A))
            {
                //_plane.UpdatePosition(_plane.Position + new Vector3(Speed * deltaTime, 0f, 0f));
                _plane.RigidBody.AddForce(new Vector3(Speed * deltaTime, 0f, 0f));
            }
            if (kb.IsKeyDown(Keys.D))
            {
                //_plane.UpdatePosition(_plane.Position - new Vector3(Speed * deltaTime, 0f, 0f));
                _plane.RigidBody.AddForce(new Vector3(-Speed * deltaTime, 0f, 0f));
            }
            if (kb.IsKeyDown(Keys.W))
            {
                //_plane.UpdatePosition(_plane.Position + new Vector3(0f, 0f, Speed * deltaTime));
                _plane.RigidBody.AddForce(new Vector3(0f, 0f, Speed * deltaTime));
            }
            if (kb.IsKeyDown(Keys.S))
            {
                //_plane.UpdatePosition(_plane.Position - new Vector3(0f, 0f, Speed * deltaTime));
                _plane.RigidBody.AddForce(new Vector3(0f, 0f, -Speed * deltaTime));
            }
        }
    }
}
