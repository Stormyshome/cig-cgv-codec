using System.Collections.Generic;
using CricCli;

namespace CricCli.Tests
{
    public static class TestMatrix
    {
        public static IEnumerable<object[]> AllCases()
        {
            var formats = new[] { ImageFormat.RawGray8, ImageFormat.RawRGB, ImageFormat.RawRGBA };
            var sizes = new[] { (32, 32), (64, 64), (128, 128) };
            var patterns = new[] { "white", "black", "gray", "gradient", "checker" };

            foreach (var format in formats)
                foreach (var (w, h) in sizes)
                    foreach (var pattern in patterns)
                    {
                        double expectedReduction = pattern switch
                        {
                            "white" or "black" => 0.5,
                            "gray" => 0.25,
                            "checker" => 0.3,
                            "gradient" => 0.05,
                            _ => 0
                        };

                        yield return new object[] { format, w, h, pattern, expectedReduction };
                    }
        }
    }
}
