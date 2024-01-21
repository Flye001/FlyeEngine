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

        /// <summary>
        /// Manages the game's physics
        /// </summary>
        private readonly PhysicsEngine.PhysicsEngine _physicsEngine;

        private readonly ICamera _gameCamera;
        private readonly Dictionary<string, Mesh> _meshCollection;
        private readonly List<GameObject> _sceneObjects;

        private bool _renderColliderWireframes = true;
        private bool _printCameraPosition = false;

        public Action<float, KeyboardState> OnUpdate;

        private Vector3 _lightPosition;
        public Vector3 LightPosition
        {
            get => _lightPosition;
            set
            {
                _lightPosition = value;
                _graphicsEngine.SetShaderUniformVector3("lightPosition", _lightPosition);
            }
        }

        private Vector3 _lightColor;
        public Vector3 LightColor
        {
            get => _lightColor;
            set
            {
                _lightColor = value;
                _graphicsEngine.SetShaderUniformVector3("lightColor", _lightColor);
            }
        }

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
            _physicsEngine = new PhysicsEngine.PhysicsEngine();
            if (camera == null)
            {
                _gameCamera = new BasicCamera(_screen_aspect_ration, Vector3.Zero);
                _graphicsEngine.CursorState = CursorState.Grabbed;
            }
            else
            {
                _gameCamera = camera;
            }
            _sceneObjects = new List<GameObject>();
            _meshCollection = new Dictionary<string, Mesh>();

            _lightPosition = Vector3.Zero;
            _lightColor = Vector3.One;
            _graphicsEngine.SetShaderUniformVector3("lightPosition", _lightPosition);
            _graphicsEngine.SetShaderUniformVector3("lightColor", _lightColor);
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
                _graphicsEngine.SetShaderUniformVector3("viewPosition", _gameCamera.Position);
            }

            if (_printCameraPosition)
            {
                Console.WriteLine(_gameCamera.GetPosition());
            }
            

            _physicsEngine.CheckCollisions(_sceneObjects.Where(x => x.RigidBody != null), _sceneObjects.Where(x => x.BoxCollider != null));

            // Run game objects' update method
            foreach (var sceneObject in _sceneObjects)
            {
                sceneObject.Update(deltaTime);
            }

            OnUpdate(deltaTime, keyboardState);
        }

        private void RenderFrame()
        {
            foreach (var sceneObject in _sceneObjects)
            {
                _graphicsEngine.UseShader(sceneObject.ShaderType);
                _graphicsEngine.SetShaderUniformMatrix4(sceneObject.ShaderType, "modelMatrix", sceneObject.ModelMatrix);
                _graphicsEngine.SetShaderUniformMatrix3(sceneObject.ShaderType, "modelNormalMatrix",
                    sceneObject.ModelNormalMatrix);

                sceneObject.Render();

                if (sceneObject.BoxCollider != null && _renderColliderWireframes)
                {
                    _graphicsEngine.UseShader(ShaderTypeEnum.Wireframe);
                    _graphicsEngine.SetShaderUniformMatrix4(ShaderTypeEnum.Wireframe, "modelMatrix",
                        sceneObject.ColliderModelMatrix ?? Matrix4.Identity);

                    sceneObject.RenderBoxCollider();
                }
            }
        }

        public GameObject AddGameObject(Transform transform)
        {
            var obj = new GameObject(transform.Position, transform.Rotation, transform.Scale);
            _sceneObjects.Add(obj);
            return obj;
        }

        public GameObject AddGameObjectFromWavefront(Transform transform, string meshFilePath,
            ShaderTypeEnum shaderType)
        {
            if (!_meshCollection.ContainsKey(meshFilePath))
            {
                _meshCollection.Add(meshFilePath, Mesh.LoadFromWavefront(meshFilePath));
            }
            var obj = new GameObject(transform.Position, transform.Rotation, transform.Scale,
                _meshCollection[meshFilePath], shaderType);
            _sceneObjects.Add(obj);
            
            return obj;
        }

        public GameObject AddGameObjectFromWavefront(Transform transform, string objFilePath, string mtlFilePath,
            ShaderTypeEnum shaderType)
        {
            if (!_meshCollection.ContainsKey(objFilePath))
            {
                _meshCollection.Add(objFilePath, Mesh.LoadFullWavefrontFile(objFilePath, mtlFilePath));
            }
            var obj = new GameObject(transform.Position, transform.Rotation, transform.Scale,
                _meshCollection[objFilePath], shaderType);
            _sceneObjects.Add(obj);

            return obj;
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
