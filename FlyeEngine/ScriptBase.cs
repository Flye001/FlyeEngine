namespace FlyeEngine
{
    public abstract class Script
    {
        public readonly GameObject Self;

        public Script(GameObject self)
        {
            Self = self;
        }

        public abstract void OnUpdate(float deltaTime);
    }
}
