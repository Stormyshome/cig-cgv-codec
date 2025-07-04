using System;
using System.IO;

namespace CricCli.Tests
{
    public static class RawTestImageBuilder
    {
        public static void CreateTestImage(string path, int width, int height, ImageFormat format, string pattern)
        {
            int pixelSize = format switch
            {
                ImageFormat.RawRGBA => 4,
                ImageFormat.RawRGB => 3,
                ImageFormat.RawGray8 => 1,
                _ => throw new NotSupportedException("Format nicht unterstützt.")
            };

            using var fs = new FileStream(path, FileMode.Create);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    byte r = 0, g = 0, b = 0, a = 255;

                    switch (pattern.ToLower())
                    {
                        case "black":
                            r = g = b = 0;
                            break;

                        case "white":
                            r = g = b = 255;
                            break;

                        case "gray":
                            r = g = b = 128;
                            break;

                        case "gradient":
                            r = g = b = (byte)((x * 255) / width);
                            break;

                        case "checker":
                            r = g = b = ((x / 8 + y / 8) % 2 == 0) ? (byte)255 : (byte)0;
                            break;

                        default:
                            r = g = b = 0;
                            break;
                    }

                    fs.WriteByte(r);
                    if (pixelSize > 1) fs.WriteByte(g);
                    if (pixelSize > 2) fs.WriteByte(b);
                    if (pixelSize > 3) fs.WriteByte(a);
                }

            Console.WriteLine($"[TestBuilder] Bild erstellt: {path} ({width}x{height}, {format}, Muster: {pattern})");
        }
    }
}
