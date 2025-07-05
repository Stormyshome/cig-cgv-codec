using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using CricCli;
using CricCli.Tests;

namespace CricCl.Tests
{
    public class ImagePatternTests
    {
        [Theory]
        [MemberData(nameof(TestMatrix.AllCases), MemberType = typeof(TestMatrix))]
        public void RunTest(ImageFormat format, int width, int height, TestPattern pattern, double expectedReduction)
        {
            // Arrange
            byte[] rawData = RawTestImageBuilder.CreateTestImage(width, height, format, pattern);
            var enSw = Stopwatch.StartNew();
            var encodedBytes = Encoder.Encode(rawData, format);
            enSw.Stop();
            var deSw = Stopwatch.StartNew();
            var decodedBytes = Decoder.Decode(encodedBytes);
            deSw.Stop();

            var result = new CodecTestResult
            {
                Format = format,
                Width = width,
                Height = height,
                OriginalSizeBytes = rawData.Length,
                ExpectedReduction = expectedReduction,
                EncodedSizeBytes = encodedBytes.Length,
                EncodeTime = enSw.Elapsed,
                DecodedSizeBytes = decodedBytes.Length,
                DecodeTime = deSw.Elapsed,
                IsLossless = decodedBytes.SequenceEqual(rawData) 
            };
            if(result.IsLossless)
            {
                Debugger.Break(); // Setze auf Originalgröße, wenn verlustfrei
            }
            else
            {
                Debugger.Break(); // Setze auf kodierte Größe, wenn nicht verlustfrei
            }
            TestSummaryWriter.AddResult(result);
            
            // Assert
            Assert.NotEmpty(encodedBytes);
            Assert.True(result.IsLossless, $"Kodierung für {format} ist nicht verlustfrei. Erwartet: {rawData.Length} Bytes, erhalten: {encodedBytes.Length} Bytes.");
        }
    }
}
