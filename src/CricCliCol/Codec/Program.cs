using System;
using System.IO;
using CricCli.Tests;

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
                Console.WriteLine("  criccli -e <inputfile> <outputfile> <format>");
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
                if (args.Length < 4 && args.Length != 1)
                {
                    Console.WriteLine("Ungültige Anzahl an Argumenten. Bitte geben Sie einen Befehl ein.");
                    continue;
                }
                if (args.Length == 1 && args[0].ToLower() == "-q")
                {
                    exit = true;
                    Console.WriteLine("Beenden...");
                    continue;
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
                    case "-e":
                        Encoder.Run(input, output, format);
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
