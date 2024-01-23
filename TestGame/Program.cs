using FlyeEngine;
using FlyeEngine.GraphicsEngine;
using OpenTK.Mathematics;
using TestGame.MyScenes;

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

            //var myCamera = new StaticCamera(new Vector3(0f, 20f, -25f), Vector3.Zero);
            //var game = new FlyeEngine.FlyeEngine(1920, 1080, "Test Game", myCamera);
            //var scene = new FallingCubes(game);
            //scene.StartGame();

            var myCamera = new StaticCamera(new Vector3(0f, 50f, 0f), new Vector3(50, 0, 50));
            var game = new FlyeEngine.FlyeEngine(1920, 1080, "Test Game");
            var scene = new TestVillage(game);
            scene.StartGame();
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

    }
}
