using OpenTK.Mathematics;

namespace FlyeEngine.GraphicsEngine
{
    internal class Mtl
    {
        /// <summary>
        /// Material Name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Specular Illumination Value
        /// </summary>
        public float Ns { get; set; }
        /// <summary>
        /// Ambient Light Values
        /// </summary>
        public Vector3 Ka { get; set; }
        /// <summary>
        /// Diffuse Light Values
        /// </summary>
        public Vector3 Kd { get; set; }
        /// <summary>
        /// Specular Reflectivity Light Values
        /// </summary>
        public Vector3 Ks { get; set; }

        /// <summary>
        /// Illumination Model
        /// </summary>
        public int Illumination { get; set; }

        public Mtl()
        {
        }

        public Mtl(string name)
        {
            Name = name;
        }
    }
}
