using System;

namespace CricCli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CRIC Codec CLI v0.1");

            if (args.Length < 4)
            {
                Console.WriteLine("Verwendung:");
                Console.WriteLine("  criccli encode <inputfile> <outputfile> <format>");
                Console.WriteLine("  criccli decode <inputfile> <outputfile> <format>");
                Console.WriteLine("  Formate: RawRGBA | RawRGB | RawGray8");
                return;
            }

            string command = args[0].ToLower();
            string input = args[1];
            string output = args[2];
            string formatStr = args[3];

            if (!Enum.TryParse(formatStr, ignoreCase: true, out ImageFormat format))
            {
                Console.WriteLine($"Ungültiges Format: {formatStr}");
                return;
            }

            switch (command)
            {
                case "encode":
                    Encoder.Run(input, output, format);
                    break;

                case "decode":
                    Decoder.Run(input, output, format);
                    break;

                default:
                    Console.WriteLine("Unbekannter Befehl.");
                    break;
            }
        }
    }
}
