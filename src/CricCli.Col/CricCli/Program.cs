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
                Console.WriteLine("  criccli -e <inputfile> <outputfile> <format>");
                Console.WriteLine("  criccli -d <inputfile> <outputfile> <format>");
                Console.WriteLine("  Formate: RawRGBA | RawRGB | RawGray8");
#if DEBUG
                Console.WriteLine("  criccli -t <encode> | <decode>");
#endif
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
                case "-e":
                    Encoder.Run(input, output, format);
                    break;

                case "-d":
                    Decoder.Run(input, output, format);
                    break;
#if DEBUG
                case "-t":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Bitte geben Sie einen Testmodus an: encode oder decode");
                        return;
                    }
                    string testMode = args[1].ToLower();
                    if (testMode == "encode")
                    {
                        foreach (ImageFormat testFormat in Enum.GetValues(typeof(ImageFormat)))
                        {
                            Console.WriteLine($"Starte Test für {testFormat}...");
                            new EncodeTests.RunEncodeTest(testFormat, "TestImage", 100, 100, new byte[100 * 100 * (testFormat == ImageFormat.RawRGBA ? 4 : testFormat == ImageFormat.RawRGB ? 3 : 1)]);
                            Encoder.Run("testinput.raw", "testoutput.cric", format);
                            
                        }
                    }
                    else if (testMode == "decode")
                    {
                        Decoder.Run("testinput.cric", "testoutput.raw", format);
                    }
                    else
                    {
                        Console.WriteLine("Ungültiger Testmodus. Bitte verwenden Sie 'encode' oder 'decode'.");
                    }
                    break;

                default:
                    Console.WriteLine("Unbekannter Befehl.");
                    break;
            }
        }
    }
}
