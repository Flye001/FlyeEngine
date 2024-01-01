using OpenTK.Windowing.Desktop;

namespace FlyeEngine
{
    internal class GraphicsEngine : GameWindow
    {
        public GraphicsEngine(NativeWindowSettings nativeWindowSettings) : base(GameWindowSettings.Default, nativeWindowSettings)
        {
        }
    }
}
