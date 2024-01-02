using System.Drawing;
using OpenTK.Graphics.OpenGL4;

namespace FlyeEngine.GraphicsEngine
{
    internal class Texture
    {
        public int Handle { get; }

        public Texture(string imagePath)
        {
            if (!File.Exists(imagePath)) throw new Exception("Image not found!");

            Handle = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            List<byte> pixels = new();
            var image = new Bitmap(imagePath);
            image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            for (var i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    var pixel = image.GetPixel(i, j);
                    pixels.Add(pixel.R);
                    pixels.Add(pixel.G);
                    pixels.Add(pixel.B);
                    pixels.Add(pixel.A);
                }
            }
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}
