using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelGenerator.DataBase.Models
{
    public class Target
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<ThreatTarget> ThreatTarget { get; set; } = new List<ThreatTarget>();

        public List<ModelPreferencesTarget> ModelPreferencesTarget { get; set; } = new List<ModelPreferencesTarget>();
    }
}