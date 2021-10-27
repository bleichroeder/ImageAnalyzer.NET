using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageAnalyzer.Extensions.Filters
{
    public static class GrayScaling
    {
        public static void ConvertToGrayScale(this ImageData originalImage)
        {
            Bitmap newBitmap = new(originalImage.Image.Width, originalImage.Image.Height);

            try
            {
                using Graphics g = Graphics.FromImage(newBitmap);
                ColorMatrix colorMatrix = new(
                   new float[][]
                   {
                        new float[] {.3f, .3f, .3f, 0, 0},
                        new float[] {.59f, .59f, .59f, 0, 0},
                        new float[] {.11f, .11f, .11f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                   });

                using ImageAttributes attributes = new();
                attributes.SetColorMatrix(colorMatrix);
                g.DrawImage(originalImage.Image, new Rectangle(0, 0, originalImage.Image.Width, originalImage.Image.Height),
                    0, 0, originalImage.Image.Width, originalImage.Image.Height, GraphicsUnit.Pixel, attributes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            originalImage.Image = newBitmap;
        }
    }
}
