using System;
using System.Drawing;
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

            // 🧠 Header lesen (enthält Magic, Version, Width, Height, PixelSize, Startindex)
            ImageHeader header = ImageHeaderHelper.ReadHeader(inputStream);

            if (header.Magic != (byte)ByteMarker.cig)
                throw new InvalidDataException("Unknown or unsupported format");

            int pixelSize = header.PixelSize;

            // Setze Position auf DataStartIndex, damit wir direkt bei den Bilddaten starten
            inputStream.Position = header.DataStartIndex;

            while (inputStream.Position < inputStream.Length)
            {
                byte marker = (byte)inputStream.ReadByte();

                switch ((ByteMarker)marker)
                {
                    case ByteMarker.std:
                        if (inputStream.Position < header.FirstStdIndex)
                        {
                            byte stdPixel = (byte)inputStream.ReadByte();
                            for (int j = 0; j < pixelSize; j++)
                                output.WriteByte(stdPixel);                   
                        }
                        else
                        {
                            byte[] stdPixel = ReadBytes(inputStream, pixelSize);
                            output.Write(stdPixel, 0, pixelSize);
                        }
                        break;
                    case ByteMarker.rle:
                        int rleCount = inputStream.ReadByte();
                        byte rleColor = (byte)inputStream.ReadByte();
                        for (int i = 0; i < rleCount; i++)
                        {
                            for (int j = 0; j < pixelSize; j++)
                                output.WriteByte(rleColor);
                        }
                        break;
                    case ByteMarker.rle2:
                        int rle2Count = inputStream.ReadByte();
                        byte[] rle2Color = ReadBytes(inputStream, pixelSize);
                        for (int i = 0; i < rle2Count; i++)
                            output.Write(rle2Color, 0, pixelSize);
                        break;
                    default:
                        output.WriteByte(marker);
                        break;
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
