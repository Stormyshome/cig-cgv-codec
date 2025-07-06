using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public static class ByteArrayComparer
    {
        public static bool Compare(byte[] expected, byte[] actual, out string message, int dumpLength = 32)
        {
            message = string.Empty;

            if (expected.Length != actual.Length)
            {
                message = $"Längen unterschiedlich: Expected = {expected.Length}, Actual = {actual.Length}\n";
                int dumpCount = Math.Min(expected.Length, actual.Length);
                for (int i = 0; i < dumpCount; i++)
                {
                    if (expected[i] != actual[i])
                    {
                        message += $"Mismatch @ {i}: expected={expected[i]}, actual={actual[i]}\n";
                        break;
                    }
                }
                return false;
            }

            for (int i = 0; i < expected.Length; i++)
            {
                if (expected[i] != actual[i])
                {
                    message = $"Mismatch @ {i}: expected={expected[i]}, actual={actual[i]}\n";
                    int end = Math.Min(expected.Length, i + dumpLength);
                    message += $"Expected Dump: {string.Join(", ", expected.Skip(i).Take(end - i))}\n";
                    message += $"Actual Dump:   {string.Join(", ", actual.Skip(i).Take(end - i))}\n";
                    return false;
                }
            }

            return true;
        }
    }

}
