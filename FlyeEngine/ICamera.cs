using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FlyeEngine
{
    public interface ICamera
    {
        public void HandleInput(KeyboardState keyboardState, MouseState mouseState, float deltaTime);
        public Matrix4 GetViewMatrix();
        public string GetPosition();
    }

    public class StaticCamera : ICamera
    {
        private readonly Vector3 _position;
        private readonly Vector3 _up = Vector3.UnitY;
        private readonly Vector3 _lookDirection;

        public StaticCamera(Vector3 position, Vector3 lookDirection)
        {
            _position = position;
            _lookDirection = lookDirection;
        }

        public void HandleInput(KeyboardState keyboardState, MouseState mouseState, float deltaTime)
        {
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(_position, _lookDirection, _up);
        }

        public string GetPosition()
        {
            return $"Position: {_position.X} {_position.Y} {_position.Z}";
        }
    }

    public class BasicCamera : ICamera
    {
        private Vector3 _position;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _front = -Vector3.UnitZ;
        private Vector3 _right = Vector3.UnitX;
        private float _pitch;
        private float _yaw = float.Pi / 2f;
        private float _aspectRatio;
        private float speed = 10f;
        private float sensitivity = 0.2f * (float.Pi / 180f);
        private bool _firstMove = true;
        private Vector2 _lastMousePos;

        public BasicCamera(float aspectRatio, Vector3 position)
        {
            _aspectRatio = aspectRatio;
            _position = position;
        }

        private float Pitch
        {
            get => _pitch;
            set
            {
                var angle = float.Clamp(value, -89 * (float.Pi / 180f), 89 * (float.Pi / 180f));
                _pitch = angle;
                UpdateVectors();
            }
        }
        private float Yaw
        {
            get => _yaw;
            set
            {
                _yaw = value;
                UpdateVectors();
            }
        }

        public void HandleInput(KeyboardState keyboardState, MouseState mouseState, float deltaTime)
        {
            if (keyboardState.IsKeyDown(Keys.W))
            {
                _position += _front * speed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                _position -= _front * speed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                _position -= _right * speed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                _position += _right * speed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                _position += _up * speed * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.LeftShift))
            {
                _position -= _up * speed * deltaTime;
            }

            if (_firstMove)
            {
                _lastMousePos = new Vector2(mouseState.X, mouseState.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouseState.X - _lastMousePos.X;
                var deltaY = mouseState.Y - _lastMousePos.Y;
                _lastMousePos = new Vector2(mouseState.X, mouseState.Y);

                Yaw += deltaX * sensitivity;
                Pitch -= deltaY * sensitivity;
            }
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(_position, _position + _front, _up);
        }

        private void UpdateVectors()
        {
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);
            _front = Vector3.Normalize(_front);
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }

        public string GetPosition()
        {
            return $"Position: {_position.X} {_position.Y} {_position.Z}";
        }
    }
}
