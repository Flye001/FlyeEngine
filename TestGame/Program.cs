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

            // Add game objects
            Transform objTrans = new()
            {
                Position = new Vector3(0f, -5f, 5f),
                Rotation = Vector3.Zero,
                Scale = new Vector3(1f)
            };
            game.AddGameObjectWithMesh(objTrans, "MyObjects/teapot.obj", ShaderTypeEnum.SingleColor, new Vector3(1f, 0.2f, 0.6f));
            Transform mountainTransform = new()
            {
                Position = new Vector3(0f, 0f, 5f),
                Rotation = new Vector3(float.Pi / 6f, 0f, 0f),
                Scale = new Vector3(5f, 1f, 1f)
            };
            game.AddGameObjectWithMesh(mountainTransform, "MyObjects/cube.obj", ShaderTypeEnum.SingleColor, new Vector3(1f, 0.2f, 0f));


            game.StartGame();
        }
    }
}
