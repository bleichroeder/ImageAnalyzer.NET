using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

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
        public string ImageName { get; set; }
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
        /// Loads a Bitmap image into a ImageData object.
        /// Holds image data such as normalized value and provides basic analysis methods.
        /// </summary>
        /// <param name="image">The Bitmap image.</param>
        /// <param name="imageDateTime">Optional: The image's DateTime.</param>
        /// <param name="imageName">Optional: The image's name.</param>
        public ImageData(Bitmap image, DateTime imageDateTime = default, string imageName = null)
        {
            Image = image;
            ImageDateTime = imageDateTime;
            ImageName = imageName;
        }

        /// <summary>
        /// Loads a Bitmap image into a ImageData object from a Uri.
        /// Holds image data such as normalized value and provides basic analysis methods.
        /// </summary>
        /// <param name="imageUri"></param>
        /// <param name="imageDateTime"></param>
        /// <param name="imageName"></param>
        public ImageData(Uri imageUri, DateTime imageDateTime = default, string imageName = null)
        {
            LoadBitmapFromUri(imageUri);
            ImageDateTime = imageDateTime;
            ImageName = imageName;
        }

        /// <summary>
        /// Loads a Bitmap image into a ImageData object from a file path.
        /// Holds image data such as normalized value and provides basic analysis methods.
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="imageDateTime"></param>
        /// <param name="imageName"></param>
        public ImageData(string imagePath, DateTime imageDateTime = default, string imageName = null)
        {
            LoadBitmapFromFile(imagePath);
            ImageDateTime = imageDateTime;
            ImageName = imageName;
        }

        /// <summary>
        /// Allows for simple resizing of this ImageData's Bitmap image.
        /// </summary>
        /// <param name="size"></param>
        public void Resize(Size size)
        {
            Image = new Bitmap(Image, size);
        }

        /// <summary>
        /// Returns percent of change between this image and another image.
        /// Can resize the second image to match the size of this image.
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        public float Difference(ImageData imageData, bool resize = false)
        {
            if (resize)
                imageData.Resize(Image.Size);

            if (!Image.Size.Equals(imageData.Image.Size))
            {
                throw new Exception("Failed to perform comparison, images are of different sizes. Resize images or specify resize during comparison.");
            }

            return Math.Abs(100 * (imageData.NormalizedValue - NormalizedValue));
        }

        /// <summary>
        /// Load a Bitmap into this ImageData from a Uri.
        /// </summary>
        /// <param name="uri"></param>
        private void LoadBitmapFromUri(Uri uri)
        {
            try
            {
                using WebClient wc = new();
                using Stream s = wc.OpenRead(uri);
                Image = new Bitmap(s);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Load a Bitmap into this ImageData from a file path.
        /// </summary>
        /// <param name="path"></param>
        private void LoadBitmapFromFile(string path)
        {
            try
            {
                Image = (Bitmap)System.Drawing.Image.FromFile(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
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
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
