using System;

namespace CricCli.Tests
{
    public class CodecTestResult
    {
        public ImageFormat Format { get; set; }
        public TestPattern Pattern { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public long OriginalSizeBytes { get; set; }
        public long EncodedSizeBytes { get; set; }
        public long DecodedSizeBytes { get; set; }
        public TimeSpan EncodeTime { get; set; }
        public TimeSpan DecodeTime { get; set; }
        public double ExpectedReduction { get; set; }

        public double CalculatedReduction =>
            OriginalSizeBytes == 0 ? 0 :
            1.0 - ((double)EncodedSizeBytes / OriginalSizeBytes);

        public bool IsLossless { get; set; }

        public override string ToString()
        {
            return $"{Format} | {Pattern} | {Width}x{Height} | CR: {CalculatedReduction:P2} | Expected: {ExpectedReduction:P2} | Lossless: {IsLossless} | Encode: {EncodeTime.TotalMilliseconds:F1} ms | Decode: {DecodeTime.TotalMilliseconds:F1} ms";
        }
    }

    public static class TimeExtensions
    {
        public static string TotalMs(this TimeSpan timeSpan) => timeSpan.TotalMilliseconds.ToString("F1");
    }
}
