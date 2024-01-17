using FlyeEngine;
using FlyeEngine.GraphicsEngine;
using OpenTK.Mathematics;

namespace TestGame
{
    internal class Program
    {
        private static GameObject plane;
        private static bool forward = true;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var game = new FlyeEngine.FlyeEngine(1920, 1080, "Test Game");

            game.LightPosition = new Vector3(-2f, 15f, 0f);

            game.OnUpdate += OnUpdate;

            // Add game objects
            game.AddGameObjectFromWavefront(new Transform(), "MyObjects\\harrypotter", ShaderTypeEnum.SingleColor);
            
            var cube = game.AddGameObjectFromWavefront(new Transform() {Position = new(-5f, 35f, 3f)}, "MyObjects\\cube", ShaderTypeEnum.SingleColorWithLight);
            cube.AddRigidBody(2f);
            cube.AddBoxCollider(new Vector3(2f));

            var solidCube =
                game.AddGameObjectFromWavefront(new Transform() {Position = new Vector3(0f, 2f, 0f)}, "MyObjects\\cube", ShaderTypeEnum.SingleColor);
            solidCube.AddBoxCollider(new Vector3(2f));

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

        private static void OnUpdate(float deltaTime)
        {
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
