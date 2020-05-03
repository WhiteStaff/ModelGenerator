using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using ModelGenerator.DataBase;

namespace ML
{
    class Program
    {
        static void Main(string[] args)
        {
            var connString = "Server=.\\SQLEXPRESS;Database=ThreatDb;Trusted_Connection=True;";
            using var context = new ThreatsDbContext(new DbContextOptions<ThreatsDbContext>());
            var mlContext = new MLContext();
            var inMemoryCollection = GetData(context);
            var data = mlContext.Data.LoadFromEnumerable(inMemoryCollection);

            EstimatorChain<RegressionPredictionTransformer<LinearRegressionModelParameters>> pipelineEstimator =
                mlContext.Transforms.Concatenate("Features", new string[]
                    {
                        "TerritorialLocation",
                        "NetworkCharacteristic",
                        "PersonalDataActionCharacteristics",
                        "PersonalDataPermissionSplit",
                        "OtherDbConnections",
                        "AnonymityLevel",
                        "PersonalDataSharingLevel",
                        "Sources",
                        "Privacy",
                        "Availability",
                        "Integrity",
                        "Targets",
                        "ThreatName"
                    })
                    .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                    .Append(mlContext.Regression.Trainers.Sdca());
            ITransformer trainedModel = pipelineEstimator.Fit(data);
            var projectPath =
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))));
            mlContext.Model.Save(trainedModel, data.Schema, $"{projectPath}\\model.zip");
        }

        private static List<PossibilityMLModel> GetData(ThreatsDbContext context)
        {
            var result = new List<PossibilityMLModel>();
            foreach (var model in context.Model.Include(x => x.Preferences).ToList())
            {
                var prefs = context.ModelPreferences
                    .Include(x => x.ThreatPossibilities).ThenInclude(x => x.Threat)
                    .Include(x => x.ThreatDangers).ThenInclude(x => x.Source)
                    .Include(x => x.ModelPreferencesSource)
                    .FirstOrDefault(x => x.Id == model.Preferences.Id);

                var notFull = context.ModelPreferences
                    .Include(x => x.ModelPreferencesTarget)
                    .ThenInclude(x => x.Target)
                    .FirstOrDefault(x => x.Id == model.Preferences.Id);

                var line = new PossibilityMLModel();
                line.TerritorialLocation = (int) prefs.LocationCharacteristic;
                line.NetworkCharacteristic = (int) prefs.NetworkCharacteristic;
                line.PersonalDataActionCharacteristics = (int) prefs.PersonalDataActionCharacteristics;
                line.PersonalDataPermissionSplit = (int) prefs.PersonalDataPermissionSplit;
                line.OtherDbConnections = (int) prefs.OtherDBConnections;
                line.AnonymityLevel = (int) prefs.AnonymityLevel;
                line.PersonalDataSharingLevel = (int) prefs.PersonalDataSharingLevel;
                line.Privacy = (int) prefs.PrivacyViolationDanger;
                line.Availability = (int) prefs.AvailabilityViolationDanger;
                line.Integrity = (int) prefs.IntegrityViolationDanger;

                var sources = prefs.ModelPreferencesSource.Select(x => x.Source.Name);
                line.Sources = context.Source
                    .ToList()
                    .Select(x => sources.Contains(x.Name) ? 1.0f : 0.0f)
                    .ToArray();

                var targets = notFull.ModelPreferencesTarget.Select(x => x.Target.Name).ToList().OrderBy(x => x)
                    .ToList();
                line.Targets =
                    context.Target
                        .ToList()
                        .Select(x => targets.Contains(x.Name) ? 1f : 0f)
                        .ToArray();

                for (var index = 0; index < prefs.ThreatPossibilities.Count; index++)
                {
                    var uniq = line;
                    var curr = prefs.ThreatPossibilities.ElementAt(index);
                    uniq.ThreatName = context.Threat
                        .Select(x => x.Name)
                        .ToList()
                        .OrderBy(x => x)
                        .Select((x, i) => (x, i))
                        .FirstOrDefault(x => x.x == curr.Threat.Name).i;

                    uniq.Risk = (int) curr.RiskProbability;
                    result.Add(uniq);
                }
            }

            return result;
        }
    }
}