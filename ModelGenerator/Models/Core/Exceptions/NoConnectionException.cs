using System;

namespace ModelGenerator.Models.Core.Exceptions
{
    class NoConnectionException : Exception
    {
        public NoConnectionException() : base("Проблемы с соединением к серверу.")
        { }
    }
}
