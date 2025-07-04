using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using CricCli;
using CricCli.Tests;

namespace CricCl.Tests
{
    public class EncodeTests
    {
        [Theory]
        [MemberData(nameof(TestMatrix.AllCases), MemberType = typeof(TestMatrix))]
        public void RunEncodeTest(ImageFormat format, int width, int height, TestPattern pattern, double expectedReduction)
        {
            // Arrange
            byte[] rawData = RawTestImageBuilder.CreateTestImage(width, height, format, pattern);
            var sw = Stopwatch.StartNew();
            var encodedBytes = Encoder.Encode(rawData, format);
            sw.Stop();

            var result = new CodecTestResult
            {
                Format = format,
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
            Assert.True(result.IsLossless, $"Kodierung für {format} ist nicht verlustfrei. Erwartet: {rawData.Length} Bytes, erhalten: {encodedBytes.Length} Bytes.");
        }
    }
}
