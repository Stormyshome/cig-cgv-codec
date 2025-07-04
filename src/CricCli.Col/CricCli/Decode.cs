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
            using var output = Decode(input, format, new FileStream(outputPath, FileMode.Create));
            output.Flush();
            output.Close();
            Console.WriteLine("[Decoder] Fertig.");
        }

        public static FileStream Decode(byte[] input, ImageFormat format, FileStream fs)
        {
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
                    fs.Write(input, i, pixelSize);
                    i += pixelSize;
                }
                else
                {
                    for (int j = 0; j < pixelSize; j++)
                        fs.WriteByte(b);
                }
            }
            return fs;
        }

        public static byte[] Decode(byte[] input, ImageFormat format)
        {
            using var ms = new MemoryStream();
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
                    ms.Write(input, i, pixelSize);
                    i += pixelSize;
                }
                else
                {
                    for (int j = 0; j < pixelSize; j++)
                        ms.WriteByte(b);
                }
            }
            return ms.ToArray();
        }
    }
}
