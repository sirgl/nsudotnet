using System;
using System.IO;

namespace Enigma
{
    public class BadKeyFileException : Exception
    {
        public BadKeyFileException(string message)
            : base(message)
        {
        }
    }

    public class EncryptionController
    {
        private readonly string inputFileName;
        private readonly string outputFileName;
        private readonly string algorithm;

        public EncryptionController(string inputFileName, string outputFileName, string algorithm)
        {
            this.inputFileName = inputFileName;
            this.outputFileName = outputFileName;
            this.algorithm = algorithm;
        }

        public void Run()
        {
            try
            {
                var secret = EncryptFile();
                WriteSecret(secret);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
            }
        }

        private Secret EncryptFile()
        {
            Encryptor encryptor = new Encryptor();
            using (FileStream streamToEncrypt = File.Open(inputFileName, FileMode.Open))
            {
                using (var encryptedStream = File.Open(outputFileName, FileMode.Create))
                {
                    return encryptor.Encrypt(algorithm, streamToEncrypt, encryptedStream);
                }
            }
        }


        private void WriteSecret(Secret secret)
        {
            string filePath = Path.GetFileNameWithoutExtension(inputFileName) + ".key.txt";

            using (var sw = new StreamWriter(filePath))
            {
                sw.WriteLine(Convert.ToBase64String(secret.Key));
                sw.WriteLine(Convert.ToBase64String(secret.InitializationVector));
            }
        }
    }
}