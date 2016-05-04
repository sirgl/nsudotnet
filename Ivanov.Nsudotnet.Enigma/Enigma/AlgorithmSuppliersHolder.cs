using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Enigma
{
    public static class AlgorithmSuppliersHolder
    {
        private static readonly Dictionary<string, Func<SymmetricAlgorithm>> _algorithmSuppliers = new Dictionary<string, Func<SymmetricAlgorithm>>();
        static AlgorithmSuppliersHolder()
        {
            AlgorithmSuppliers["aes"] = Aes.Create;
            AlgorithmSuppliers["des"] = DES.Create;
            AlgorithmSuppliers["rc2"] = RC2.Create;
            AlgorithmSuppliers["rijndael"] = Rijndael.Create;
        }

        public static Dictionary<string, Func<SymmetricAlgorithm>> AlgorithmSuppliers
        {
            get { return _algorithmSuppliers; }
        }
    }
}