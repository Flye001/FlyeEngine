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

            game.LightPosition = new Vector3(0, 30f, 0f);

            game.OnUpdate += OnUpdate;

            // Add game objects
            //Transform objTrans = new()
            //{
            //    Position = new Vector3(-5f, 0f, 15f),
            //    Rotation = Vector3.Zero,
            //    Scale = new Vector3(1f)
            //};
            //game.AddGameObjectWithMesh(objTrans, "MyObjects/cube.obj", ShaderTypeEnum.SingleColorWithLight, new Vector3(1f, 0.2f, 0.6f));

            //Transform mountainTransform = new()
            //{
            //    Position = new Vector3(10f, -15f, 30f),
            //    Rotation = Vector3.Zero,
            //    Scale = new Vector3(1f)
            //};
            //game.AddGameObjectWithMesh(mountainTransform, "MyObjects/mountains.obj", ShaderTypeEnum.SingleColorWithLight, new Vector3(0.7f, 0.4f, 0.1f));

            Transform planeTrans = new()
            {
                Position = new Vector3(-40, 20, -50),
                Rotation = new Vector3(0, float.Pi / 2f, -float.Pi / 9f),
                Scale = new Vector3(0.01f)
            };
            Transform boring = new() { Position = Vector3.Zero, Rotation = Vector3.Zero, Scale = new Vector3(0.01f) };
            plane = game.AddGameObjectWithTexture(planeTrans, "MyObjects/airplane.obj", "MyTextures/airplane.png", ShaderTypeEnum.Texture);

            //Transform lightT = new()
            //{
            //    Position = new Vector3(0, 50f, 0f),
            //    Rotation = Vector3.Zero,
            //    Scale = Vector3.One
            //};
            //game.AddGameObjectWithMesh(lightT, "MyObjects/cube.obj", ShaderTypeEnum.SingleColor, new Vector3(1f, 1f, 0f));

            Transform spyroTransform = new();
            game.AddGameObjectWithTexture(spyroTransform, "MyObjects/spyro.obj", "MyTextures/spyro.png", ShaderTypeEnum.Texture);
            game.AddGameObjectWithMesh(spyroTransform, "MyObjects/3dcircle.obj", ShaderTypeEnum.SingleColorWithLight, new(1f, 0.3f, 0.2f));


            game.StartGame();
        }

        private static void OnUpdate()
        {
            if (plane.Position.X >= 40)
            {
                forward = false;
                plane.Rotation = new Vector3(0, -float.Pi / 2f, -float.Pi / 9f);
            }

            if (plane.Position.X <= -40)
            {
                forward = true;
                plane.Rotation = new Vector3(0, float.Pi / 2f, -float.Pi / 9f);
            }

            if (forward)
            {
                plane.Position += new Vector3(0.5f, 0f, 0f);
            }
            else
            {
                plane.Position += new Vector3(-0.5f, 0f, 0f);
            }
        }
    }
}
