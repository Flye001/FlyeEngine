using FlyeEngine.GraphicsEngine;
using OpenTK.Windowing.Desktop;

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
        private GraphicsEngine.GraphicsEngine _graphicsEngine;

        private Dictionary<string, Mesh> _meshCollection;
        private List<GameObject> _sceneObjects;

        /// <summary>
        /// Create new game instance
        /// </summary>
        /// <param name="targetWidth">Target game window width</param>
        /// <param name="targetHeight">Target game window height</param>
        /// <param name="name">Game window name</param>
        public FlyeEngine(int targetWidth, int targetHeight, string name)
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
            _graphicsEngine = new(windowSettings);
            _sceneObjects = new();
            _meshCollection = new();
        }

        public void AddGameObject(Transform transform)
        {
            _sceneObjects.Add(new GameObject(transform.Position, transform.Rotation, transform.Scale));
        }

        public void AddGameObjectWithMesh(Transform transform, string meshFilePath)
        {
            if (! _meshCollection.ContainsKey(meshFilePath))
            {
                _meshCollection.Add(meshFilePath, new Mesh(meshFilePath));
            }
            _sceneObjects.Add(new GameObject(transform.Position, transform.Rotation, transform.Scale, _meshCollection[meshFilePath]));
        }

        public void StartGame()
        {
            _graphicsEngine.Run();
        }
    }
}
