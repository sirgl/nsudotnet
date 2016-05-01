using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Enigma
{
    public struct Secret
    {
        public Secret(byte[] key, byte[] initializationVector)
            : this()
        {
            Key = key;
            InitializationVector = initializationVector;
        }

        public byte[] Key { get; private set; }
        public byte[] InitializationVector { get; private set; }
    }

    public class Encryptor
    {
        private static readonly Dictionary<string, Func<SymmetricAlgorithm>> AlgorithmSuppliers = new Dictionary<string, Func<SymmetricAlgorithm>>();

        static Encryptor()
        {
            AlgorithmSuppliers["aes"] = Aes.Create;
            AlgorithmSuppliers["des"] = DES.Create;
            AlgorithmSuppliers["rc2"] = RC2.Create;
            AlgorithmSuppliers["rijndael"] = Rijndael.Create;
        }

        public Secret Encrypt(string algorithm, Stream inputStream, Stream encryptedStream)
        {
            var algorithmSupplier = AlgorithmSuppliers[algorithm.ToLower()];
            if (algorithmSupplier == null)
            {
                throw new AlgorithmNotSupported(string.Format("Algorithm {0} not suppoted", algorithm));
            }

            return Encrypt(inputStream, encryptedStream, algorithmSupplier);
        }

        private static Secret Encrypt(Stream inputStream, Stream outputStream,
            Func<SymmetricAlgorithm> getAlgorithm)
        {
            using (SymmetricAlgorithm algorithm = getAlgorithm())
            {
                Secret secret = new Secret(algorithm.Key, algorithm.IV);

                ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);

                using (CryptoStream cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
                {
                    inputStream.CopyTo(cryptoStream);
                }
                return secret;
            }            
        }
    }
}