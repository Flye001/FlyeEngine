using FlyeEngine.GraphicsEngine;
using FlyeEngine;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TestGame.MyScenes
{
    internal class TestVillage : Scene
    {
        private const float PoliceSpeed = 15f;
        private readonly GameObject _policeCar;
        public TestVillage(FlyeEngine.FlyeEngine engine) : base(engine)
        {
            var lightPosition = new Vector3(-23, 34, 18);
            Engine.LightPosition = lightPosition;
            Engine.LightColor = new Vector3(1f, 1f, 1f);

            var gapSize = 20;
            Dictionary<string, string> buildings = new()
            {
                {
                    "b", "MyObjects\\CityKit\\base"
                },
                {
                    "b1", "MyObjects\\CityKit\\building_A"
                },
                {
                    "b2", "MyObjects\\CityKit\\building_B"
                },
                {
                    "b3", "MyObjects\\CityKit\\building_C"
                },
                {
                    "b4", "MyObjects\\CityKit\\building_D"
                },
                {
                    "b5", "MyObjects\\CityKit\\building_E"
                },
                {
                    "b6", "MyObjects\\CityKit\\building_F"
                },
                {
                    "b7", "MyObjects\\CityKit\\building_G"
                },
                {
                    "r", "MyObjects\\CityKit\\road_straight"
                },
                {
                    "rc", "MyObjects\\CityKit\\road_corner"
                },
                {
                    "rcc", "MyObjects\\CityKit\\road_corner_curved"
                },
                {
                    "policeCar", "MyObjects\\CityKit\\car_police_full"
                },
                {
                    "hatchback", "MyObjects\\CityKit\\car_hatchback_full"
                },
                {
                    "stationwagon", "MyObjects\\CityKit\\car_stationwagon_full"
                },
                {
                    "sedan", "MyObjects\\CityKit\\car_sedan_full"
                }
            };
            string[,] layout = { { "b", "b1", "b1", "b1", "b" }, { "rc", "r", "r", "r", "rc" }, { "r", "b2", "b3", "b4", "r" }, {"r", "b5", "b6", "b7", "r"}, {"rc", "r", "r", "r", "rc"} };
            float[,] rotations = { { 0, 0, 0, 0, 0 }, { 0, float.Pi / 2f, float.Pi / 2f, float.Pi / 2f, -float.Pi / 2f }, { 0, float.Pi, float.Pi, float.Pi, 0 }, { 0, 0, 0, 0, 0 }, { float.Pi / 2f, float.Pi / 2f, float.Pi / 2f, float.Pi / 2f, float.Pi } };

            var width = layout.GetLength(1);
            var height = layout.GetLength(0);

            for (var w = 0; w < width; w++)
            {
                for (var h = 0; h < height; h++)
                {
                    var o = Engine.AddGameObjectFromWavefront(new() { Scale = new(10f), Position = new(gapSize * w, 0, gapSize * h), Rotation = new(0, rotations[h, w], 0) }, buildings[layout[h, w]],
                        ShaderTypeEnum.TextureWithLight);
                    //o.AddBoxCollider(new Vector3(20, 1, 20), new Vector3(0, 0.5f, 0));
                }
            }

            var ground = Engine.AddGameObject(new Transform());
            ground.AddBoxCollider(new Vector3(100, 1f, 100), new Vector3(40, 0.5f, 40));

            _policeCar = Engine.AddGameObjectFromWavefront(new Transform() {Scale = new(10), Position = new(0, 3f, 20)}, buildings["policeCar"], ShaderTypeEnum.TextureWithLight);
            _policeCar.AddBoxCollider(new Vector3(4,4,9), new Vector3(0, 1.5f, 0));
            _policeCar.AddRigidBody(5);

            var car1 = Engine.AddGameObjectFromWavefront(new Transform() {Scale = new(10), Position = new(0, 1.4f, 35)}, buildings["hatchback"], ShaderTypeEnum.TextureWithLight);
            car1.AddBoxCollider(new Vector3(4, 4, 8), new Vector3(0, 1.5f, 0));
            car1.AddRigidBody(5);

            var car2 = Engine.AddGameObjectFromWavefront(new Transform() {Scale = new(10), Position = new(0, 1.4f, 50)}, buildings["stationwagon"], ShaderTypeEnum.TextureWithLight);
            car2.AddBoxCollider(new Vector3(4, 4, 9), new Vector3(0, 1.5f, 0));
            car2.AddRigidBody(5);

            var car3 = Engine.AddGameObjectFromWavefront(new Transform() {Scale = new(10), Position = new(0, 1.4f, 65)}, buildings["sedan"], ShaderTypeEnum.TextureWithLight);
            car3.AddBoxCollider(new Vector3(4, 4, 9), new Vector3(0, 1.5f, 0));
            car3.AddRigidBody(5);


            //Engine.AddGameObjectFromWavefront(new Transform() { Position = new(5f, 0f, 5f) },
            //    "MyObjects\\PlatformerPack\\Tower.obj", "MyObjects\\PlatformerPack\\Tower.mtl",
            //    ShaderTypeEnum.SingleColorWithLight);

            // "Sun"
            Engine.AddGameObjectFromWavefront(new Transform() { Position = lightPosition, Scale = new(0.1f) },
                "MyObjects\\cube\\cube", ShaderTypeEnum.SingleColor);
        }

        public override void OnUpdate(float deltaTime, KeyboardState kb)
        {
            if (kb.IsKeyDown(Keys.W))
            {
                _policeCar.RigidBody.AddForce(new Vector3(0, 0, PoliceSpeed * deltaTime));
            }
            if (kb.IsKeyDown(Keys.S))
            {
                _policeCar.RigidBody.AddForce(new Vector3(0, 0, -PoliceSpeed * deltaTime));
            }
            if (kb.IsKeyDown(Keys.A))
            {
                _policeCar.RigidBody.AddForce(new Vector3(PoliceSpeed * deltaTime, 0, 0));
            }
            if (kb.IsKeyDown(Keys.D))
            {
                _policeCar.RigidBody.AddForce(new Vector3(-PoliceSpeed * deltaTime, 0, 0));
            }
            if (kb.IsKeyReleased(Keys.Space))
            {
                _policeCar.RigidBody.AddForce(new Vector3(0, 5, 0));
            }
        }
    }
}
