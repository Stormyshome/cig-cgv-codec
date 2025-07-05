using System.Collections;
using System.Collections.Generic;
using CricCli;

namespace CricCli.Tests
{
    public struct MatrixParameters : IEnumerable
    {
        public ImageFormat Format { get; }
        public int Width { get; }
        public int Height { get; }
        public TestPattern Pattern { get; }
        public double ExpectedReduction { get; }
        public MatrixParameters(ImageFormat format, int width, int height, TestPattern pattern, double expectedReduction)
        {
            Format = format;
            Width = width;
            Height = height;
            Pattern = pattern;
            ExpectedReduction = expectedReduction;
        }

        public IEnumerable Add(ImageFormat format, int width, int height, TestPattern pattern, double expectedReduction)
        {
            yield return new MatrixParameters(format, width, height, pattern, expectedReduction);
        }

        public IEnumerator GetEnumerator()
        {
            return new object[] { Format, Width, Height, Pattern, ExpectedReduction }.GetEnumerator();
        }
    }
    public static class TestMatrix
    {
        public static IEnumerable<object[]> AllCases()
        {
            ImageFormat[] formats = new[] { ImageFormat.RawGray8, ImageFormat.RawRGB, ImageFormat.RawRGBA };
            (int, int)[] sizes = new[] { (32, 32), (64, 64), (128, 128) };
            TestPattern[] patterns = new[] { TestPattern.White, TestPattern.Black, TestPattern.Gray, TestPattern.Gradient, TestPattern.Checker };

            foreach (ImageFormat format in formats) // Fixed: Iterate over the array of formats
            {
                foreach ((int width, int height) in sizes)
                {
                    foreach (TestPattern pattern in patterns) // Fixed: Iterate over the array of patterns
                    {
                        double expectedReduction = pattern switch
                        {
                            TestPattern.White or TestPattern.Black => 0.5,
                            TestPattern.Gray => 0.25,
                            TestPattern.Checker => 0.3,
                            TestPattern.Gradient => 0.05,
                            _ => 0
                        };

                        yield return new object[] { format, width, height, pattern, expectedReduction };
                    }
                }
            }
        }
    }
}
