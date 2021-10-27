using ImageAnalyzer.Extensions.Filters;
using System;
using System.IO;
using Xunit;

namespace ImageAnalyzer.Tests
{
    public class ImageDataTests
    {
        [Fact]
        public void Assign_ImageData_Metadata_FromFile()
        {
            ImageData imageData = new(Path.Combine("Images", "GrayScaleImages", "GrayScaleDog.png"));
            Assert.True(imageData.Metadata.Name == "GrayScaleDog.png");
        }

        [Fact]
        public void Assign_ImageData_Metadata_FromUri()
        {
            ImageData imageData = new(new Uri("https://images.photowall.com/products/46394/car-engine.jpg?h=699&q=85"));
            Assert.True(imageData.Metadata.Name == "car-engine.jpg");
        }

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
            ImageData image1 = new(new Uri("https://cdn.pixabay.com/photo/2013/07/12/17/47/test-pattern-152459_960_720.png"));

            Assert.True(image1.NormalizedValue > 0.4 && image1.NormalizedValue < 0.5);
        }

        [Fact]
        public void Load_Images_From_File()
        {
            ImageData image1 = new(Path.Combine("Images", "GrayScaleImages", "GrayScaleDog.png"));

            Assert.True(image1.IsGrayScale);
        }

        [Fact]
        public void Convert_To_GrayScale()
        {
            ImageData image1 = new(new Uri(@"https://images.photowall.com/products/46394/car-engine.jpg?h=699&q=85"));
            image1.ConvertToGrayScale();
            Assert.True(image1.IsGrayScale);
        }

        [Fact]
        public void Apply_Filter_Sobel()
        {
            ImageData image1 = new(Path.Combine("Images", "EdgeDetection", "Sobel", "Raw.jpg"));

            try
            {
                image1.ApplySobelFilter();
                Assert.True(true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
