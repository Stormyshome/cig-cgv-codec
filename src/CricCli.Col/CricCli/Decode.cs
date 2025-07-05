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
            int pixelSize = FormatHelper.FormatToPixelSize[format];
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

        public static byte[] Decode(byte[] input)
        {
            using var inputStream = new MemoryStream(input);
            using var output = new MemoryStream();

            // Header lesen
            if ((ByteMarker)inputStream.ReadByte() != ByteMarker.cig)
                throw new InvalidDataException("Invalid header");

            byte version = (byte)inputStream.ReadByte();
            byte pixelSize = (byte)inputStream.ReadByte();

            while (inputStream.Position < inputStream.Length)
            {
                ByteMarker marker = (ByteMarker)inputStream.ReadByte();

                switch (marker)
                {
                    case ByteMarker.std:
                        byte gray = (byte)inputStream.ReadByte();
                        for (int i = 0; i < pixelSize; i++)
                            output.WriteByte(gray);
                        break;

                    case ByteMarker.rle:
                    case ByteMarker.rle2:
                        int count = inputStream.ReadByte();
                        byte[] color = marker == ByteMarker.rle
                            ? new byte[] { (byte)inputStream.ReadByte() }
                            : ReadBytes(inputStream, pixelSize);
                        for (int i = 0; i < count; i++)
                        {
                            if (color.Length == 1)
                                output.WriteByte(color[0]);
                            else
                                output.Write(color, 0, pixelSize);
                        }
                        break;

                    default:
                        throw new InvalidDataException($"Unknown or unsupported marker: {marker:X2}");
                }
            }

            return output.ToArray();
        }

        private static byte[] ReadBytes(Stream stream, int count)
        {
            byte[] buffer = new byte[count];
            int read = stream.Read(buffer, 0, count);
            if (read != count)
                throw new EndOfStreamException("Unexpected end of stream");
            return buffer;
        }
    }
}
