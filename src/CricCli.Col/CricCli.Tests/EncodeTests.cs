using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using CricCli.tests;

namespace CricCli.Tests
{
    public class EncodeTests
    {
        [Theory]
        [MemberData(nameof(TestMatrix.AllCases), MemberType = typeof(TestMatrix))]
        public void RunEncodeTest(ImageFormat format, string imageType, int width, int height, byte[] rawData)
        {
            // Arrange
            var sw = Stopwatch.StartNew();
            var encodedBytes = Encoder.Encode(rawData, format);
            sw.Stop();

            var result = new CodecTestResult
            {
                Format = format,
                ImageType = imageType,
                Width = width,
                Height = height,
                OriginalSizeBytes = rawData.Length,
                EncodedSizeBytes = encodedBytes.Length,
                EncodeTime = sw.Elapsed,
                IsLossless = Decoder.Decode(encodedBytes, format).SequenceEqual(rawData) // vorläufige Prüfung, ersetze später durch echten Decode
            };

            TestSummaryWriter.AddResult(result);

            // Assert
            Assert.NotEmpty(encodedBytes);
        }
    }
}
