using FlyeEngine;
using FlyeEngine.GraphicsEngine;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TestGame.MyScenes
{
    internal class CoffeeShack : Scene
    {
        public CoffeeShack(FlyeEngine.FlyeEngine engine) : base(engine)
        {
            Engine.LightPosition = new Vector3(-2.6f, 6.3f, 3.2f);

            Engine.AddGameObjectFromWavefront(new Transform(), "MyObjects\\coffeeHouse", ShaderTypeEnum.SingleColorWithLight);
        }

        public override void OnUpdate(float deltaTime, KeyboardState kb)
        {
            //
        }
    }
}
