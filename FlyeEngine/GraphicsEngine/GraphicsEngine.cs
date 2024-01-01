using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace FlyeEngine.GraphicsEngine
{
    internal class GraphicsEngine : GameWindow
    {
        private readonly Shader _basicColorShader;

        private readonly Action _update;
        private readonly Action _render;

        public GraphicsEngine(NativeWindowSettings nativeWindowSettings, Action update, Action render, float aspect) : base(GameWindowSettings.Default, nativeWindowSettings)
        {
            _update = update;
            _render = render;
            _basicColorShader = new Shader("Shaders/SingleColorShader.vert", "Shaders/SingleColorShader.frag");

            // Temporary
            var projMatrix = Matrix4.CreatePerspectiveFieldOfView(float.Pi / 2f, aspect, 0.1f, 1000f);
            var viewMatrix = Matrix4.Identity;
            var modelMatrix = Matrix4.CreateRotationX(float.Pi / 6f) * Matrix4.CreateTranslation(0f, 0f, -5f);
            _basicColorShader.SetMatrix4("modelMatrix", ref modelMatrix);
            _basicColorShader.SetMatrix4("viewMatrix", ref viewMatrix);
            _basicColorShader.SetMatrix4("projectionMatrix", ref projMatrix);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0f, 0f, 0f, 1f);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            _update();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Temporary
            _basicColorShader.Use();

            _render();

            SwapBuffers();
        }
    }
}
