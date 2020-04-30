using System;

namespace ThreatsParser.Exceptions
{
    class NoConnectionException : Exception
    {
        public NoConnectionException() : base("Проблемы с соединением к серверу.")
        { }
    }
}
