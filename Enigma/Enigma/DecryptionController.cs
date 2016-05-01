using System;
using System.IO;

namespace Enigma
{
    public class DecryptionController
    {
        private readonly string inputFileName;
        private readonly string outputFileName;
        private readonly string keyFileName;
        private readonly string algorithm;

        public DecryptionController(string inputFileName, string outputFileName, string algorithm, string keyFileName)
        {
            this.inputFileName = inputFileName;
            this.outputFileName = outputFileName;
            this.algorithm = algorithm;
            this.keyFileName = keyFileName;
        }

        public void Run()
        {
            try
            {
                var secret = ReadSecret();
                DecryptFile(secret);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
            }
        }

        private Secret ReadSecret()
        {
            var keyFileLines = File.ReadAllLines(keyFileName);
            if (keyFileLines.Length < 2)
            {
                throw new BadKeyFileException("Key file must contains at least 2 lines");
            }
            byte[] key = System.Convert.FromBase64String(keyFileLines[0]);
            byte[] initialVector = System.Convert.FromBase64String(keyFileLines[1]);
            return new Secret(key, initialVector);
        }

        private void DecryptFile(Secret secret)
        {
            Decryptor decryptor = new Decryptor();
            using (FileStream streamToDecrypt = File.Open(inputFileName, FileMode.Open))
            {
                using (var decryptedStream = File.Open(outputFileName, FileMode.Create))
                {
                    decryptor.Decrypt(algorithm, streamToDecrypt, decryptedStream, secret.Key, secret.InitializationVector);
                }
            }
        }
    }
}