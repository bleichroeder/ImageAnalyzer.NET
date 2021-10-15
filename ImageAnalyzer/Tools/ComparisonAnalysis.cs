using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageAnalyzer.Tools
{
    /// <summary>
    /// Represents a way to perform basic analysis on a collection of ImageComparisons.
    /// </summary>
    public class ComparisonAnalysis
    {
        /// <summary>
        /// The average difference between all comparisons in the collection.
        /// </summary>
        public float AverageDifferencePercentage
        {
            get
            {
                return ImageComparisons
                    .Average(x => x.DifferencePercentage);
            }
        }

        /// <summary>
        /// The minimum difference across all comparisons in the collection.
        /// </summary>
        public float MinimumDifferencePercentage
        {
            get
            {
                return ImageComparisons
                    .Min(x => x.DifferencePercentage);
            }
        }

        /// <summary>
        /// The maximum difference across all comparisons in the collection.
        /// </summary>
        public float MaximumDifferencePercentage
        {
            get
            {
                return ImageComparisons
                    .Max(x => x.DifferencePercentage);
            }
        }

        /// <summary>
        /// The ImageComparison collection for analysis.
        /// </summary>
        public List<ImageComparison> ImageComparisons;

        /// <summary>
        /// Provides methods for image comparison analysis.
        /// </summary>
        /// <param name="comparisons"></param>
        public ComparisonAnalysis(ImageComparison[] comparisons = null)
        {
            if (comparisons == null)
                comparisons = Array.Empty<ImageComparison>();

            ImageComparisons = comparisons.ToList();
        }

        /// <summary>
        /// Adds a ImageComparison to the collection.
        /// </summary>
        /// <param name="imageComparison"></param>
        public void Add(ImageComparison imageComparison)
        {
            ImageComparisons.Add(imageComparison);
        }

        /// <summary>
        /// Adds a range of ImageComparisons to the collection.
        /// </summary>
        /// <param name="imageComparisons"></param>
        public void AddRange(ImageComparison[] imageComparisons)
        {
            ImageComparisons.AddRange(imageComparisons);
        }

        /// <summary>
        /// Returns an array of ImageComparisons where difference percentages were consecutively below a specified threshold.<br />
        /// Will stop and return all items in the collection after meeting the minimum required value.
        /// </summary>
        /// <param name="threshold"></param>
        /// <param name="consecutive"></param>
        /// <returns></returns>
        public ImageComparison[] ReturnConsecutiveBelowThreshold(float threshold, int minimum = 2)
        {
            List<ImageComparison> retVal = new();

            foreach (ImageComparison comparison in ImageComparisons)
            {
                if (comparison.DifferencePercentage < threshold)
                {
                    retVal.Add(comparison);
                }
                else
                {
                    if (retVal.Count > 0)
                        retVal.Clear();
                }

                if (retVal.Count >= minimum)
                    break;
            }

            return retVal.ToArray();
        }
    }
}
