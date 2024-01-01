using FlyeEngine;
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
                Position = Vector3.Zero,
                Rotation = Vector3.Zero,
                Scale = Vector3.One
            };
            game.AddGameObjectWithMesh(objTrans, "MyObjects/teapot.obj");
            
            game.StartGame();
        }
    }
}
