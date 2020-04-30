using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelGenerator.DataBase.Models
{
    public class Threat
    {
        [Key]
        public Guid Id { get; set; }

        public int ThreatId { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsHasPrivacyViolation { get; set; }

        public bool IsHasIntegrityViolation { get; set; }

        public bool IsHasAvailabilityViolation { get; set; }

        public List<ThreatSource> ThreatSource { get; set; } = new List<ThreatSource>();

        public List<ThreatTarget> ThreatTarget { get; set; } = new List<ThreatTarget>();
    }
}