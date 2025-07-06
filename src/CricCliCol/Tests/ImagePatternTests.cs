using CricCli;
using CricCli.Tests;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Tests;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace CricCl.Tests
{
    [CollectionDefinition("TestSummaryCollection")]
    public class TestSummaryCollection : ICollectionFixture<TestSummaryWriter>
    {
        // Leerer Marker – nur für xUnit-Verknüpfung
    }
    [Collection("TestSummaryCollection")]
    public class ImagePatternTests
    {
        [Theory]
        [MemberData(nameof(TestMatrix.AllCases), MemberType = typeof(TestMatrix))]
        public void RunTest(ImageFormat format, int width, int height, TestPattern pattern, double expectedReduction)
        {
            TestSummaryWriter testSummary = new TestSummaryWriter();
            // Arrange
            byte[] rawData = RawTestImageBuilder.CreateTestImage(width, height, format, pattern);
            var enSw = Stopwatch.StartNew();
            var encodedBytes = Encoder.Encode(rawData, format, width, height);
            enSw.Stop();
            var deSw = Stopwatch.StartNew();
            var decodedBytes = Decoder.Decode(encodedBytes);
            deSw.Stop();
            bool isEqual = ByteArrayComparer.Compare(rawData, decodedBytes, out string debugMsg);
            if (!isEqual)
            {
                Console.WriteLine(debugMsg);
                Debugger.Break();
            }
            
            var result = new CodecTestResult
            {
                Format = format,
                Pattern = pattern,
                Width = width,
                Height = height,
                OriginalSizeBytes = rawData.Length,
                ExpectedReduction = expectedReduction,
                EncodedSizeBytes = encodedBytes.Length - ImageHeader.GetSize(),
                EncodeTime = enSw.Elapsed,
                DecodedSizeBytes = decodedBytes.Length,
                DecodeTime = deSw.Elapsed,
                IsLossless = isEqual
            };
            if(!result.IsLossless)
            {
                Console.WriteLine(debugMsg);
            }
            testSummary.AddResult(result);
            // Assert
            Assert.NotEmpty(encodedBytes);
            Assert.True(result.IsLossless, $"Kodierung für {format} ist nicht verlustfrei. Erwartet: {rawData.Length} Bytes, erhalten: {decodedBytes.Length} Bytes.");
            
        }
    }
}
