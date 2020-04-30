using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ModelGenerator.DataBase.Models.Enums;

namespace ModelGenerator.DataBase.Models
{
    public class ThreatPossibility
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Threat")]
        public Guid ThreatId { get; set; }

        public Threat Threat { get; set; }

        public RiskProbabilities RiskProbability { get; set; }
    }
}