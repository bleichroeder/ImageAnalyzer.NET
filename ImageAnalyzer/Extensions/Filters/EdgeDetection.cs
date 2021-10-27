using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageAnalyzer.Extensions.Filters
{
    public static class EdgeDetection
    {
        public static void ApplySobelFilter(this ImageData originalImage)
        {
            // Thank you Andraz Krizisnik for this
            // epochabuse.com/csharp-sobel/

            using (originalImage.Image)
            {
                var xkernel = new double[,]
            {
                { -1, 0, 1 },
                { -2, 0, 2 },
                { -1, 0, 1 }
            };

                var ykernel = new double[,]
                {
                {  1,  2,  1 },
                {  0,  0,  0 },
                { -1, -2, -1 }
                };

                int width = originalImage.Image.Width;
                int height = originalImage.Image.Height;

                BitmapData srcData = originalImage.Image.LockBits(new Rectangle(0, 0, width, height),
                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                int bytes = srcData.Stride * srcData.Height;

                byte[] pixelBuffer = new byte[bytes];
                byte[] resultBuffer = new byte[bytes];

                IntPtr srcScan0 = srcData.Scan0;

                Marshal.Copy(srcScan0, pixelBuffer, 0, bytes);

                originalImage.Image.UnlockBits(srcData);

                int filterOffset = 1;

                for (int OffsetY = filterOffset; OffsetY < height - filterOffset; OffsetY++)
                {
                    for (int OffsetX = filterOffset; OffsetX < width - filterOffset; OffsetX++)
                    {
                        double yb;
                        double yg;
                        double yr;
                        double xb;
                        double xg;

                        double xr = xg = xb = yr = yg = yb = 0;
                        double bt;
                        double gt;

                        int byteOffset = OffsetY * srcData.Stride + OffsetX * 4;

                        for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                        {
                            for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                            {
                                int calcOffset = byteOffset + filterX * 4 + filterY * srcData.Stride;
                                xb += (pixelBuffer[calcOffset]) * xkernel[filterY + filterOffset, filterX + filterOffset];
                                xg += (pixelBuffer[calcOffset + 1]) * xkernel[filterY + filterOffset, filterX + filterOffset];
                                xr += (pixelBuffer[calcOffset + 2]) * xkernel[filterY + filterOffset, filterX + filterOffset];
                                yb += (pixelBuffer[calcOffset]) * ykernel[filterY + filterOffset, filterX + filterOffset];
                                yg += (pixelBuffer[calcOffset + 1]) * ykernel[filterY + filterOffset, filterX + filterOffset];
                                yr += (pixelBuffer[calcOffset + 2]) * ykernel[filterY + filterOffset, filterX + filterOffset];
                            }
                        }

                        bt = Math.Sqrt((xb * xb) + (yb * yb));
                        gt = Math.Sqrt((xg * xg) + (yg * yg));
                        double rt = Math.Sqrt((xr * xr) + (yr * yr));

                        if (bt > 255) bt = 255;
                        else if (bt < 0) bt = 0;
                        if (gt > 255) gt = 255;
                        else if (gt < 0) gt = 0;
                        if (rt > 255) rt = 255;
                        else if (rt < 0) rt = 0;

                        resultBuffer[byteOffset] = (byte)(bt);
                        resultBuffer[byteOffset + 1] = (byte)(gt);
                        resultBuffer[byteOffset + 2] = (byte)(rt);
                        resultBuffer[byteOffset + 3] = 255;
                    }
                }

                Bitmap resultImage = new(width, height);

                BitmapData resultData = resultImage.LockBits(new Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);

                resultImage.UnlockBits(resultData);

                originalImage.Image = resultImage;
            }
        }
    }
}
