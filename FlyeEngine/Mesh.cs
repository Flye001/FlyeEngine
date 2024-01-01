using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FlyeEngine
{
    /// <summary>
    /// Object that creates / stores a vertex buffer of all vertices for a given model
    /// </summary>
    internal class Mesh
    {
        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _numOfVertices;

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
                                throw new NotImplementedException();
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
                                for (var i = 0; i < 3; i++)
                                {
                                    var coords = index[i].Split('/');
                                    verticesForGpu.Add(allVertices[int.Parse(coords[0]) - 1].X);
                                    verticesForGpu.Add(allVertices[int.Parse(coords[0]) - 1].Y);
                                    verticesForGpu.Add(allVertices[int.Parse(coords[0]) - 1].Z);
                                }
                            }
                            else
                            {
                                var index = line.Split(' ');

                                for (var i = 1; i < 4; i++)
                                {
                                    verticesForGpu.Add(allVertices[int.Parse(index[i]) - 1].X);
                                    verticesForGpu.Add(allVertices[int.Parse(index[0]) - 1].Y);
                                    verticesForGpu.Add(allVertices[int.Parse(index[0]) - 1].Z);
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
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            var vertexArray = verticesForGpu.ToArray();
            _numOfVertices = vertexArray.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, _numOfVertices * sizeof(float), vertexArray, BufferUsageHint.StaticDraw);
        }
    }
}
