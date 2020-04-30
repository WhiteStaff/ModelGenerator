namespace ThreatsParser.Entities
{
    class ThreatsChanges
    {
        private readonly Threat _was;
        private readonly Threat _will;
        public string[] Was => _was == null ? new [] {"", "", "", "", "", "", "", ""} : _was.GetValuesAsArray();
        public string[] Will => _will == null ? new[] { "", "", "", "", "", "", "", "" } : _will.GetValuesAsArray();
        public ThreatsChanges(Threat was, Threat will)
        {
            _was = was;
            _will = will;
        }

        public override string ToString()
        {
            return _was != null ? $"УБИ.{_was.Id}" : $"УБИ.{_will.Id}";
        }
    }
}
