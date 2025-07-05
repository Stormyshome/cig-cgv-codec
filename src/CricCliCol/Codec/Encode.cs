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
            fs.Write(Encode(input, format));
            return fs;
        }

        public static byte[] Encode(byte[] input, ImageFormat format)
        {
            using var output = new MemoryStream();

            int pixelSize = FormatHelper.FormatToPixelSize[format];

            // Header
            output.WriteByte((byte)ByteMarker.cig); // Magic
            output.WriteByte(0x01);                 // Version
            output.WriteByte((byte)pixelSize);      // Format info

            int i = 0;
            while (i < input.Length)
            {
                // Check for RLE
                int runLength = 1;
                while (i + runLength * pixelSize < input.Length &&
                       runLength < 255 &&
                       EqualPixel(input, i, i + runLength * pixelSize, pixelSize))
                {
                    runLength++;
                }

                if (runLength >= 3)
                {
                    bool compressed = IsSingleByteColor(input, i, pixelSize);
                    if (compressed)
                    {
                        output.WriteByte((byte)ByteMarker.rle);
                        output.WriteByte((byte)runLength);
                        output.WriteByte(input[i]);
                    }
                    else
                    {
                        output.WriteByte((byte)ByteMarker.rle2);
                        output.WriteByte((byte)runLength);
                        output.Write(input, i, pixelSize);
                    }
                    i += runLength * pixelSize;
                }
                else
                {
                    bool compressed = IsSingleByteColor(input, i, pixelSize);
                    if (compressed)
                    {
                        output.WriteByte(input[i]);  // Nur 1 Byte, z. B. Grau
                    }
                    else
                    {
                        output.WriteByte((byte)ByteMarker.std);
                        output.Write(input, i, pixelSize);
                    }
                    i += pixelSize;
                }
            }

            return output.ToArray();
        }

        private static bool EqualPixel(byte[] data, int a, int b, int len)
        {
            for (int i = 0; i < len; i++)
                if (data[a + i] != data[b + i]) return false;
            return true;
        }

        private static bool IsSingleByteColor(byte[] data, int index, int pixelSize)
        {
            for (int i = 1; i < pixelSize; i++)
                if (data[index + i] != data[index]) return false;
            return true;
        }
    }
}
