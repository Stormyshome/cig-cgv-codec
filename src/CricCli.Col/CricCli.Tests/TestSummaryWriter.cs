public static class TestSummaryWriter
{
    public static void WriteResults(IEnumerable<CodecTestResult> results, string outputPath)
    {
        var lines = new List<string>
        {
            "Format,Type,Size,Original,Encoded,Decoded,CompressionRatio,IsLossless,EncodeMs,DecodeMs"
        };

        foreach (var r in results)
        {
            lines.Add($"{r.Format},{r.ImageType},{r.Width}x{r.Height},{r.OriginalSizeBytes},{r.EncodedSizeBytes},{r.DecodedSizeBytes},{r.CompressionRatio:F3},{r.IsLossless},{r.EncodeTime.TotalMilliseconds:F1},{r.DecodeTime.TotalMilliseconds:F1}");
        }

        File.WriteAllLines(outputPath, lines);
    }
}
