using System;

namespace CricCli.Tests
{
    public class CodecTestResult
    {
        public ImageFormat Format { get; set; } = ImageFormat.RawRGBA;
        public int Width { get; set; }
        public int Height { get; set; }

        public long OriginalSizeBytes { get; set; }
        public long EncodedSizeBytes { get; set; }
        public long DecodedSizeBytes { get; set; }

        public double CompressionRatio => OriginalSizeBytes == 0 ? 0 : (double)EncodedSizeBytes / OriginalSizeBytes;
        public bool IsLossless { get; set; }

        public TimeSpan EncodeTime { get; set; }
        public TimeSpan DecodeTime { get; set; }

        public override string ToString()
        {
            return $"{Format} | {Width}x{Height} | CR: {CompressionRatio:P2} | Lossless: {IsLossless} | Encode: {EncodeTime.TotalMilliseconds:F1} ms | Decode: {DecodeTime.TotalMilliseconds:F1} ms";
        }
    }

    public static class TimeExtensions
    {
        public static string TotalMs(this TimeSpan timeSpan) => timeSpan.TotalMilliseconds.ToString("F1");
    }
}
