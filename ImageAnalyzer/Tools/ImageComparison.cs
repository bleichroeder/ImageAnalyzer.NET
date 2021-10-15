using ImageAnalyzer.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageAnalyzer.Tools
{
    /// <summary>
    /// Accepts two Bitmap images and provides methods for image analysis.
    /// </summary>
    public class ImageComparison
    {
        /// <summary>
        /// Image 1 used for image comparison.
        /// </summary>
        public readonly ImageData Image1;
        /// <summary>
        /// Image 2 used for image comparison.
        /// </summary>
        public readonly ImageData Image2;

        /// <summary>
        /// The percentage difference between Image1 and Image2.
        /// </summary>
        public unsafe float DifferencePercentage => ReturnDifferencePercentage();
        /// <summary>
        /// Determines whether Image1 and Image2 are the same size.
        /// </summary>
        public bool AreSameSize
        {
            get
            {
                return Image1.Image.Width == Image2.Image.Width &&
                Image1.Image.Height == Image2.Image.Height;
            }
        }

        /// <summary>
        /// Accepts two Bitmap images and provides methods for image analysis.
        /// </summary>
        /// <param name="image1">Image 1 used for comparison.</param>
        /// <param name="image2">Image 2 used for comparison.</param>
        public ImageComparison(ImageData image1, ImageData image2)
        {
            Image1 = image1;
            Image2 = image2;
        }

        private unsafe float ReturnDifferencePercentage()
        {
            if (!AreSameSize)
                throw new Exception("Failed to calculate difference percentage, images are of different sizes.");

            try
            {
                float retVal = 0;
                int bytesPerPixel = 3;
                Rectangle rec1 = new(0, 0, Image1.Image.Width, Image1.Image.Height);
                Rectangle rec2 = new(0, 0, Image2.Image.Width, Image2.Image.Height);

                BitmapData image1Data = Image1.Image.LockBits(rec1, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                BitmapData image2Data = Image2.Image.LockBits(rec2, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                byte* image1Scan0 = (byte*)image1Data.Scan0.ToPointer();
                byte* image2Scan0 = (byte*)image2Data.Scan0.ToPointer();

                int image1Stride = image1Data.Stride;
                int image2Stride = image2Data.Stride;

                for (int y = 0; y < image1Data.Height; y++)
                {
                    byte* image1Row = image1Scan0 + (y * image1Stride);
                    byte* image2Row = image2Scan0 + (y * image2Stride);

                    for (int x = 0; x < image1Data.Width; x++)
                    {
                        int bIndex = x * bytesPerPixel;
                        int gIndex = bIndex + 1;
                        int rIndex = bIndex + 2;

                        byte pixelR1 = image1Row[rIndex];
                        byte pixelG1 = image1Row[gIndex];
                        byte pixelB1 = image1Row[bIndex];

                        byte pixelR2 = image2Row[rIndex];
                        byte pixelG2 = image2Row[gIndex];
                        byte pixelB2 = image2Row[bIndex];

                        retVal += Math.Abs(pixelR1 - pixelR2);
                        retVal += Math.Abs(pixelG1 - pixelG2);
                        retVal += Math.Abs(pixelB1 - pixelB2);
                    }
                }

                Image1.Image.UnlockBits(image1Data);
                Image2.Image.UnlockBits(image2Data);

                return 100 * (retVal / 255) / (Image1.Image.Width * Image1.Image.Height * bytesPerPixel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
