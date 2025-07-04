using Xunit;
using System.IO;
using System.Drawing.Imaging;
using CricCli.tests;

namespace CricCli.Tests
{
    public class RawTestImageBuilderTests
    {
        [Fact]
        public void CreatesWhiteImageCorrectly()
        {
            string path = "test_white.raw";
            int width = 4;
            int height = 4;

            RawTestImageBuilder.CreateTestImage(path, width, height, ImageFormat.RawGray8, "white");

            byte[] data = File.ReadAllBytes(path);
            Assert.Equal(width * height, data.Length);

            foreach (byte b in data)
            {
                Assert.Equal(255, b);
            }

            File.Delete(path);
        }
    }
}
