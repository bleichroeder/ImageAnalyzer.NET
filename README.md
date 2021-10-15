# ImageAnalyzer.NET

A library of image analysis and image comparison tools.

## Usages

#### ImageComparison analysis
Allows for the analysis of collections of ImageComparisons.\
Can be used for detecting issues in video such as frozen images, or black/blue screens by generating thumbnails and performing an analysis chronologically.
```csharp
using ImageAnalyzer.Models;
using ImageAnalyzer.Tools;

// Our collection of thumbnails ordered chronologically
string[] images = Directory.EnumerateFiles(Path.Combine("Images", "FrozenVideo")).ToArray();

// Create an empty list of comparisons
List<ImageComparison> comparisons = new();

// Create new image comparisons using groups of images (a -> b, b -> c, c -> d)
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
int consecutiveMinimum = 2;

// Return consecutively below threshold ImageAnalyses
ImageComparison[] consecutive = analysis
 .ReturnConsecutiveBelowThreshold(threshold, consecutiveMinimum);

```
#### Basic image comparison
We can use the ImageComparison class to return a percentage of "change" between two images of the same size.
```csharp
using ImageAnalyzer.Models;
using ImageAnalyzer.Tools;

// Load our two identical test images
Bitmap image1 = (Bitmap)Image.FromFile(Path.Combine("Images", "IdenticalImages", "SameSize", "Image1.png"));
Bitmap image2 = (Bitmap)Image.FromFile(Path.Combine("Images", "IdenticalImages", "SameSize", "Image2.png"));

// Create a new ImageComparison
ImageComparison comparison = new(new ImageData(image1), new ImageData(image2));

// Because our images are identical, the DifferencePercentage will return 0
Assert.True(comparison.DifferencePercentage == 0);
```
#### Basic image resizing and comparison
The ImageData class provides a simple method for resizing images before performing comparisons/analyses
```csharp
using ImageAnalyzer.Models;
using ImageAnalyzer.Tools;

// Load our two identical test images of different sizing
Bitmap image1 = (Bitmap)Image.FromFile(Path.Combine("Images", "IdenticalImages", "DifferentSize", "Image1.png"));
Bitmap image2 = (Bitmap)Image.FromFile(Path.Combine("Images", "IdenticalImages", "DifferentSize", "Image2.png"));

ImageData imageData1 = new(image1);
ImageData imageData2 = new(image2);

// Resize imagedata2 using imageData1 width and height values
imageData2.Resize(new Size(imageData1.Image.Width, imageData1.Image.Height));

// Create and perform a new ImageComparison
ImageComparison comparison = new(imageData1, imageData2);

// Because the images were resized and are identical, the DifferencePercentage will return 0
Assert.True(comparison.DifferencePercentage == 0);
```

#### GrayScale detection
The ImageData class provides a simple method for GrayScale detection
```csharp
using ImageAnalyzer.Models;
using ImageAnalyzer.Tools;

Bitmap image1 = (Bitmap)Image.FromFile(Path.Combine("Images", "GrayScaleImages", "GrayScaleDog.png"));

// Create a new ImageData using our GrayScaleDog.png
ImageData imageData = new(image1);

// Because the image is GrayScale, the IsGrayScale property will return true
Assert.True(imageData.IsGrayScale);
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)
