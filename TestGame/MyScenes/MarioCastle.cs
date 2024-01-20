using FlyeEngine.GraphicsEngine;
using FlyeEngine;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TestGame.MyScenes
{
    internal class MarioCastle : Scene
    {
        public MarioCastle(FlyeEngine.FlyeEngine engine) : base(engine)
        {
            var lightPosition = new Vector3(-18, 14, 0);
            Engine.LightPosition = lightPosition;
            Engine.LightColor = new Vector3(1f, 1f, 1f);

            Engine.AddGameObjectFromWavefront(new() {Scale = new(10f)}, "MyObjects\\peach\\peach", ShaderTypeEnum.Texture);

            // "Sun"
            Engine.AddGameObjectFromWavefront(new Transform() { Position = lightPosition, Scale = new(0.1f) },
                "MyObjects\\cube\\cube", ShaderTypeEnum.SingleColor);
        }

        public override void OnUpdate(float deltaTime, KeyboardState kb)
        {
            //throw new NotImplementedException();
        }
    }
}
