using System;

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
        public float DifferencePercentage => ReturnDifferencePercentage();
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

        private float ReturnDifferencePercentage()
        {
            return Math.Abs(100 * (Image2.NormalizedValue - Image1.NormalizedValue));
        }
    }
}
