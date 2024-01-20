using FlyeEngine.GraphicsEngine;
using FlyeEngine;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TestGame.MyScenes
{
    internal class TestVillage : Scene
    {
        public TestVillage(FlyeEngine.FlyeEngine engine) : base(engine)
        {
            var lightPosition = new Vector3(13, 25f, 23f);
            Engine.LightPosition = lightPosition;
            Engine.LightColor = new Vector3(1f, 1f, 1f);

            for (var i = 0; i < 3; i++)
            {
                Engine.AddGameObjectFromWavefront(new() { Scale = new(10f), Position = new(20 * i, 0, 0) }, "MyObjects\\CityKit\\building_A",
                    ShaderTypeEnum.TextureWithLight);
            }



            //Engine.AddGameObjectFromWavefront(new Transform() {Scale = new(5f)}, "MyObjects\\ModularVillage\\Prop_Barrel_1.obj",
            //    "MyObjects\\ModularVillage\\Prop_Barrel_1.mtl", ShaderTypeEnum.SingleColorWithLight);

            //Engine.AddGameObjectFromWavefront(new Transform() { Position = new(5f, 0f, 5f) },
            //    "MyObjects\\PlatformerPack\\Tower.obj", "MyObjects\\PlatformerPack\\Tower.mtl",
            //    ShaderTypeEnum.SingleColorWithLight);

            // "Sun"
            Engine.AddGameObjectFromWavefront(new Transform() { Position = lightPosition, Scale = new(0.1f) },
                "MyObjects\\cube\\cube", ShaderTypeEnum.SingleColor);
        }

        public override void OnUpdate(float deltaTime, KeyboardState kb)
        {
            //
        }
    }
}
