using System;
using System.Collections.Generic;

namespace ThreatsParser.Entities
{
    public class GlobalPreferences
    {
        public List<Threat> Items { get; set; }

        public List<Threat> AllItems { get; set; }

        public List<(string, bool)> Source { get; set; }

        public List<(string, bool)> Targets { get; set; }

        public InitialSecurityLevel InitialSecurityLevel { get; set; }

        public List<DangerousLevelLine> AllDangers { get; set; }

        public List<DangerousLevelLine> Dangers { get; set; }

        public GlobalPreferences()
        {
            Items = new List<Threat>();
            AllItems = new List<Threat>();
            Source = new List<(string, bool)>();
            Targets = new List<(string, bool)>();
            Dangers = new List<DangerousLevelLine>();
            AllDangers = new List<DangerousLevelLine>();
        }
    }
}