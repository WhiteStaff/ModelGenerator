using System;
using System.ComponentModel.DataAnnotations;

namespace ModelGenerator.DataBase.Models
{
    public class SecurityTestResult
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Data { get; set; }
    }
}