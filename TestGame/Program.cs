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

            game.LightPosition = new Vector3(0, 30.0f, 0f);

            // Add game objects
            Transform objTrans = new()
            {
                Position = new Vector3(-5f, 0f, 15f),
                Rotation = Vector3.Zero,
                Scale = new Vector3(1f)
            };
            game.AddGameObjectWithMesh(objTrans, "MyObjects/cube.obj", ShaderTypeEnum.SingleColor, new Vector3(1f, 0.2f, 0.6f));
            
            Transform mountainTransform = new()
            {
                Position = new Vector3(0f, -15f, 5f),
                Rotation = Vector3.Zero,
                Scale = new Vector3(1f)
            };
            game.AddGameObjectWithMesh(mountainTransform, "MyObjects/mountains.obj", ShaderTypeEnum.SingleColorWithLight, new Vector3(0.7f, 0.4f, 0.1f));

            Transform planeTrans = new()
            {
                Position = new Vector3(250, 250, 0),
                Rotation = Vector3.Zero,
                Scale = new Vector3(0.01f)
            };
            game.AddGameObjectWithMesh(planeTrans, "MyObjects/airplane.obj", ShaderTypeEnum.SingleColorWithLight, new Vector3(1f));



            game.StartGame();
        }
    }
}
