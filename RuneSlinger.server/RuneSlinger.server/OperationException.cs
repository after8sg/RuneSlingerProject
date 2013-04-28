using System;

namespace RuneSlinger.server
{
    public class OperationException : Exception
    {
        public OperationException(string message) : base(message)
        {
        }
    }
}
