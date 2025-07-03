using System;
using System.IO;

namespace CricCli
{
    public static class Decoder
    {
        public static void Run(string inputPath, string outputPath, ImageFormat format)
        {
            Console.WriteLine($"[Decoder] Dekodieren: {inputPath} → {outputPath} ({format})");

            byte[] input = File.ReadAllBytes(inputPath);
            using var output = new FileStream(outputPath, FileMode.Create);

            int pixelSize = format switch
            {
                ImageFormat.RawRGBA => 4,
                ImageFormat.RawRGB => 3,
                ImageFormat.RawGray8 => 1,
                _ => throw new NotSupportedException($"Format {format} nicht unterstützt")
            };

            int i = 0;
            while (i < input.Length)
            {
                byte b = input[i++];
                if (b == 0xFF)
                {
                    if (i + pixelSize - 1 >= input.Length)
                    {
                        Console.WriteLine("[Decoder] Warnung: unvollständige Rohdaten, Abbruch.");
                        break;
                    }

                    output.Write(input, i, pixelSize);
                    i += pixelSize;
                }
                else
                {
                    for (int j = 0; j < pixelSize; j++)
                        output.WriteByte(b);
                }
            }

            Console.WriteLine("[Decoder] Fertig.");
        }
    }
}
