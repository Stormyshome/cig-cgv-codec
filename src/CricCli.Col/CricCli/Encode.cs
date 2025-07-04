using System;
using System.IO;

namespace CricCli
{
    public static class Encoder
    {
        public static void Run(string inputPath, string outputPath, ImageFormat format)
        {
            Console.WriteLine($"[Encoder] Kodieren: {inputPath} → {outputPath} ({format})");

            byte[] input = File.ReadAllBytes(inputPath);

            using var output = Encode(input, format, new FileStream(outputPath, FileMode.Create));

            Console.WriteLine("[Encoder] Fertig.");
        }

        public static FileStream Encode(byte[] input, ImageFormat format, FileStream fs)
        {
            int pixelSize = format switch
            {
                ImageFormat.RawRGBA => 4,
                ImageFormat.RawRGB => 3,
                ImageFormat.RawGray8 => 1,
                _ => throw new NotSupportedException($"Format {format} nicht unterstützt")
            };

            for (int i = 0; i < input.Length; i += pixelSize)
            {
                byte r = input[i];
                byte g = (i + 1 < input.Length) ? input[i + 1] : r;
                byte b = (i + 2 < input.Length) ? input[i + 2] : r;
                byte a = (pixelSize == 4 && i + 3 < input.Length) ? input[i + 3] : r;

                bool allEqual = format switch
                {
                    ImageFormat.RawRGBA => r == g && g == b && b == a,
                    ImageFormat.RawRGB => r == g && g == b,
                    ImageFormat.RawGray8 => true,
                    _ => false
                };

                if (allEqual)
                {
                    fs.WriteByte(r); // Nur 1 Byte
                }
                else
                {
                    fs.WriteByte(0xFF);
                    fs.WriteByte(r);
                    if (pixelSize > 1) fs.WriteByte(g);
                    if (pixelSize > 2) fs.WriteByte(b);
                    if (pixelSize > 3) fs.WriteByte(a);
                }
            }
            return fs;
        }

        public static byte[] Encode(byte[] input, ImageFormat format)
        {
            // Fix for CS1586: Initialize the array with a size or use a list for dynamic resizing
            var output = new List<byte>();

            int pixelSize = format switch
            {
                ImageFormat.RawRGBA => 4,
                ImageFormat.RawRGB => 3,
                ImageFormat.RawGray8 => 1,
                _ => throw new NotSupportedException($"Format {format} nicht unterstützt")
            };

            for (int i = 0; i < input.Length; i += pixelSize)
            {
                byte r = input[i];
                byte g = (i + 1 < input.Length) ? input[i + 1] : r;
                byte b = (i + 2 < input.Length) ? input[i + 2] : r;
                byte a = (pixelSize == 4 && i + 3 < input.Length) ? input[i + 3] : r;

                bool allEqual = format switch
                {
                    ImageFormat.RawRGBA => r == g && g == b && b == a,
                    ImageFormat.RawRGB => r == g && g == b,
                    ImageFormat.RawGray8 => true,
                    _ => false
                };

                if (allEqual)
                {
                    output.Add(r); // Nur 1 Byte
                }
                else
                {
                    output.Add(0xFF);
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
