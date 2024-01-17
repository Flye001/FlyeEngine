using FlyeEngine.GraphicsEngine;
using OpenTK.Graphics.OpenGL4;
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
        private readonly Dictionary<string, Texture> _textureCollection;
        private readonly List<GameObject> _sceneObjects;

        public Action<float> OnUpdate;

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

        private readonly Dictionary<int, TextureUnit> _textureMappings = new()
        {
            { 0, TextureUnit.Texture0 },
            { 1, TextureUnit.Texture1 },
            { 2, TextureUnit.Texture2 },
            { 3, TextureUnit.Texture3 },
            { 4, TextureUnit.Texture4 },
            { 5, TextureUnit.Texture5 },
            { 6, TextureUnit.Texture6 },
            { 7, TextureUnit.Texture7 },
            { 8, TextureUnit.Texture8 },
            { 9, TextureUnit.Texture9 },
            { 10, TextureUnit.Texture10 },
            { 11, TextureUnit.Texture11 },
            { 12, TextureUnit.Texture12 },
            { 13, TextureUnit.Texture13 },
            { 14, TextureUnit.Texture14 },
            { 15, TextureUnit.Texture15 },
        };

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
            _sceneObjects = new List<GameObject>();
            _meshCollection = new Dictionary<string, Mesh>();
            _textureCollection = new Dictionary<string, Texture>();

            _lightPosition = Vector3.Zero;
            _graphicsEngine.SetShaderUniformVector3("lightPosition", _lightPosition);
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
            Console.WriteLine(_gameCamera.GetPosition());

            // Run game objects' update method
            foreach (var sceneObject in _sceneObjects)
            {
                sceneObject.Update(deltaTime);
            }

            OnUpdate(deltaTime);
        }

        private void RenderFrame()
        {
            foreach (var sceneObject in _sceneObjects)
            {
                _graphicsEngine.UseShader(sceneObject.ShaderType);
                _graphicsEngine.SetShaderUniformMatrix4(sceneObject.ShaderType, "modelMatrix", sceneObject.ModelMatrix);
                _graphicsEngine.SetShaderUniformMatrix3(sceneObject.ShaderType, "modelNormalMatrix", sceneObject.ModelNormalMatrix);
                
                // Set textures
                //var texs = sceneObject.GetTextures();
                //if (texs != null)
                //{
                //    foreach (var i in texs)
                //    {
                //        _textureCollection[i.Key].Use(_textureMappings[i.Value]);
                //    }
                //}
                
                sceneObject.Render();
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
                _meshCollection.Add(meshFilePath, Mesh.LoadFullWavefrontFile(meshFilePath));
            }
            var obj = new GameObject(transform.Position, transform.Rotation, transform.Scale,
                _meshCollection[meshFilePath], shaderType);
            _sceneObjects.Add(obj);

            var texs = obj.GetTextures();
            if (texs != null)
            {
                foreach (var tex in texs.Keys)
                {
                    if (!_textureCollection.ContainsKey(tex))
                    {
                        _textureCollection.Add(tex, new Texture(tex));
                    }
                }
                
            }

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
