using System;
using System.IO;

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
                Console.WriteLine("  criccli -e <inputfile> <outputfile> <format> <width> <height>");
                Console.WriteLine("  criccli -d <inputfile> <outputfile> <format>");
                Console.WriteLine("  criccli -q zum Beenden...");
                Console.WriteLine("  Formate: RawRGBA | RawRGB | RawGray8");
            }

            bool exit = false;
            while (!exit)
            {
                if (args.Length < 4 && args.Length != 1)
                {
                    Console.Write("> ");
                    args = Console.ReadLine()?.Split(' ') ?? Array.Empty<string>();
                }

                if (args.Length == 1 && args[0].ToLower() == "-q")
                {
                    exit = true;
                    Console.WriteLine("Beenden...");
                    continue;
                }

                if (args.Length < 4)
                {
                    Console.WriteLine("Ungültige Anzahl an Argumenten.");
                    continue;
                }

                string command = args[0].ToLower();
                string input = args[1];
                string output = args[2];
                string formatStr = args[3];

                if (!Enum.TryParse(formatStr, ignoreCase: true, out ImageFormat format))
                {
                    Console.WriteLine($"Ungültiges Format: {formatStr}");
                    continue;
                }

                switch (command)
                {
                    case "-e":
                        if (args.Length < 6 ||
                            !int.TryParse(args[4], out int width) ||
                            !int.TryParse(args[5], out int height))
                        {
                            Console.WriteLine("Bitte Breite und Höhe als Ganzzahlen angeben.");
                            continue;
                        }
                        Encoder.Run(input, output, format, width, height);
                        break;

                    case "-d":
                        Decoder.Run(input, output, format);
                        break;

                    case "-q":
                        exit = true;
                        Console.WriteLine("Beenden...");
                        break;

                    default:
                        Console.WriteLine("Unbekannter Befehl.");
                        break;
                }
            }
        }
    }
}
