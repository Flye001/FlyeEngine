using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace FlyeEngine.GraphicsEngine
{
    internal class GraphicsEngine : GameWindow
    {
        private Dictionary<ShaderTypeEnum, Shader> _shaders;

        private readonly Action<float> _update;
        private readonly Action _render;

        private readonly Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;

        private int count = 0;

        public GraphicsEngine(NativeWindowSettings nativeWindowSettings, Action<float> update, Action render, float aspect) : base(GameWindowSettings.Default, nativeWindowSettings)
        {
            _update = update;
            _render = render;

            _shaders = new Dictionary<ShaderTypeEnum, Shader>
            {
                {
                    ShaderTypeEnum.SingleColor,
                    new Shader("Engine/VertexShaders/MainVertexShader.vert", "Engine/FragmentShaders/SingleColorShader.frag")
                },
                {
                    ShaderTypeEnum.SingleColorWithLight,
                    new Shader("Engine/VertexShaders/MainVertexShader.vert",
                        "Engine/FragmentShaders/SingleColorWithLightShader.frag")
                },
                { 
                    ShaderTypeEnum.Texture, 
                    new Shader("Engine/VertexShaders/MainVertexShader.vert", "Engine/FragmentShaders/TextureShader.frag")
                },
                {
                    ShaderTypeEnum.TextureWithLight,
                    new Shader("Engine/VertexShaders/MainVertexShader.vert", "Engine/FragmentShaders/TextureShaderWithLight.frag")
                },
                {
                    ShaderTypeEnum.Wireframe,
                    new Shader("Engine/VertexShaders/WireframeVertexShader.vert", "Engine/FragmentShaders/WireframeShader.frag")
                }
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
                for (int i = 0; i < 16; i++)
                {
                    shader.SetIntArray("textures", i, i);
                }
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

        public void SetUniformIntArray(ShaderTypeEnum shaderType, string propertyName, int index, int value)
        {
            if (!_shaders.ContainsKey(shaderType))
            {
                throw new KeyNotFoundException($"Could not find shader for '{shaderType}'!");
            }
            _shaders[shaderType].SetIntArray(propertyName, index, value);
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
            //_update(1 / 60f);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _render();

            SwapBuffers();

            // Record?

            //Bitmap bmp = new Bitmap(ClientSize.X, ClientSize.Y);
            //BitmapData data = bmp.LockBits(new Rectangle(0, 0, ClientSize.X, ClientSize.Y),
            //    ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            //GL.ReadPixels(0, 0, ClientSize.X, ClientSize.Y, PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);

            //bmp.UnlockBits(data);
            //bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            //bmp.Save($"screenshots/{count}.png", ImageFormat.Png);
            //count++;

            // ffmpeg -framerate 60 -i %d.png -c:v libx264 -r 60 output.mp4
        }
    }
}
