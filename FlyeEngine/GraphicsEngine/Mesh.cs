using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FlyeEngine.GraphicsEngine
{
    /// <summary>
    /// Object that creates / stores a vertex buffer of all vertices for a given model
    /// </summary>
    public class Mesh
    {
        private readonly int _vertexArrayObject;
        private readonly int _vertexBufferObject;
        private readonly int _numOfVertices;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectFilePath">Do not include file extension</param>
        /// <returns></returns>
        public static Mesh LoadFullWavefrontFile(string objectFilePath)
        {
            var objFile = objectFilePath + ".obj";
            var mtlFile = objectFilePath + ".mtl";
            if (!File.Exists(objFile) || !File.Exists(mtlFile))
            {
                throw new FileNotFoundException($"Could not find an object file at '{objectFilePath}'!");
            }

            // Load materials
            Dictionary<string, Mtl> materials = new();
            Mtl currentMaterial = new();
            foreach (var line in File.ReadAllLines(mtlFile))
            {
                if (string.IsNullOrEmpty(line)) continue;
                var splitLine = line.Split(' ');
                switch (splitLine[0])
                {
                    case "newmtl":
                        if (!string.IsNullOrEmpty(currentMaterial.Name))
                        {
                            materials.Add(currentMaterial.Name, currentMaterial);
                        }

                        currentMaterial = new Mtl(splitLine[1]);
                        break;
                    case "Ns":
                        currentMaterial.Ns = float.Parse(splitLine[1]);
                        break;
                    case "Ka":
                        currentMaterial.Ka = new Vector3(float.Parse(splitLine[1]),  float.Parse(splitLine[2]), float.Parse(splitLine[3]));
                        break;
                    case "Kd":
                        currentMaterial.Kd = new Vector3(float.Parse(splitLine[1]), float.Parse(splitLine[2]), float.Parse(splitLine[3]));
                        break;
                    case "Ks":
                        currentMaterial.Ks = new Vector3(float.Parse(splitLine[1]), float.Parse(splitLine[2]), float.Parse(splitLine[3]));
                        break;
                    case "illum":
                        currentMaterial.Illumination = int.Parse(splitLine[1]);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(currentMaterial.Name))
            {
                materials.Add(currentMaterial.Name, currentMaterial);
            }

            // Load vertices
            List<float> gpuVertices = new();
            List<Vector3> allVertices = new();
            List<Vector2> allTextures = new();
            foreach (var line in File.ReadAllLines(objFile))
            {
                if (string.IsNullOrEmpty(line)) continue;
                var splitLine = line.Split(' ');
                switch (splitLine[0])
                {
                    case "usemtl":
                        currentMaterial = materials[splitLine[1]];
                        break;
                    case "v":
                        allVertices.Add(new Vector3(float.Parse(splitLine[1]), float.Parse(splitLine[2]), float.Parse(splitLine[3])));
                        break;
                    case "vt":
                        allTextures.Add(new Vector2(float.Parse(splitLine[1]), float.Parse(splitLine[2])));
                        break;
                    case "f":
                        // face has texture data
                        if (splitLine[1].Contains('/'))
                        {
                            var tempVertices = new List<Vector3>(3);
                            var tempTextures = new List<Vector2>(3);

                            for (var i = 1; i <= 3; i++)
                            {
                                var tempLine = splitLine[i].Split('/');
                                tempVertices.Add(allVertices[int.Parse(tempLine[0]) - 1]);
                                tempTextures.Add(allTextures[int.Parse(tempLine[1]) - 1]);
                            }
                            // Calculate Normal
                            var line1 = tempVertices[1] - tempVertices[0];
                            var line2 = tempVertices[2] - tempVertices[0];
                            var normal = Vector3.Cross(line1, line2);
                            // Add vertex data
                            for (var i = 0; i < 3; i++)
                            {
                                gpuVertices.Add(tempVertices[i].X);
                                gpuVertices.Add(tempVertices[i].Y);
                                gpuVertices.Add(tempVertices[i].Z);
                                gpuVertices.Add(normal.X);
                                gpuVertices.Add(normal.Y);
                                gpuVertices.Add(normal.Z);
                                gpuVertices.Add(tempTextures[i].X);
                                gpuVertices.Add(tempTextures[i].Y);
                                gpuVertices.Add(currentMaterial.Ka.X);
                                gpuVertices.Add(currentMaterial.Ka.Y);
                                gpuVertices.Add(currentMaterial.Ka.Z);
                            }
                        }
                        // face has no texture data
                        else
                        {
                            // Calculate Normal
                            var line1 = allVertices[int.Parse(splitLine[2]) - 1] - allVertices[int.Parse(splitLine[1]) - 1];
                            var line2 = allVertices[int.Parse(splitLine[3]) - 1] - allVertices[int.Parse(splitLine[1]) - 1];
                            var normal = Vector3.Cross(line1, line2);
                            // Add vertex data
                            for (var i = 1; i <= 3; i++)
                            {
                                gpuVertices.Add(allVertices[int.Parse(splitLine[i]) - 1].X);
                                gpuVertices.Add(allVertices[int.Parse(splitLine[i]) - 1].Y);
                                gpuVertices.Add(allVertices[int.Parse(splitLine[i]) - 1].Z);
                                gpuVertices.Add(normal.X);
                                gpuVertices.Add(normal.Y);
                                gpuVertices.Add(normal.Z);
                                gpuVertices.Add(0);
                                gpuVertices.Add(0);
                                gpuVertices.Add(currentMaterial.Kd.X);
                                gpuVertices.Add(currentMaterial.Kd.Y);
                                gpuVertices.Add(currentMaterial.Kd.Z);
                            }
                        }

                        break;
                }
            }

            // Add to buffers
            var vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            var vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);

            // Vertices
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            // Normals
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            // Textures
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 11 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);
            // Colors
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 9 * sizeof(float));
            GL.EnableVertexAttribArray(3);

            var vertexArray = gpuVertices.ToArray();
            var numOfVertices = vertexArray.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, numOfVertices * sizeof(float), vertexArray, BufferUsageHint.StaticDraw);

            return new Mesh(vertexArrayObject, vertexBufferObject, numOfVertices);
        }

        private Mesh(int vertexArrayObject, int vertexBufferObject, int numOfVertices)
        {
            _vertexArrayObject = vertexArrayObject;
            _vertexBufferObject = vertexBufferObject;
            _numOfVertices = numOfVertices;
        }

        public void Draw()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BindVertexArray(_vertexArrayObject);

            GL.DrawArrays(PrimitiveType.Triangles, 0, _numOfVertices);
        }
    }
}
