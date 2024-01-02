using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FlyeEngine.GraphicsEngine
{
    /// <summary>
    /// Object that creates / stores a vertex buffer of all vertices for a given model
    /// </summary>
    internal class Mesh
    {
        private readonly int _vertexArrayObject;
        private readonly int _vertexBufferObject;
        private readonly int _numOfVertices;

        public Mesh(string objectFilePath)
        {
            if (!File.Exists(objectFilePath) || !objectFilePath.EndsWith(".obj"))
            {
                throw new FileNotFoundException($"Could not find an object file at '{objectFilePath}'!");
            }

            var fileContent = File.ReadAllLines(objectFilePath);
            if (fileContent.Length <= 0)
            {
                throw new FileLoadException("The object file has no content!");
            }

            List<Vector3> allVertices = new();
            List<float> verticesForGpu = new();
            foreach (var line in fileContent)
            {
                if (string.IsNullOrEmpty(line)) continue;
                try
                {
                    switch (line[0])
                    {
                        case 'v':
                            if (line[1] == 't')
                            {
                                // Texture
                                //throw new NotImplementedException();
                                //var vertex = line.Split(' ');
                                //tempTextures.Add(new Vector2(float.Parse(vertex[1]), float.Parse(vertex[2])));
                            }
                            else
                            {
                                // Vertex
                                var vertex = line.Split(' ');
                                allVertices.Add(new Vector3(float.Parse(vertex[1]), float.Parse(vertex[2]),
                                    float.Parse(vertex[3])));
                            }
                            break;
                        case 'f':
                            if (line.Contains("/")) // Contains texture data
                            {
                                var index = line.Split(' ');
                                var temp = index.ToList();
                                temp.RemoveAt(0);
                                index = temp.ToArray();

                                var line1 = allVertices[1] - allVertices[0];
                                var line2 = allVertices[2] - allVertices[0];
                                var normal = Vector3.Cross(line1, line2);

                                for (var i = 0; i < 3; i++)
                                {
                                    var coords = index[i].Split('/');
                                    verticesForGpu.Add(allVertices[int.Parse(coords[0]) - 1].X);
                                    verticesForGpu.Add(allVertices[int.Parse(coords[0]) - 1].Y);
                                    verticesForGpu.Add(allVertices[int.Parse(coords[0]) - 1].Z);
                                    verticesForGpu.Add(normal.X);
                                    verticesForGpu.Add(normal.Y);
                                    verticesForGpu.Add(normal.Z);
                                }
                            }
                            else
                            {
                                var index = line.Split(' ');

                                var line1 = allVertices[2] - allVertices[1];
                                var line2 = allVertices[3] - allVertices[1];
                                var normal = Vector3.Cross(line1, line2);

                                for (var i = 1; i < 4; i++)
                                {
                                    verticesForGpu.Add(allVertices[int.Parse(index[i]) - 1].X);
                                    verticesForGpu.Add(allVertices[int.Parse(index[i]) - 1].Y);
                                    verticesForGpu.Add(allVertices[int.Parse(index[i]) - 1].Z);
                                    verticesForGpu.Add(normal.X);
                                    verticesForGpu.Add(normal.Y);
                                    verticesForGpu.Add(normal.Z);
                                }
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    throw new FileLoadException($"There was an error parsing the line:\n\n{line}\n\nError: {e.Message}");
                }
            }

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            // Vertices
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            // Normals
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            var vertexArray = verticesForGpu.ToArray();
            _numOfVertices = vertexArray.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, _numOfVertices * sizeof(float), vertexArray, BufferUsageHint.StaticDraw);
        }

        public void Draw()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BindVertexArray(_vertexArrayObject);

            GL.DrawArrays(PrimitiveType.Triangles, 0, _numOfVertices);
        }
    }
}
