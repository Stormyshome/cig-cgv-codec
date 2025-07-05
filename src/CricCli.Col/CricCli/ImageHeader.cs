using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricCli
{
    public struct ImageHeader
    {
        public byte Magic { get; set; }              // 0xC9
        public byte Version { get; set; }            // e.g. 0x01
        public ushort Width { get; set; }            // width of image in pixel
        public ushort Height { get; set; }           // height of image in pixel
        public byte PixelSize { get; set; }          // e.g. 4 for RGBA
        public int DataStartIndex { get; set; }      // Offset in stream, start of imagedata
        public int FirstStdIndex { get; set; }       // first 0xFF (standard pixel) index
        public byte Reserved { get; set; }           // for extensions
    }
}
