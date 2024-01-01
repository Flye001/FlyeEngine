using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;

namespace FlyeEngine
{
    internal class GraphicsEngine : GameWindow
    {
        private readonly Shader _basicColorShader;
        public GraphicsEngine(NativeWindowSettings nativeWindowSettings) : base(GameWindowSettings.Default, nativeWindowSettings)
        {
            _basicColorShader = new Shader("Shaders/SingleColorShader.vert", "Shaders/SingleColorShader.frag");
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Enable(EnableCap.DepthTest);
        }
    }
}
