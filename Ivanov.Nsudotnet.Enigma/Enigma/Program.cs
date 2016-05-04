using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    class Program
    {
        const string Help = 
@"arguments: 
encrypt <inputFile> <algorithm> <encryptedFileName> |
decrypt <encryptedFileName> <algorithm> <keyFileName> <decryptedFileName>";



        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Out.WriteLine(Help);
                return;
            }
            if (args[0] == "encrypt")
            {
                if (args.Length < 4)
                {
                    Console.Out.WriteLine(Help);
                    return;
                }
                EncryptionController controller = new EncryptionController(args[1], args[3], args[2]);
                controller.Run();
            }
            else if (args[0] == "decrypt")
            {
                if (args.Length < 5)
                {
                    Console.Out.WriteLine(Help);
                    return;
                }
                DecryptionController controller = new DecryptionController(args[1], args[4], args[2], args[3]);
                controller.Run();
            }
            else
            {
                Console.Out.WriteLine(Help);
                return;
            }
        }
    }
}
