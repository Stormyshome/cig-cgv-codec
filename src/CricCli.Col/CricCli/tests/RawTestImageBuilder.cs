using System;
using System.IO;

namespace CricCli.Tests
{
    public static class RawTestImageBuilder
    {
        public static void CreateTestImage(string path, int width, int height, ImageFormat format, TestPattern pattern)
        {
            using var fs = new FileStream(path, FileMode.Create);
            byte[] rawdata = CreateTestImage(width, height, format, pattern);
            fs.Write(rawdata, 0, rawdata.Length);
            fs.Flush();
            fs.Close();
            Console.WriteLine($"[TestBuilder] Bild erstellt: {path} ({width}x{height}, {format}, Muster: {pattern})");
        }

        public static byte[] CreateTestImage(int width, int height, ImageFormat format, TestPattern pattern)
        {
            var output = new List<byte>();
            int pixelSize = FormatHelper.FormatToPixelSize[format];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte r = 0, g = 0, b = 0, a = 255;

                    switch (pattern)
                    {
                        case TestPattern.Black:
                            r = g = b = 0;
                            break;

                        case TestPattern.White:
                            r = g = b = 255;
                            break;

                        case TestPattern.Gray:
                            r = g = b = 128;
                            break;

                        case TestPattern.Gradient:
                            r = g = b = (byte)((x * 255) / width);
                            break;

                        case TestPattern.Checker:
                            r = g = b = ((x / 8 + y / 8) % 2 == 0) ? (byte)255 : (byte)0;
                            break;

                        default:
                            r = g = b = 0;
                            break;
                    }

                    output.Add(r);
                    if (pixelSize > 1) output.Add(g);
                    if (pixelSize > 2) output.Add(b);
                    if (pixelSize > 3) output.Add(a);
                }
            }
            return output.ToArray();
        }
    } 
}
