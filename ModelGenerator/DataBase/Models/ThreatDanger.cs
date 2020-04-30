using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ModelGenerator.DataBase.Models.Enums;

namespace ModelGenerator.DataBase.Models
{
    public class ThreatDanger
    {
        [Key] public Guid Id { get; set; }

        [ForeignKey("Threat")] 
        public Guid ThreatId { get; set; }

        public Threat Threat { get; set; }

        [ForeignKey("Source")] 
        public Guid SourceId { get; set; }

        public Source Source { get; set; }

        public string Properties { get; set; }

        public DangerLevel DangerLevel { get; set; }
    }
}