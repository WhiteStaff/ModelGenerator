using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using ModelGenerator.DataBase.Models.Enums;

namespace ModelGenerator.DataBase.Models
{
    public class ModelLine
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Model")]
        public Guid ModelId { get; set; }

        public Model Model { get; set; }

        public int LineId { get; set; }
        
        [ForeignKey("Threat")]
        public Guid ThreatId { get; set; }

        public Threat Threat { get; set; }

        [ForeignKey("Target")]
        public Guid TargetId { get; set; }

        public Target Target { get; set; }

        [ForeignKey("Source")]
        public Guid SourceId { get; set; }

        public Source Source { get; set; }

        public RiskProbabilities Possibility { get; set; }

        public string RealisationCoefficient { get; set; }

        public DangerLevel DangerLevel { get; set; }
        
        public bool IsActual { get; set; }
    }
}