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
        [MemberData(nameof(TestMatrix.AllCases), MemberType = typeof(TestMatrix))]
        public void RunDecodeTest(ImageFormat format, int width, int height, byte[] rawData)
        {
            var encodedBytes = Encoder.Encode(rawData, format);

            var sw = Stopwatch.StartNew();
            var decodedBytes = Decoder.Decode(encodedBytes);
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
                Width = width,
                Height = height,
                OriginalSizeBytes = rawData.Length,
                EncodedSizeBytes = encodedBytes.Length,
                DecodedSizeBytes = decodedBytes.Length,
                IsLossless = isLossless,
                DecodeTime = sw.Elapsed
            };

            Assert.True(isLossless, $"Decode failed: Not lossless for {format} {width}x{height}");
        }
    }
}
