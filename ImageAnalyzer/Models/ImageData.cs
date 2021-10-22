using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageAnalyzer.Models
{
    /// <summary>
    /// Provides methods for image analysis, as well as the ability to perform image comparisons.
    /// </summary>
    public class ImageData
    {
        /// <summary>
        /// Optional: Allows for setting the image's path.
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// Optional: Allows for setting the image's DateTime.
        /// </summary>
        public DateTime ImageDateTime { get; set; }
        /// <summary>
        /// This ImageData's Bitmap image.
        /// </summary>
        public Bitmap Image { get; set; }
        /// <summary>
        /// Determines whether this image is GrayScale.
        /// </summary>
        public bool IsGrayScale
        {
            get
            {
                return GrayScale();
            }
        }

        /// <summary>
        /// Represents the images normalized value between 0.0 and 1.0.<br />
        /// Used for image comparisons.
        /// </summary>
        public float NormalizedValue
        {
            get
            {
                return ReturnImageValue();
            }
        }

        /// <summary>
        /// Accepts a Bitmap image and provides image analysis tools.<br />
        /// Provides the ability to perform image comparisons using the ImageComparison class.
        /// </summary>
        /// <param name="image">The Bitmap image.</param>
        /// <param name="imageDateTime">Optional: The image's DateTime.</param>
        /// <param name="imagePath">Optional: The image's path.</param>
        public ImageData(Bitmap image, DateTime imageDateTime = default, string imagePath = null)
        {
            Image = image;
            ImageDateTime = imageDateTime;
            ImagePath = imagePath;
        }

        /// <summary>
        /// Allows for simple resizing of this ImageData's Bitmap image.
        /// </summary>
        /// <param name="size"></param>
        public void Resize(Size size)
        {
            Image = new Bitmap(Image, size);
        }

        private unsafe float ReturnImageValue()
        {
            try
            {
                float retVal = 0;
                int bytesPerPixel = 3;

                Rectangle rec1 = new(0, 0, Image.Width, Image.Height);

                BitmapData ImageData = Image.LockBits(rec1, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                for (int y = 0; y < ImageData.Height; y++)
                {
                    byte* ImageRow = (byte*)ImageData.Scan0.ToPointer() + (y * ImageData.Stride);

                    for (int x = 0; x < ImageData.Width; x++)
                    {
                        int bIndex = x * bytesPerPixel;
                        int gIndex = bIndex + 1;
                        int rIndex = bIndex + 2;

                        retVal += Math.Abs(ImageRow[rIndex]);
                        retVal += Math.Abs(ImageRow[gIndex]);
                        retVal += Math.Abs(ImageRow[bIndex]);
                    }
                }

                Image.UnlockBits(ImageData);

                return (retVal / 255) / (Image.Width * Image.Height * bytesPerPixel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private unsafe bool GrayScale()
        {
            using (var bmp = new Bitmap(Image.Width, Image.Height, PixelFormat.Format32bppArgb))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(Image, 0, 0);
                }

                var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);

                var pt = (int*)data.Scan0;
                var res = true;

                for (var i = 0; i < data.Height * data.Width; i++)
                {
                    var color = Color.FromArgb(pt[i]);

                    if (color.A != 0 && (color.R != color.G || color.G != color.B))
                    {
                        res = false;
                        break;
                    }
                }

                bmp.UnlockBits(data);

                return res;
            }
        }
    }
}
