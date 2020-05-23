using System;

namespace ModelGenerator.Models.Core.Exceptions
{
    class NoFileException : Exception
    {
        public NoFileException() : base("Отсутствует файл базы данных угроз.")
        {

        }
    }
}
