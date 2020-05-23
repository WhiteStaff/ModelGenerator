using System.Collections.Generic;
using System.Linq;
using ModelGenerator.Models.Core.Entities;
using ModelGenerator.Models.Core.Helpers;

namespace ModelGenerator.Models.Core
{
    public static class ModelGeneration
    {
        public static List<ModelLine> GenerateModelForPreview(GlobalPreferences globalPreferences)
        {
            var _model = new List<ModelLine>();
            var counter = 0;

            globalPreferences.Targets
                .Where(x => x.Item2)
                .Select(x => x.Item1)
                .ToList()
                .ForEach(target => globalPreferences.Source
                    .Where(x => x.Item2)
                    .Select(x => x.Item1)
                    .ToList()
                    .ForEach(source => globalPreferences.Items
                        .ForEach(threat =>
                        {
                            if (threat.ExposureSubject.Contains(target) && threat.Source.Contains(source))
                            {
                                counter++;
                                var danger = globalPreferences.Dangers
                                    .FirstOrDefault(
                                        dangerCurr => dangerCurr.Equal(threat.Name, source, threat.Properties))
                                    .DangerLevel;
                                _model.Add(new ModelLine
                                {
                                    Id = "0",
                                    Target = target,
                                    Source = source,
                                    ThreatName = threat.Name,
                                    Possibility = threat.GetPossibility,
                                    Y = Resolver.ResolveRealizeCoef(
                                        (globalPreferences.InitialSecurityLevel.GlobalCoef +
                                         (int) threat.RiskProbabilities) / 20),
                                    Danger = danger.ResolveDanger(),
                                    isActual = Resolver.ResolveActual(danger, threat.RiskProbabilities).ToString()
                                });
                            }
                        })));

            return _model;
        }
    }
}