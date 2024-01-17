using FlyeEngine;
using FlyeEngine.GraphicsEngine;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TestGame
{
    internal class Program
    {
        private static GameObject plane;
        private static float speed = 15f;
        private static bool forward = true;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var myCamera = new StaticCamera(new Vector3(0f, 20f, -25f), Vector3.Zero);

            var game = new FlyeEngine.FlyeEngine(1920, 1080, "Test Game", myCamera);

            game.LightPosition = new Vector3(-10f, 15f, -20f);

            game.OnUpdate += OnUpdate;

            // Add game objects
            var HP = game.AddGameObjectFromWavefront(new Transform() {Rotation = new(0f, -float.Pi / 2f, 0f)}, "MyObjects\\harrypotter", ShaderTypeEnum.SingleColorWithLight);
            HP.AddBoxCollider(new Vector3(25f, 0.5f, 25f), new Vector3(5f, -0.5f, 0f));
            
            // Falling cubes
            for (int i = 0; i < 6; i++)
            {
                var transform = new Transform()
                {
                    Position = new(15 + -5f * i, 30f + i * 7, -3f)
                };
                var cube = game.AddGameObjectFromWavefront(transform, "MyObjects\\cube", ShaderTypeEnum.SingleColorWithLight);
                cube.AddRigidBody(2f);
                cube.AddBoxCollider(new Vector3(2.25f), Vector3.Zero);
            }

            plane =
                game.AddGameObjectFromWavefront(new Transform() {Position = new Vector3(-5f, 2f, 0f)}, "MyObjects\\cube", ShaderTypeEnum.SingleColorWithLight);
            //solidCube.AddBoxCollider(new Vector3(2f), Vector3.Zero);

            // SUN
            Transform sunTransform = new()
            {
                Position = game.LightPosition,
                Scale = new Vector3(0.25f)
            };
            game.AddGameObjectFromWavefront(sunTransform, "MyObjects\\sphere", ShaderTypeEnum.SingleColor);

            game.StartGame();
        }

        private void SpyroMarioLevel(FlyeEngine.FlyeEngine game)
        {
            Transform peachTrans = new()
            {
                Position = new Vector3(105, -3f, -16f),
                Rotation = new Vector3(0, float.Pi, 0f),
                Scale = new Vector3(15f)
            };
            game.AddGameObjectFromWavefront(peachTrans, "MyObjects\\peach", ShaderTypeEnum.Texture);

            Transform planeTrans = new()
            {
                Position = new Vector3(0),
                Rotation = new Vector3(0, float.Pi / 2f, float.Pi / 2f),
                Scale = new Vector3(0.1f)
            };
            game.AddGameObjectFromWavefront(planeTrans, "MyObjects\\plane2", ShaderTypeEnum.Texture);


            Transform spyroTransform = new()
            {
                Position = new Vector3(0, 0, 0)
            };
            game.AddGameObjectFromWavefront(spyroTransform, "MyObjects\\spyro", ShaderTypeEnum.Texture);
        }

        private static void OnUpdate(float deltaTime, KeyboardState kb)
        {
            
            if (kb.IsKeyDown(Keys.A))
            {
                plane.UpdatePosition(plane.Position + new Vector3(speed * deltaTime, 0f, 0f));
            }
            if (kb.IsKeyDown(Keys.D))
            {
                plane.UpdatePosition(plane.Position - new Vector3(speed * deltaTime, 0f, 0f));
            }
            if (kb.IsKeyDown(Keys.W))
            {
                plane.UpdatePosition(plane.Position + new Vector3(0f, 0f, speed * deltaTime));
            }
            if (kb.IsKeyDown(Keys.S))
            {
                plane.UpdatePosition(plane.Position - new Vector3(0f, 0f, speed * deltaTime));
            }
            //var speed = 15f;
            //if (plane.Position.X >= 40)
            //{
            //    forward = false;
            //    plane.Rotation = new Vector3(0, -float.Pi / 2f, -float.Pi / 9f);
            //}

            //if (plane.Position.X <= -40)
            //{
            //    forward = true;
            //    plane.Rotation = new Vector3(0, float.Pi / 2f, -float.Pi / 9f);
            //}

            //if (forward)
            //{
            //    plane.Position += new Vector3(1f, 0f, 0f) * speed * deltaTime;
            //}
            //else
            //{
            //    plane.Position += new Vector3(1f, 0f, 0f) * speed * deltaTime;
            //}
        }
    }
}
