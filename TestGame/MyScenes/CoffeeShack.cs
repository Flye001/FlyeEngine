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
            var lightPosition = new Vector3(17f, 17f, 22f);
            Engine.LightPosition = lightPosition;
            Engine.LightColor = new Vector3(1f, 1f, 1f);

            Engine.AddGameObjectFromWavefront(new Transform() {Scale = new(2f)}, "MyObjects\\coffeeHouse", ShaderTypeEnum.SingleColorWithLight);

            Engine.AddGameObjectFromWavefront(new Transform() { Position = new(-4, 7, 17) }, "MyObjects\\sphere",
                ShaderTypeEnum.SingleColorWithLight);

            // "Sun"
            Engine.AddGameObjectFromWavefront(new Transform() { Position = lightPosition, Scale = new(0.1f) },
                "MyObjects\\cube", ShaderTypeEnum.SingleColor);
        }

        public override void OnUpdate(float deltaTime, KeyboardState kb)
        {
            //
        }
    }
}
