using System;

namespace ThreatsParser.Exceptions
{
    class NoFileException : Exception
    {
        public NoFileException() : base("Отсутствует файл базы данных угроз.")
        {

        }
    }
}
