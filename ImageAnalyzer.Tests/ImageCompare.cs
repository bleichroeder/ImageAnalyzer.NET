using ImageAnalyzer.Models;
using ImageAnalyzer.Tools;
using System;
using System.Drawing;
using System.IO;
using Xunit;

namespace ImageAnalyzer.Tests
{
    public class ImageCompare
    {
        [Fact]
        public void Compare_IdenticalContent_Images_SameSize_Returns_0()
        {
            // Load our two identical test images
            Bitmap image1 = (Bitmap)Image.FromFile(Path.Combine("Images", "IdenticalImages", "SameSize", "Image1.png"));
            Bitmap image2 = (Bitmap)Image.FromFile(Path.Combine("Images", "IdenticalImages", "SameSize", "Image2.png"));

            ImageComparison comparison = new(new ImageData(image1), new ImageData(image2));

            Assert.True(comparison.DifferencePercentage == 0);
        }

        [Fact]
        public void Compare_NonIdenticalContent_Images_SameSize_Returns_100Plus()
        {
            // Load our two non-identical test images
            Bitmap image1 = (Bitmap)Image.FromFile(Path.Combine("Images", "NonIdenticalImages", "SameSize", "Black.png"));
            Bitmap image2 = (Bitmap)Image.FromFile(Path.Combine("Images", "NonIdenticalImages", "SameSize", "White.png"));

            ImageComparison comparison = new(new ImageData(image1), new ImageData(image2));

            Assert.True(comparison.DifferencePercentage >= 100);
        }

        [Fact]
        public void Compare_IdenticalContent_Images_DifferentSize_Returns_0()
        {
            // Load our two identical test images
            Bitmap image1 = (Bitmap)Image.FromFile(Path.Combine("Images", "IdenticalImages", "DifferentSize", "Image1.png"));
            Bitmap image2 = (Bitmap)Image.FromFile(Path.Combine("Images", "IdenticalImages", "DifferentSize", "Image2.png"));

            ImageData imageData1 = new(image1);
            ImageData imageData2 = new(image2);

            // resize imagedata2
            imageData2.Resize(new Size(imageData1.Image.Width, imageData1.Image.Height));

            // compare
            ImageComparison comparison = new(imageData1, imageData2);

            Assert.True(comparison.DifferencePercentage == 0);
        }

        [Fact]
        public void Compare_NonIdenticalContent_Images_DifferentSize_Returns_()
        {
            // Load our two identical test images
            Bitmap image1 = (Bitmap)Image.FromFile(Path.Combine("Images", "NonIdenticalImages", "DifferentSize", "Large.png"));
            Bitmap image2 = (Bitmap)Image.FromFile(Path.Combine("Images", "NonIdenticalImages", "DifferentSize", "small.png"));

            ImageData imageData1 = new(image1);
            ImageData imageData2 = new(image2);

            // resize imagedata2
            imageData2.Resize(new Size(imageData1.Image.Width, imageData1.Image.Height));

            // compare
            ImageComparison comparison = new(imageData1, imageData2);

            Assert.True(comparison.DifferencePercentage - 4.184278f <= Math.Abs(comparison.DifferencePercentage * .0001f));
        }
    }
}
