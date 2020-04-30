using ModelGenerator.DataBase.Models.Enums;


namespace ThreatsParser.Entities
{
    public class DangerousLevelLine
    {
        public string ThreatName { get; }

        public string Source { get; }

        public string Properties { get; }

        public DangerLevel DangerLevel { get; set; }

        public DangerousLevelLine(string source, string threatName, string properties)
        {
            Source = source;
            ThreatName = threatName;
            Properties = properties;
        }

        public bool Equal(string threatDescription, string source, string properties)
        {
            return ThreatName == threatDescription && Source == source && Properties == properties;
        }
    }
}