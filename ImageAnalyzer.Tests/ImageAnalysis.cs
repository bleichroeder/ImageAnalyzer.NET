using ImageAnalyzer.Models;
using ImageAnalyzer.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Xunit;

namespace ImageAnalyzer.Tests
{
    public class ImageAnalysis
    {
        /// <summary>
        /// For testing a real world application in which the program retreives images chronologically.<br />
        /// It then compares image A to B, B to C, C to D etc.<br />
        /// The test will return consecutive comparisons below a specified threshold indicating an issue with the source.
        /// In this scenario the images show the video has frozen by the 4th image.
        /// </summary>
        [Fact]
        public void Return_Two_Conesecutive_Comparisons()
        {
            // Get all of the images we need for analysis
            string[] images = Directory.EnumerateFiles(Path.Combine("Images", "FrozenVideo")).ToArray();

            // Create an empty list of comparisons
            List<ImageComparison> comparisons = new();

            // Create new image comparisons using groups of images
            for (int i = 1; i < images.Length; i++)
            {
                ImageData a = new ((Bitmap)Image.FromFile(images[i - 1]));
                ImageData b = new((Bitmap)Image.FromFile(images[i]));

                comparisons.Add(new ImageComparison(a, b));
            }

            // Create a new ImageAnalysis and pass in our comparisons
            ComparisonAnalysis analysis = new();
            analysis.AddRange(comparisons.ToArray());

            // Set the threshold of change for an issue (1% change or less)
            float threshold = 1.0f;

            // Set the number of consecutive issues to return (2 comparisons/3 consecutive images)
            int consecutiveMinimum = 3;

            // Return consecutively below threshold ImageAnalyses
            ImageComparison[] consecutive = analysis
                .ReturnConsecutiveBelowThreshold(threshold, consecutiveMinimum);

            Assert.True(consecutive.Length.Equals(consecutiveMinimum));
        }


        [Fact]
        public void Comparison_Analysis()
        {
            // Get all of the images we need for analysis
            var images = Directory.EnumerateFiles(Path.Combine("Images", "FrozenVideo")).ToList();

            // Create an empty list of comparisons
            List<ImageData> imageDatas = new();

            images.ForEach(x => imageDatas.Add(new ImageData(x)));
        }
    }
}
