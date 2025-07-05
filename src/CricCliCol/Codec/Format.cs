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
        Png,       // Geplant für zukünftige Implementierung
        Bmp        // Geplant für zukünftige Implementierung
    }
    public static class FormatHelper
    {
        public const byte Version = 0x01; // Version 1.0
        public static Dictionary<ImageFormat, int> FormatToPixelSize { get; } = new()
        {
            { ImageFormat.RawRGBA, 0x04 },
            { ImageFormat.RawRGB, 0x03 },
            { ImageFormat.RawGray8, 0x01 }
        };
        public static Dictionary<ByteMarker, int> MarkerToByte { get; } = new()
        {
            { ByteMarker.cig, 0xC9 },
            { ByteMarker.std, 0xFF },
            { ByteMarker.rle, 0xFE },
            { ByteMarker.rle2, 0xFD },
            { ByteMarker.blk, 0xFC },
            { ByteMarker.blk2, 0xFB },
            { ByteMarker.cpl, 0xFA }
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

    public enum ByteMarker
    {
        cig, // Compressed Image
        std, // Standard Pixel (uncompressed)
        rle, // Line with compressed Color
        rle2, // Line with uncompressed Color 
        blk, // Block with compressed Color
        blk2, // Block with uncompressed Color
        cpl // Complex Pixel
    }
}
