using System.Collections.Generic;

namespace CricCli
{
    public enum ImageFormat
    {
        RawRGBA,   // 4 Bytes/Pixel
        RawRGB,    // 3 Bytes/Pixel
        RawGray8  // 1 Byte/Pixel
    }
    public enum NotSupportedYetButPlanned
    {
        Png,       // Geplant f�r zuk�nftige Implementierung
        Bmp        // Geplant f�r zuk�nftige Implementierung
    }
    public static class FormatHelper
    {
        public static Dictionary<ImageFormat, int> FormatToPixelSize { get; } = new()
        {
            { ImageFormat.RawRGBA, 4 },
            { ImageFormat.RawRGB, 3 },
            { ImageFormat.RawGray8, 1 }
        };
    }

    public enum TestPattern
    {
        White,
        Black,
        Gray,
        Gradient,
        Checker
    }
}
