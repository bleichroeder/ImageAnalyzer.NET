using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace ImageAnalyzer
{
    public class Metadata
    {
        /// <summary>
        /// This image's name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// This image's DateTime.
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// This image's path.
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// This image's Uri.
        /// </summary>
        public Uri Uri { get; set; }
        /// <summary>
        /// This image's description.
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// Provides methods for image analysis, as well as the ability to perform image comparisons.
    /// </summary>
    public class ImageData
    {
        /// <summary>
        /// This image's meta-data.<br />
        /// Contains information such as image path, date-time, and description.
        /// </summary>
        public Metadata Metadata { get; set; }

        /// <summary>
        /// This ImageData's Bitmap image.
        /// </summary>
        public Bitmap Image { get; set; }

        /// <summary>
        /// Represents whether this image is grayscale.
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
                return ReturnNormalizedValue();
            }
        }

        /// <summary>
        /// Loads a Bitmap image into a ImageData object.
        /// Holds image data such as normalized value and provides basic analysis methods.
        /// </summary>
        /// <param name="image">The Bitmap image.</param>
        /// <param name="imageDateTime">Optional: The image's DateTime.</param>
        /// <param name="imageName">Optional: The image's name.</param>
        public ImageData(Bitmap image)
        {
            Image = image;
            Metadata = new();
        }

        /// <summary>
        /// Loads a Bitmap image into a ImageData object from a Uri.
        /// Holds image data such as normalized value and provides basic analysis methods.
        /// </summary>
        /// <param name="imageUri"></param>
        /// <param name="imageDateTime"></param>
        /// <param name="imageName"></param>
        public ImageData(Uri imageUri)
        {
            Metadata = new Metadata()
            {
                Uri = imageUri,
                Name = imageUri.Segments[^1],
                Path = imageUri.AbsolutePath
            };
            LoadBitmapFromUri(imageUri);
        }

        /// <summary>
        /// Loads a Bitmap image into a ImageData object from a file path.
        /// Holds image data such as normalized value and provides basic analysis methods.
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="imageDateTime"></param>
        /// <param name="imageName"></param>
        public ImageData(string imagePath)
        {
            FileInfo fi = new(imagePath);
            Metadata = new Metadata()
            {
                DateTime = fi.LastWriteTime,
                Path = fi.FullName,
                Name = fi.Name
            };
            LoadBitmapFromFile(imagePath);
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
                throw new Exception("Failed to perform comparison, images are of different sizes." +
                    "Resize images or specify resize during comparison.");
            }

            return Math.Abs(100 * (imageData.NormalizedValue - NormalizedValue));
        }

        /// <summary>
        /// Allows for simple resizing of this ImageData's Bitmap image.
        /// </summary>
        /// <param name="size"></param>
        public void Resize(Size size)
        {
            if(!Image.Size.Equals(size))
                Image = new Bitmap(Image, size);
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
                Image = (Bitmap)System.Drawing
                    .Image.FromFile(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Returns this image's normalized value.
        /// </summary>
        /// <returns></returns>
        private unsafe float ReturnNormalizedValue()
        {
            try
            {
                float retVal = 0;
                int bytesPerPixel = 3;

                Rectangle rec1 = new(0, 0, Image.Width, Image.Height);

                BitmapData ImageData = Image
                    .LockBits(rec1, ImageLockMode.ReadWrite,
                    PixelFormat.Format24bppRgb);

                for (int y = 0; y < ImageData.Height; y++)
                {
                    byte* ImageRow = (byte*)ImageData.Scan0.ToPointer() +
                        (y * ImageData.Stride);

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

        /// <summary>
        /// A boolean value representing whether this image is GrayScale.
        /// </summary>
        /// <returns></returns>
        private unsafe bool GrayScale()
        {
            try
            {
                using var bmp = new Bitmap(Image.Width, Image.Height,
                    PixelFormat.Format32bppArgb);

                using (var g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(Image, 0, 0);
                }

                var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadOnly, bmp.PixelFormat);

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}