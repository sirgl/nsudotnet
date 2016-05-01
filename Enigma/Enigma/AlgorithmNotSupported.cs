using System;

namespace Enigma
{
    public class AlgorithmNotSupported : Exception
    {
        public AlgorithmNotSupported(string message) : base(message)
        {
        }
    }
}