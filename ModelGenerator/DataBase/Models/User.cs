using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelGenerator.DataBase.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public ICollection<Model> Model { get; set; }
    }
}