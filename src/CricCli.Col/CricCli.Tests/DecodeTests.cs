using CricCli.tests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace CricCli.Tests
{
    public class DecodeTests
    {
        [Theory]
        [MemberData(nameof(GetTestCases))]
        public void RunDecodeTest(string format, string imageType, int width, int height, byte[] rawData)
        {
            var encodedBytes = EncodeHelper.Encode(rawData, format);

            var sw = Stopwatch.StartNew();
            var decodedBytes = DecodeHelper.Decode(encodedBytes, format);
            sw.Stop();

            bool isLossless = decodedBytes.Length == rawData.Length;
            if (isLossless)
            {
                for (int i = 0; i < rawData.Length; i++)
                {
                    if (rawData[i] != decodedBytes[i])
                    {
                        isLossless = false;
                        break;
                    }
                }
            }

            var result = new CodecTestResult
            {
                Format = format,
                ImageType = imageType,
                Width = width,
                Height = height,
                OriginalSizeBytes = rawData.Length,
                EncodedSizeBytes = encodedBytes.Length,
                DecodedSizeBytes = decodedBytes.Length,
                IsLossless = isLossless,
                DecodeTime = sw.Elapsed
            };

            Assert.True(isLossless, $"Decode failed: Not lossless for {imageType} {width}x{height}");
        }

        public static IEnumerable<object[]> GetTestCases()
        {
            yield return new object[] { "cig", "gray", 64, 64, RawTestImageBuilder.CreateGrayImage(64, 64) };
            yield return new object[] { "cig", "color", 64, 64, RawTestImageBuilder.CreateColorImage(64, 64) };
        }
    }

    public static class DecodeHelper
    {
        public static byte[] Decode(byte[] input, string format)
        {
            // Dummy-Decoder – bitte mit echter Logik ersetzen
            return input; // placeholder
        }
    }
}
