using CricCli.Tests;
using System.Globalization;
using System.IO;
using System.Text;

namespace CricCli.Tests
{
    public class TestSummaryWriter : IDisposable
    {
        static IEnumerable<CodecTestResult> results = new List<CodecTestResult>();

        public TestSummaryWriter()
        {
        }

        public void AddResult(CodecTestResult result)
        {
            results = results.Append(result);
        }
        public void WriteToCSV(string outputPath)
        {
            using var writer = new StreamWriter(outputPath, false, Encoding.UTF8);

            string colheader = string.Join(", ", new[]
                {'"' + "Format" + '"',
                 '"' + "Pattern" + '"',
                 '"' + "Size" + '"',
                 '"' + "Original" + '"',
                 '"' + "Encoded" + '"',
                 '"' + "Decoded" + '"',
                 '"' + "ExpectedReduction" + '"',
                 '"' + "CalculatedReduction" + '"',
                 '"' + "IsLossless" + '"',
                 '"' + "EncodeMs" + '"',
                 '"' + "DecodeMs" + '"'});
            // Header
            writer.Write(colheader + "\n");

            foreach (var r in results)
            {
                string line = string.Join(", ", new[]
                {
                '"' + $"{r.Format.ToString()}" + '"',
                '"' + $"{r.Pattern.ToString()}" + '"',
                '"' + $"{r.Width}x{r.Height}" + '"',
                r.OriginalSizeBytes.ToString(),
                r.EncodedSizeBytes.ToString(),
                r.DecodedSizeBytes.ToString(),
                r.ExpectedReduction.ToString("P2", CultureInfo.InvariantCulture),
                r.CalculatedReduction.ToString("P2", CultureInfo.InvariantCulture),
                '"' + $"{r.IsLossless.ToString()}" + '"',
                r.EncodeTime.TotalMilliseconds.ToString("F3", CultureInfo.InvariantCulture),
                r.DecodeTime.TotalMilliseconds.ToString("F3", CultureInfo.InvariantCulture)
            });

                writer.Write(line + "\n");
            }
        }
        
        public void WriteToConsole()
        {
            foreach (var r in results)
            {
                Console.WriteLine ($"{r.Format},{r.Pattern},{r.Width}x{r.Height},{r.OriginalSizeBytes},{r.EncodedSizeBytes},{r.DecodedSizeBytes},{r.ExpectedReduction:F3},{r.CalculatedReduction:F3},{r.IsLossless},{r.EncodeTime.TotalMilliseconds:F1},{r.DecodeTime.TotalMilliseconds:F1}");
            }
        }

        public void Dispose()
        {
            // 🔥 Wird nach dem letzten Test ausgeführt
            WriteToConsole();
            WriteToCSV("test_summary.csv");
        }
    }
}
