using ImageAnalyzer.Models;
using System;
using System.IO;
using Xunit;

namespace ImageAnalyzer.Tests
{
    public class ImageDataTests
    {
        [Fact]
        public void Detect_GrayScale_Returns_True()
        {
            ImageData imageData = new(Path.Combine("Images", "GrayScaleImages", "GrayScaleDog.png"));

            Assert.True(imageData.IsGrayScale);
        }

        [Fact]
        public void Return_Image_Data_Value()
        {
            ImageData image1Data = new(Path.Combine("Images", "NonIdenticalImages", "SameSize", "White.png"));
            ImageData image2Data = new(Path.Combine("Images", "NonIdenticalImages", "SameSize", "Black.png"));

            Assert.True(image1Data.NormalizedValue >= 1.0 && image2Data.NormalizedValue <= 0.0);
        }

        [Fact]
        public void Load_Images_From_Uri()
        {
            ImageData image1 = new(new Uri("https://cdn.pixabay.com/photo/2013/07/12/17/47/test-pattern-152459_960_720.png"), DateTime.Now, "TestImage1");

            Assert.True(image1.NormalizedValue > 0.4 && image1.NormalizedValue < 0.5);
        }

        [Fact]
        public void Load_Images_From_File()
        {
            ImageData image1 = new(Path.Combine("Images", "GrayScaleImages", "GrayScaleDog.png"), DateTime.Now, "TestImage1");

            Assert.True(image1.IsGrayScale);
        }
    }
}
