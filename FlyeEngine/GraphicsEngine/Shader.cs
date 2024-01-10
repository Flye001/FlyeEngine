using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FlyeEngine.GraphicsEngine
{
    public class Shader : IDisposable
    {
        private readonly int Handle;
        private bool _disposedValue = false;

        public Shader(string vertexPath, string fragmentPath)
        {
            var vertexShaderSource = File.ReadAllText(vertexPath);
            var fragmentShaderSource = File.ReadAllText(fragmentPath);

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);

            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                // Compilation Error
                var infoLog = GL.GetShaderInfoLog(vertexShader);
                throw new Exception($"Failed to compile shader '{vertexPath}':\n\n{infoLog}");
            }

            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                // Compilation Error
                var infoLog = GL.GetShaderInfoLog(fragmentShader);
                throw new Exception($"Failed to compile shader '{fragmentPath}':\n\n{infoLog}");
            }

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                // Link Error
                var infoLog = GL.GetProgramInfoLog(Handle);
                throw new Exception($"Failed to link shader:\n\n{infoLog}");
            }

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public void SetMatrix4(string name, ref Matrix4 matrix)
        {
            GL.UseProgram(Handle);
            var location = GL.GetUniformLocation(Handle, name);
            GL.UniformMatrix4(location, true, ref matrix);
        }

        public void SetMatrix3(string name, ref Matrix3 matrix)
        {
            GL.UseProgram(Handle);
            var location = GL.GetUniformLocation(Handle, name);
            GL.UniformMatrix3(location, true, ref matrix);
        }

        public void SetVector3(string name, ref Vector3 vector)
        {
            GL.UseProgram(Handle);
            var location = GL.GetUniformLocation(Handle, name);
            GL.Uniform3(location, ref vector);
        }

        public void SetInt(string name, int i)
        {
            GL.UseProgram(Handle);
            var location = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(location, i);
        }

        public void SetIntArray(string name, int index, int i)
        {
            GL.UseProgram(Handle);
            var location = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(location + index, i);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                GL.DeleteProgram(Handle);
                _disposedValue = true;
            }
        }

        ~Shader()
        {
            if (_disposedValue == false)
            {
                Console.WriteLine("GPU Resource Leak! Did you forget to call dispose?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
