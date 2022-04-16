using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelGenerator.DataBase.Models
{
    public class Model
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreationTime { get; set; }

        public ICollection<ModelLine> Lines { get; set; }

        public ModelPreferences Preferences { get; set; }

        public SecurityTestResult SecurityTestResult { get; set; }
    }
}