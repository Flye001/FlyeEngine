using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using static System.Formats.Asn1.AsnWriter;

namespace FlyeEngine.PhysicsEngine
{
    internal class BoxCollider
    {
        /// <summary>
        /// Y Component
        /// </summary>
        public float Height { get; private set; }
        /// <summary>
        /// X Component
        /// </summary>
        public float Width { get; private set; }
        /// <summary>
        /// Z Component
        /// </summary>
        public float Depth { get; private set; }

        public Vector3 WorldPosition { get; private set; }
        public Matrix4 ModelMatrix { get; private set; }

        private readonly int _vertexArrayObject;
        private readonly int _vertexBufferObject;
        private readonly int _elementBufferObject;
        private readonly int _indicesLength;

        public float MinX => WorldPosition.X - Width / 2f;
        public float MaxX => WorldPosition.X + Width / 2f;
        public float MinY => WorldPosition.Y - Height / 2f;
        public float MaxY => WorldPosition.Y + Height / 2f;
        public float MinZ => WorldPosition.Z - Depth / 2f;
        public float MaxZ => WorldPosition.Z + Depth / 2f;

        public void UpdatePosition(Vector3 newPosition)
        {
            WorldPosition = newPosition;
            ModelMatrix = Matrix4.CreateTranslation(WorldPosition);
        }

        public BoxCollider(Vector3 size, Vector3 initialPosition)
        {
            WorldPosition = initialPosition;
            ModelMatrix = Matrix4.CreateTranslation(WorldPosition);

            Width = size.X;
            Height = size.Y;
            Depth = size.Z;

            float[] vertices =
            {
                Width / 2f,  Height / 2f,  Depth / 2f,  // top right front
                Width / 2f, -Height / 2f,  Depth / 2f,  // bottom right front
                -Width / 2f, -Height / 2f,  Depth / 2f,  // bottom left front
                -Width / 2f,  Height / 2f,  Depth / 2f,  // top left front
                Width / 2f,  Height / 2f, -Depth / 2f,  // top right back
                Width / 2f, -Height / 2f, -Depth / 2f,  // bottom right back
                -Width / 2f, -Height / 2f, -Depth / 2f,  // bottom left back
                -Width / 2f,  Height / 2f, -Depth / 2f   // top left back
            };

            uint[] indices =
            {
                0, 1, 3,  // front
                1, 2, 3,
                0, 1, 4,  // right
                1, 4, 5,
                1, 2, 5,  // bottom
                2, 5, 6,
                2, 3, 6,  // left
                3, 6, 7,
                0, 3, 4,  // top
                3, 4, 7,
                4, 5, 6,  // back
                4, 6, 7
            };
            _indicesLength = indices.Length;

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            // Vertices
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        }

        public void RenderWireframe()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.BindVertexArray(_vertexArrayObject);

            GL.DrawElements(PrimitiveType.Triangles, _indicesLength, DrawElementsType.UnsignedInt, 0);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }
    }
}
