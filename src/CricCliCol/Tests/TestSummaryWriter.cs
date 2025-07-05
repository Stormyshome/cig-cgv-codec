using CricCli.Tests;

namespace CricCli.Tests
{
    public static class TestSummaryWriter
    {
        static IEnumerable<CodecTestResult> results = new List<CodecTestResult>();

        public static void AddResult(CodecTestResult result)
        {
            results = results.Append(result);
        }
        public static void WriteToCSV(string outputPath)
        {
            var lines = new List<string>
            {
                "Format,Type,Size,Original,Encoded,Decoded,CompressionRatio,IsLossless,EncodeMs,DecodeMs"
            };

            foreach (var r in results)
            {
                lines.Add($"{r.Format},{r.Width}x{r.Height},{r.OriginalSizeBytes},{r.EncodedSizeBytes},{r.DecodedSizeBytes},{r.CompressionRatio:F3},{r.IsLossless},{r.EncodeTime.TotalMilliseconds:F1},{r.DecodeTime.TotalMilliseconds:F1}");
            }

            File.WriteAllLines(outputPath, lines);
        }

        public static void WriteToConsole()
        {
            foreach (var r in results)
            {
                Console.WriteLine ($"{r.Format},{r.Width}x{r.Height},{r.OriginalSizeBytes},{r.EncodedSizeBytes},{r.DecodedSizeBytes},{r.CompressionRatio:F3},{r.IsLossless},{r.EncodeTime.TotalMilliseconds:F1},{r.DecodeTime.TotalMilliseconds:F1}");
            }
        }
    }
}
