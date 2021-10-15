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
