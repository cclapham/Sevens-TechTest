using System;

namespace Sevens.Game.Engine
{
    [Serializable]
    public class UnableToPlayCardException : Exception
    {
        public UnableToPlayCardException()
        { }

        public UnableToPlayCardException(string message)
            : base(message)
        { }

        public UnableToPlayCardException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
