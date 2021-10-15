using ImageAnalyzer.Models;
using System.Drawing;
using System.IO;
using Xunit;

namespace ImageAnalyzer.Tests
{
    public class ImageDataTests
    {
        [Fact]
        public void Detect_GrayScale_Returns_True()
        {
            Bitmap image1 = (Bitmap)Image.FromFile(Path.Combine("Images", "GrayScaleImages", "GrayScaleDog.png"));

            ImageData imageData = new(image1);

            Assert.True(imageData.IsGrayScale);
        }
    }
}
