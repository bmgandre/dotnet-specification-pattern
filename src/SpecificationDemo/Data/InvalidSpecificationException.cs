using System;

namespace SpecificationDemo.Data
{
    [Serializable]
    public class InvalidSpecificationException : Exception
    {
        public InvalidSpecificationException(string message)
            : base(message)
        {
        }
    }
}