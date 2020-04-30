using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelGenerator.DataBase.Models
{
    public class ThreatTarget
    {
        public Guid ThreatId { get; set; }

        public Threat Threat { get; set; }
        
        public Guid TargetId { get; set; }

        public Target Target { get; set; }
    }
}