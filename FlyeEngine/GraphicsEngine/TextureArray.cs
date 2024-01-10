using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace FlyeEngine.GraphicsEngine
{
    internal class TextureArray
    {
        public int Handle { get; }

        public TextureArray(Dictionary<string, int> texturePaths)
        {
            //if (!File.Exists(imagePath)) throw new Exception("Image not found!");

            Handle = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2DArray, Handle);

            StbImage.stbi_set_flip_vertically_on_load(1);
            List<byte> pixels = new();
            (int, int) dimensions = (0,0);
            foreach (var texturePath in texturePaths.Keys)
            {
                if (!File.Exists(texturePath)) throw new Exception("Image not found!");

                using (Stream stream = File.OpenRead(texturePath))
                {
                    ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

                    if (dimensions == (0, 0))
                    {
                        dimensions = (image.Width, image.Height);
                    }
                    else
                    {
                        if (dimensions != (image.Width, image.Height))
                        {
                            throw new Exception("Not all the texture files are the same size!");
                        }
                    }

                    pixels.AddRange(image.Data);

                    //GL.TexStorage3D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0,
                    //    PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
                }
                GL.TexStorage3D(TextureTarget3d.Texture2DArray, 1, SizedInternalFormat.Rgba8, dimensions.Item1, dimensions.Item2, texturePath.Length);
            }

            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use(TextureUnit target = TextureUnit.Texture0)
        {
            GL.ActiveTexture(target);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}
