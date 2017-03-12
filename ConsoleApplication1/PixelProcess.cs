using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    internal class PixelProcess
    {
        public static void Run()
        {
            var image = new Bitmap("Images/victorias.jpg");
            var sw = new Stopwatch();
            sw.Start();
            DetectColorWithUnsafeParallel(image, 253, 129, 255, 84);
            Console.WriteLine(sw.Elapsed);
            Console.ReadKey();
        }

        private static void DetectColorWithGetSetPixel(Bitmap image, byte searchedR, byte searchedG, int searchedB, int tolerance)
        {
            int toleranceSquared = tolerance * tolerance;

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);

                    int diffR = pixel.R - searchedR;
                    int diffG = pixel.G - searchedG;
                    int diffB = pixel.B - searchedB;

                    int distance = diffR * diffR + diffG * diffG + diffB * diffB;

                    image.SetPixel(x, y,
                        distance > toleranceSquared ? Color.Black : Color.White);
                }
            }
            image.Save("fff.jpg");
        }

        private static unsafe void DetectColorWithUnsafe(Bitmap image, byte searchedR, byte searchedG, int searchedB, int tolerance)
        {
            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int bytesPerPixel = 3;

            var scan0 = (byte*)imageData.Scan0.ToPointer();
            int stride = imageData.Stride;

            byte unmatchingValue = 0;
            byte matchingValue = 255;
            int toleranceSquared = tolerance * tolerance;

            for (int y = 0; y < imageData.Height; y++)
            {
                byte* row = scan0 + (y * stride);

                for (int x = 0; x < imageData.Width; x++)
                {
                    // Watch out for actual order (BGR)!
                    int bIndex = x * bytesPerPixel;
                    int gIndex = bIndex + 1;
                    int rIndex = bIndex + 2;

                    byte pixelR = row[rIndex];
                    byte pixelG = row[gIndex];
                    byte pixelB = row[bIndex];

                    int diffR = pixelR - searchedR;
                    int diffG = pixelG - searchedG;
                    int diffB = pixelB - searchedB;

                    int distance = diffR * diffR + diffG * diffG + diffB * diffB;

                    row[rIndex] = row[bIndex] = row[gIndex] = distance >
                                                              toleranceSquared
                        ? unmatchingValue
                        : matchingValue;
                }
            }

            image.UnlockBits(imageData);
        }

        private static void DetectColorWithMarshal(Bitmap image, byte searchedR, byte searchedG, int searchedB, int tolerance)
        {
            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            var imageBytes = new byte[Math.Abs(imageData.Stride) * image.Height];
            IntPtr scan0 = imageData.Scan0;

            Marshal.Copy(scan0, imageBytes, 0, imageBytes.Length);

            byte unmatchingValue = 0;
            byte matchingValue = 255;
            int toleranceSquared = tolerance * tolerance;

            for (int i = 0; i < imageBytes.Length; i += 3)
            {
                byte pixelB = imageBytes[i];
                byte pixelR = imageBytes[i + 2];
                byte pixelG = imageBytes[i + 1];

                int diffR = pixelR - searchedR;
                int diffG = pixelG - searchedG;
                int diffB = pixelB - searchedB;

                int distance = diffR * diffR + diffG * diffG + diffB * diffB;

                imageBytes[i] =
                    imageBytes[i + 1] = imageBytes[i + 2] = distance >
                                                            toleranceSquared
                        ? unmatchingValue
                        : matchingValue;
            }

            Marshal.Copy(imageBytes, 0, scan0, imageBytes.Length);

            image.UnlockBits(imageData);
        }
        static unsafe void DetectColorWithUnsafeParallel(Bitmap image, byte searchedR, byte searchedG, int searchedB, int tolerance)
        {
            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int bytesPerPixel = 3;

            byte* scan0 = (byte*)imageData.Scan0.ToPointer();
            int stride = imageData.Stride;

            byte unmatchingValue = 0;
            byte matchingValue = 255;
            int toleranceSquared = tolerance * tolerance;

            Task[] tasks = new Task[4];
            for (int i = 0; i < tasks.Length; i++)
            {
                int ii = i;
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    int minY = ii < 2 ? 0 : imageData.Height / 2;
                    int maxY = ii < 2 ? imageData.Height / 2 : imageData.Height;

                    int minX = ii % 2 == 0 ? 0 : imageData.Width / 2;
                    int maxX = ii % 2 == 0 ? imageData.Width / 2 : imageData.Width;

                    for (int y = minY; y < maxY; y++)
                    {
                        byte* row = scan0 + (y * stride);

                        for (int x = minX; x < maxX; x++)
                        {
                            int bIndex = x * bytesPerPixel;
                            int gIndex = bIndex + 1;
                            int rIndex = bIndex + 2;

                            byte pixelR = row[rIndex];
                            byte pixelG = row[gIndex];
                            byte pixelB = row[bIndex];

                            int diffR = pixelR - searchedR;
                            int diffG = pixelG - searchedG;
                            int diffB = pixelB - searchedB;

                            int distance = diffR * diffR + diffG * diffG + diffB * diffB;

                            row[rIndex] = row[bIndex] = row[gIndex] = distance >
                                toleranceSquared ? unmatchingValue : matchingValue;
                        }
                    }
                });
            }

            Task.WaitAll(tasks);

            image.UnlockBits(imageData);
        }
    }
}