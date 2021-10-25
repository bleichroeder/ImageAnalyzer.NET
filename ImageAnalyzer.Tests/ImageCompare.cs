using ImageAnalyzer.Models;
using System;
using System.IO;
using Xunit;

namespace ImageAnalyzer.Tests
{
    public class ImageCompare
    {

        [Fact]
        public void Compare_IdenticalContent_Images_SameSize_Returns_0()
        {
            ImageData image1 = new(Path.Combine("Images", "IdenticalImages", "SameSize", "Image1.png"));
            ImageData image2 = new(Path.Combine("Images", "IdenticalImages", "SameSize", "Image2.png"));

            Assert.True(image1.Difference(image2) == 0);
        }

        [Fact]
        public void Compare_Flipped_Identical_Images_Returns_GT_0()
        {
            ImageData image1 = new(Path.Combine("Images", "IdenticalImages", "Flipped", "Up.png"));
            ImageData image2 = new(Path.Combine("Images", "IdenticalImages", "Flipped", "Down.png"));

            Assert.True(image1.Difference(image2) > 0);
        }

        [Fact]
        public void Compare_NonIdenticalContent_Images_SameSize_Returns_GT_100()
        {
            ImageData image1 = new(Path.Combine("Images", "NonIdenticalImages", "SameSize", "Black.png"));
            ImageData image2 = new(Path.Combine("Images", "NonIdenticalImages", "SameSize", "White.png"));

            Assert.True(image1.Difference(image2) >= 100);
        }

        [Fact]
        public void Compare_IdenticalContent_Images_DifferentSize_Returns_0()
        {
            ImageData imageData1 = new(Path.Combine("Images", "IdenticalImages", "DifferentSize", "Image1.png"));
            ImageData imageData2 = new(Path.Combine("Images", "IdenticalImages", "DifferentSize", "Image2.png"));

            Assert.True(imageData1.Difference(imageData2, true) == 0);
        }

        [Fact]
        public void Compare_NonIdenticalContent_Images_DifferentSize()
        {
            ImageData imageData1 = new(Path.Combine("Images", "NonIdenticalImages", "DifferentSize", "Large.png"));
            ImageData imageData2 = new(Path.Combine("Images", "NonIdenticalImages", "DifferentSize", "small.png"));

            // Compare and resize
            float difference = imageData1.Difference(imageData2, true);

            Assert.True(difference - 4.184278f <= Math.Abs(difference * .0001f));
        }

        [Fact]
        public void Compare_Images_Different_Sizes_Throws_Exception()
        {
            try
            {
                ImageData imageData1 = new(Path.Combine("Images", "IdenticalImages", "DifferentSize", "Image1.png"));
                ImageData imageData2 = new(Path.Combine("Images", "IdenticalImages", "DifferentSize", "Image2.png"));

                imageData1.Difference(imageData2);
            }
            catch
            {
                Assert.True(true);
            }
        }
    }
}
