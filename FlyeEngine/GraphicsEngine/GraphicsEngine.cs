using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace FlyeEngine.GraphicsEngine
{
    internal class GraphicsEngine : GameWindow
    {
        private Dictionary<ShaderTypeEnum, Shader> _shaders;

        private readonly Action<float> _update;
        private readonly Action _render;

        private readonly Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        public GraphicsEngine(NativeWindowSettings nativeWindowSettings, Action<float> update, Action render, float aspect) : base(GameWindowSettings.Default, nativeWindowSettings)
        {
            _update = update;
            _render = render;

            _shaders = new Dictionary<ShaderTypeEnum, Shader>
            {
                {
                    ShaderTypeEnum.SingleColor,
                    new Shader("Engine/Shaders/SingleColorShader.vert", "Engine/Shaders/SingleColorShader.frag")
                },
                {
                    ShaderTypeEnum.SingleColorWithLight,
                    new Shader("Engine/Shaders/SingleColorShader.vert",
                        "Engine/Shaders/SingleColorWithLightShader.frag")
                },
                { ShaderTypeEnum.Texture, new Shader("Engine/Shaders/SingleColorShader.vert", "Engine/Shaders/TextureShader.frag") }
            };

            // Temporary
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(float.Pi / 2f, aspect, 0.1f, 1000f);
            _viewMatrix = Matrix4.Identity;
            //var modelMatrix = Matrix4.CreateRotationX(float.Pi / 6f) * Matrix4.CreateTranslation(0f, 0f, -5f);
            foreach (var shader in _shaders.Values)
            {
                //shader.SetMatrix4("modelMatrix", ref modelMatrix);
                shader.SetMatrix4("viewMatrix", ref _viewMatrix);
                shader.SetMatrix4("projectionMatrix", ref _projectionMatrix);
            }
        }

        public void UpdateViewMatrix(Matrix4 matrix)
        {
            _viewMatrix = matrix;
            foreach (var shader in _shaders.Values)
            {
                shader.SetMatrix4("viewMatrix", ref _viewMatrix);
            }
        }

        public void UseShader(ShaderTypeEnum type)
        {
            if (!_shaders.ContainsKey(type))
            {
                throw new KeyNotFoundException($"Could not find shader for '{type}'!");
            }
            _shaders[type].Use();
        }

        public void SetShaderUniformVector3(ShaderTypeEnum shaderType, string propertyName, Vector3 propertyValue)
        {
            if (!_shaders.ContainsKey(shaderType))
            {
                throw new KeyNotFoundException($"Could not find shader for '{shaderType}'!");
            }
            _shaders[shaderType].SetVector3(propertyName, ref propertyValue);
        }

        public void SetShaderUniformVector3(string propertyName, Vector3 propertyValue)
        {
            foreach (var shader in _shaders.Values)
            {
                shader.SetVector3(propertyName, ref propertyValue);
            }
        }

        public void SetShaderUniformMatrix4(ShaderTypeEnum shaderType, string propertyName, Matrix4 propertyValue)
        {
            if (!_shaders.ContainsKey(shaderType))
            {
                throw new KeyNotFoundException($"Could not find shader for '{shaderType}'!");
            }
            _shaders[shaderType].SetMatrix4(propertyName, ref propertyValue);
        }

        public void SetShaderUniformMatrix3(ShaderTypeEnum shaderType, string propertyName, Matrix3 propertyValue)
        {
            if (!_shaders.ContainsKey(shaderType))
            {
                throw new KeyNotFoundException($"Could not find shader for '{shaderType}'!");
            }
            _shaders[shaderType].SetMatrix3(propertyName, ref propertyValue);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0f, 0f, 0f, 1f);
            GL.Enable(EnableCap.DepthTest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
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

            _render();

            SwapBuffers();
        }
    }
}
