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
        public Secret Encrypt(string algorithm, Stream inputStream, Stream encryptedStream)
        {
            Func<SymmetricAlgorithm> algorithmSupplier;
            if (!AlgorithmSuppliersHolder.AlgorithmSuppliers.TryGetValue(algorithm.ToLower(), out algorithmSupplier))
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