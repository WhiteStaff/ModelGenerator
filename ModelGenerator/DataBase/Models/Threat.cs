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

        public bool IsHasPrivacyViolation { get; }

        public bool IsHasIntegrityViolation { get; }

        public bool IsHasAvailabilityViolation { get; }

        public List<ThreatSource> ThreatSource { get; }

        public List<ThreatTarget> ThreatTarget { get; }
    }
}