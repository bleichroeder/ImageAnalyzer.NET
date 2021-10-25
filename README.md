# ImageAnalyzer.NET

A library of image analysis and image comparison tools.

## Usages
### ImageData
#### Image Comparison
The ImageData class provides a method for determining the percentage of change between itself and a second ImageData object.\
Simply instanciate two ImageData objects using a file-path, URI, or a Bitmap object, and call the **Difference** method to return
the percentage of change represented as a floating point value. If the images are of varying sizes, the **resize**(bool) parameter can be used
to automatically resize the second image to the first image.\
\
In the case of this first example, the resulting value will equal 0 as these two images are identical.
```csharp

ImageData imageData1 = new(Path.Combine("Images", "IdenticalImages", "DifferentSize", "Image1.png"));
ImageData imageData2 = new(Path.Combine("Images", "IdenticalImages", "DifferentSize", "Image2.png"));

float dif = imageData1.Difference(imageData2, true);

```
#### GrayScale Detection
```csharp

ImageData imageData = new(Path.Combine("Images", "GrayScaleImages", "GrayScaleDog.png"));
bool isGrayScale = imageData.IsGrayScale;

```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)
