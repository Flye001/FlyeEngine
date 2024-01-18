using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TestGame
{
    internal abstract class Scene
    {
        internal readonly FlyeEngine.FlyeEngine Engine;

        protected Scene(FlyeEngine.FlyeEngine engine)
        {
            Engine = engine;

            Engine.OnUpdate += OnUpdate;
        }

        public void StartGame()
        {
            Engine.StartGame();
        }

        public abstract void OnUpdate(float deltaTime, KeyboardState kb);


    }
}
