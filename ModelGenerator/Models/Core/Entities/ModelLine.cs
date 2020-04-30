
namespace ThreatsParser.Entities
{
    public class ModelLine
    {
        private bool _isActual;

        public string Id { get; set; }

        public string Target { get; set; }

        public string Source { get; set; }

        public string ThreatName { get; set; }

        public string Possibility { get; set; }

        public string Y { get; set; }

        public string Danger { get; set; }

        public string isActual
        {
            get => _isActual ? "Актуальная" : "Неактуальная";
            set => _isActual = value == "True" || value == "Актуальная";
        }

        public ModelLine()
        {
        }

        public ModelLine(int id, ModelLine model)
        {
            Id = id.ToString();
            Target = model.Target;
            Source = model.Source;
            ThreatName = model.ThreatName;
            Possibility = model.Possibility;
            Y = model.Y;
            Danger = model.Danger;
            isActual = model.isActual;
        }
    }
}