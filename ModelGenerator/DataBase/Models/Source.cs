using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelGenerator.DataBase.Models
{
    public class Source
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<ThreatSource> ThreatSource { get; set; } = new List<ThreatSource>();

        public List<ModelPreferencesSource> ModelPreferencesSource { get; set; } = new List<ModelPreferencesSource>();
    }
}