using FlyeEngine.GraphicsEngine;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FlyeEngine
{
    public class FlyeEngine
    {
        /// <summary>
        /// Game window width
        /// </summary>
        private int _screen_width;
        /// <summary>
        /// Game window height
        /// </summary>
        private int _screen_height;
        /// <summary>
        /// Returns the current aspect ratio of the game window
        /// </summary>
        private float _screen_aspect_ration => _screen_width / (float)_screen_height;
        /// <summary>
        /// Game window name
        /// </summary>
        private string _screen_name;

        /// <summary>
        /// Manages the OpenTK window
        /// </summary>
        private readonly GraphicsEngine.GraphicsEngine _graphicsEngine;

        private readonly ICamera _gameCamera;
        private readonly Dictionary<string, Mesh> _meshCollection;
        private readonly List<GameObject> _sceneObjects;

        /// <summary>
        /// Create new game instance
        /// </summary>
        /// <param name="targetWidth">Target game window width</param>
        /// <param name="targetHeight">Target game window height</param>
        /// <param name="name">Game window name</param>
        public FlyeEngine(int targetWidth, int targetHeight, string name, ICamera camera = null)
        {
            // TODO: Add logic to check if target screen size is larger than current display size
            _screen_width = targetWidth;
            _screen_height = targetHeight;
            _screen_name = name;

            NativeWindowSettings windowSettings = new()
            {
                ClientSize = (_screen_width, _screen_height),
                MaximumClientSize = (_screen_width, _screen_height),
                MinimumClientSize = (_screen_width, _screen_height),
                Title = _screen_name
            };
            _graphicsEngine = new(windowSettings, UpdateFrame, RenderFrame, _screen_aspect_ration);
            if (camera == null)
            {
                _gameCamera = new BasicCamera(_screen_aspect_ration, Vector3.Zero);
                _graphicsEngine.CursorState = CursorState.Grabbed;
            }
            _sceneObjects = new();
            _meshCollection = new();
        }

        private void UpdateFrame(float deltaTime)
        {
            var keyboardState = _graphicsEngine.KeyboardState;
            var mouseState = _graphicsEngine.MouseState;

            // Handle Input
            if (_graphicsEngine.IsFocused)
            {
                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    StopGame();
                }

                _gameCamera.HandleInput(keyboardState, mouseState, deltaTime);
                _graphicsEngine.UpdateViewMatrix(_gameCamera.GetViewMatrix());
            }
        }

        private void RenderFrame()
        {
            foreach (var sceneObject in _sceneObjects)
            {
                _graphicsEngine.UseShader(sceneObject.ShaderType);
                _graphicsEngine.SetShaderUniformVector3(sceneObject.ShaderType, "color", sceneObject.Color);
                _graphicsEngine.SetShaderUniformMatrix4(sceneObject.ShaderType, "modelMatrix", sceneObject.ModelMatrix);
                sceneObject.Render();
            }
        }

        public void AddGameObject(Transform transform)
        {
            _sceneObjects.Add(new GameObject(transform.Position, transform.Rotation, transform.Scale));
        }

        public void AddGameObjectWithMesh(Transform transform, string meshFilePath, ShaderTypeEnum shaderType, Vector3 objectColor)
        {
            if (! _meshCollection.ContainsKey(meshFilePath))
            {
                _meshCollection.Add(meshFilePath, new Mesh(meshFilePath));
            }
            _sceneObjects.Add(new GameObject(transform.Position, transform.Rotation, transform.Scale, _meshCollection[meshFilePath], shaderType, objectColor));
        }

        public void StartGame()
        {
            _graphicsEngine.Run();
        }

        public void StopGame()
        {
            _graphicsEngine.Close();
        }
    }
}
