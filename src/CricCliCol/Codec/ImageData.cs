using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricCli
{
    public class ImageData
    {
        public ImageHeader Header { get; set; } // Header information
        public byte[] Data { get; set; } // Image data bytes
        public ImageData()
        {
            Header = new ImageHeader();
            Data = Array.Empty<byte>();
        }
        public ImageData(ImageHeader header, byte[] data)
        {
            Header = header;
            Data = data ?? Array.Empty<byte>();
        }
        public ImageData(byte[] data)
        {
            Header = new ImageHeader();
            Data = data ?? Array.Empty<byte>();
        }
        public ImageData(int width, int height, ImageFormat format)
        {
            Header = new ImageHeader
            {
                Magic = (byte)FormatHelper.MarkerToByte[ByteMarker.cig],
                Version = FormatHelper.Version,
                Width = (ushort)width,
                Height = (ushort)height,
                PixelSize = (byte)FormatHelper.FormatToPixelSize[format],
                DataStartIndex = ImageHeader.GetSize(),
                FirstStdIndex = -1, // Not set yet
                Reserved = 0 // Reserved for future use
            };
            Data = new byte[width * height * FormatHelper.FormatToPixelSize[format]];
        }
        public override string ToString()
        {
            return $"Image {Header.Width}x{Header.Height}, PixelSize: {Header.PixelSize}, Format: {Header.Version}, DataLength: {Data.Length}";
        }
        public bool IsValidSize()
        {
            int expectedSize = Header.Width * Header.Height * Header.PixelSize;
            return Data != null && Data.Length == expectedSize;
        }

    }
}
