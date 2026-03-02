using System.Text;

namespace Game
{
    internal class Ascii
    {
        static Ascii()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        internal static string Chr(int p_intByte)
        {
            if ((p_intByte < 0) || (p_intByte > 255))
            {
                throw new ArgumentOutOfRangeException("p_intByte", p_intByte, "Must be between 1 and 255.");
            }
            byte[] bytBuffer = new byte[] { (byte)p_intByte };
            return Encoding.GetEncoding(1252).GetString(bytBuffer);
        }
        internal static int Asc(string p_strChar)
        {
            if ((p_strChar.Length == 0) || (p_strChar.Length > 1))
            {
                throw new ArgumentOutOfRangeException("p_strChar", p_strChar, "Must be a single character.");
            }
            char[] chrBuffer = { Convert.ToChar(p_strChar) };
            byte[] bytBuffer = Encoding.GetEncoding(1252).GetBytes(chrBuffer);
            return (int)bytBuffer[0];
        }

    }
}
