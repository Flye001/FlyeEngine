using FlyeEngine;
using FlyeEngine.GraphicsEngine;
using OpenTK.Mathematics;

namespace TestGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var game = new FlyeEngine.FlyeEngine(1920, 1080, "Test Game");

            game.LightPosition = new Vector3(0, 30f, 0f);

            // Add game objects
            Transform objTrans = new()
            {
                Position = new Vector3(-5f, 0f, 15f),
                Rotation = Vector3.Zero,
                Scale = new Vector3(1f)
            };
            game.AddGameObjectWithMesh(objTrans, "MyObjects/cube.obj", ShaderTypeEnum.SingleColorWithLight, new Vector3(1f, 0.2f, 0.6f));
            
            Transform mountainTransform = new()
            {
                Position = new Vector3(10f, -15f, 30f),
                Rotation = Vector3.Zero,
                Scale = new Vector3(1f)
            };
            game.AddGameObjectWithMesh(mountainTransform, "MyObjects/mountains.obj", ShaderTypeEnum.SingleColorWithLight, new Vector3(0.7f, 0.4f, 0.1f));

            Transform planeTrans = new()
            {
                Position = new Vector3(-5, 10, 30),
                Rotation = new Vector3(-float.Pi / 6f, 0, float.Pi / 4f),
                Scale = new Vector3(0.01f)
            };
            Transform boring = new() { Position = Vector3.Zero, Rotation = Vector3.Zero, Scale = new Vector3(0.01f)};
            game.AddGameObjectWithTexture(boring, "MyObjects/airplane.obj", "MyTextures/airplane.png", ShaderTypeEnum.Texture);

            Transform lightT = new()
            {
                Position = new Vector3(0, 50f, 0f),
                Rotation = Vector3.Zero,
                Scale = Vector3.One
            };
            game.AddGameObjectWithMesh(lightT, "MyObjects/cube.obj", ShaderTypeEnum.SingleColor, new Vector3(1f, 1f, 0f));

            game.StartGame();
        }
    }
}
