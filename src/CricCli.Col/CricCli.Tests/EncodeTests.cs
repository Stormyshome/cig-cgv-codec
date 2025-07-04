using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace CricCli.Tests
{
    public class EncodeTests
    {
        [Theory]
        [MemberData(nameof(GetTestCases))]
        public void RunEncodeTest(string format, string imageType, int width, int height, byte[] rawData)
        {
            string tempInputPath = Path.GetTempFileName();
            File.WriteAllBytes(tempInputPath, rawData);

            var sw = Stopwatch.StartNew();
            var encodedBytes = EncodeHelper.Encode(rawData, format);
            sw.Stop();

            var encodeResult = new CodecTestResult
            {
                Format = format,
                ImageType = imageType,
                Width = width,
                Height = height,
                OriginalSizeBytes = rawData.Length,
                EncodedSizeBytes = encodedBytes.Length,
                EncodeTime = sw.Elapsed
            };

            Assert.True(encodedBytes.Length > 0);
            File.Delete(tempInputPath);
        }

        public static IEnumerable<object[]> GetTestCases()
        {
            yield return new object[] { "cig", "gray", 64, 64, RawTestImageBuilder.CreateGrayImage(64, 64) };
            yield return new object[] { "cig", "color", 64, 64, RawTestImageBuilder.CreateColorImage(64, 64) };
        }
    }

    public static class EncodeHelper
    {
        public static byte[] Encode(byte[] input, string format)
        {
            // Dummy implementation – ersetzt durch echte Logik
            return input; // placeholder
        }
    }

    public static class RawTestImageBuilder
    {
        public static byte[] CreateGrayImage(int width, int height)
        {
            byte[] data = new byte[width * height];
            for (int i = 0; i < data.Length; i++) data[i] = 128;
            return data;
        }

        public static byte[] CreateColorImage(int width, int height)
        {
            byte[] data = new byte[width * height * 3];
            for (int i = 0; i < data.Length; i++) data[i] = (byte)(i % 256);
            return data;
        }
    }
}
