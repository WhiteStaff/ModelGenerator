using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks.Sources;

namespace ModelGenerator.DataBase.Models
{
    public class ThreatSource
    {
        

        public Guid ThreatId { get; set; }

        public Threat Threat { get; set; }

        public Guid SourceId { get; set; }

        public Source Source { get; set; }
    }
}