using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Enigma
{
    public class Decryptor
    {


        public void Decrypt(string algorithm, Stream encryptedStream, Stream outputStream, byte[] key, byte[] initialVelctor)
        {
            Func<SymmetricAlgorithm> algorithmSupplier;
            if (!AlgorithmSuppliersHolder.AlgorithmSuppliers.TryGetValue(algorithm.ToLower(), out algorithmSupplier))
            {
                throw new AlgorithmNotSupported(string.Format("Algorithm {0} not suppoted", algorithm));
            }

            Decrypt(encryptedStream, outputStream, key, initialVelctor, algorithmSupplier);
        }

        private static void Decrypt(Stream inputStream, Stream outputStream, byte[] key, byte[] initialVelctor, 
            Func<SymmetricAlgorithm> getAlgorithm)
        {
            using (SymmetricAlgorithm algorithm = getAlgorithm())
            {
                algorithm.Key = key;
                algorithm.IV = initialVelctor;

                var decryptor = algorithm.CreateDecryptor(key, initialVelctor);

                using (CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                {
                    cryptoStream.CopyTo(outputStream);
                }
            }
        }
    }
}