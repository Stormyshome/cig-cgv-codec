using System;
using System.IO;

namespace CricCli
{
    public static class Encoder
    {
        public static void Run(string inputPath, string outputPath, ImageFormat format, int width, int height)
        {
            Console.WriteLine($"[Encoder] Kodieren: {inputPath} → {outputPath} ({format})");

            byte[] input = File.ReadAllBytes(inputPath);
            using var output = Encode(input, format, width, height, new FileStream(outputPath, FileMode.Create));

            Console.WriteLine("[Encoder] Fertig.");
        }

        public static FileStream Encode(byte[] input, ImageFormat format, int width, int height, FileStream fs)
        {
            fs.Write(Encode(input, format, width, height));
            return fs;
        }

        public static byte[] Encode(byte[] input, ImageFormat format, int width, int height)
        {
            int pixelSize = FormatHelper.FormatToPixelSize[format];
            int expectedLength = width * height * pixelSize;
            if (input.Length != expectedLength)
                throw new ArgumentException($"Eingabedaten stimmen nicht mit angegebenen Maßen überein: {input.Length} vs erwartet {expectedLength}");

            using var output = new MemoryStream();            

            // Header reservieren
            var header = new ImageHeader
            {
                Magic = (byte)ByteMarker.cig,
                Version = FormatHelper.Version,
                Width = (ushort)width,
                Height = (ushort)height,
                PixelSize = (byte)pixelSize,
                DataStartIndex = ImageHeader.GetSize(),
                FirstStdIndex = -1,
                Reserved = 0
            };

            // Schreibposition nach dem Header merken
            ImageHeaderHelper.WriteHeader(output, header);
            long dataStartPos = output.Position;

            // Start Kompression
            int i = 0;
            long? firstStdPos = null;

            while (i < input.Length)
            {
                bool isCompressed = IsSingleByteColor(input, i, pixelSize);
                int runLength = 1;

                // Check for RLE
                while (i + runLength * pixelSize < input.Length &&
                       runLength < 255 &&
                       EqualPixel(input, i, i + runLength * pixelSize, pixelSize))
                {
                    runLength++;
                }

                if (runLength >= 3)
                {
                    if (isCompressed)
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
                    if (isCompressed)
                    {
                        output.WriteByte(input[i]); // 1 Byte Pixel
                    }
                    else
                    {
                        // Hier wird ein std-Marker geschrieben → Position merken
                        firstStdPos ??= output.Position;
                        output.WriteByte((byte)ByteMarker.std);
                        output.Write(input, i, pixelSize);
                    }
                    i += pixelSize;
                }
            }

            // 🧠 Header am Anfang mit aktualisiertem FirstStdIndex überschreiben
            header.FirstStdIndex = firstStdPos.HasValue
                            ? (int)firstStdPos.Value
                            : (int)output.Length;

            output.Position = 0;
            ImageHeaderHelper.WriteHeader(output, header); // überschreibt die ersten 16 Bytes

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
