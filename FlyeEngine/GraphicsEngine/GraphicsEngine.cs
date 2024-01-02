using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace FlyeEngine.GraphicsEngine
{
    internal class GraphicsEngine : GameWindow
    {
        private readonly Shader _basicColorShader;

        private readonly Action<float> _update;
        private readonly Action _render;

        private readonly Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        public GraphicsEngine(NativeWindowSettings nativeWindowSettings, Action<float> update, Action render, float aspect) : base(GameWindowSettings.Default, nativeWindowSettings)
        {
            _update = update;
            _render = render;
            _basicColorShader = new Shader("Shaders/SingleColorShader.vert", "Shaders/SingleColorShader.frag");

            // Temporary
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(float.Pi / 2f, aspect, 0.1f, 1000f);
            _viewMatrix = Matrix4.Identity;
            var modelMatrix = Matrix4.CreateRotationX(float.Pi / 6f) * Matrix4.CreateTranslation(0f, 0f, -5f);
            _basicColorShader.SetMatrix4("modelMatrix", ref modelMatrix);
            _basicColorShader.SetMatrix4("viewMatrix", ref _viewMatrix);
            _basicColorShader.SetMatrix4("projectionMatrix", ref _projectionMatrix);
        }

        public void UpdateViewMatrix(Matrix4 matrix)
        {
            _viewMatrix = matrix;
            _basicColorShader.SetMatrix4("viewMatrix", ref _viewMatrix);
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

            _update((float)args.Time);
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
