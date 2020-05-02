using System;
using System.Collections.Generic;

namespace ThreatsParser.Entities
{
    public class GlobalPreferences
    {
        public List<ThreatModel> Items { get; set; }

        public List<ThreatModel> AllItems { get; set; }

        public List<(string, bool)> Source { get; set; }

        public List<(string, bool)> Targets { get; set; }

        public InitialSecurityLevel InitialSecurityLevel { get; set; }

        public List<DangerousLevelLine> AllDangers { get; set; }

        public List<DangerousLevelLine> Dangers { get; set; }

        public Guid ModelId { get; set; }

        public GlobalPreferences()
        {
            Items = new List<ThreatModel>();
            AllItems = new List<ThreatModel>();
            Source = new List<(string, bool)>();
            Targets = new List<(string, bool)>();
            Dangers = new List<DangerousLevelLine>();
            AllDangers = new List<DangerousLevelLine>();
        }
    }
}