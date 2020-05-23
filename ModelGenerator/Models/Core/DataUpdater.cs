using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using ModelGenerator.DataBase.Models.Enums;
using ModelGenerator.Models.Core.Entities;
using ModelGenerator.Models.Core.Helpers;

namespace ModelGenerator.Models.Core
{
    public static class DataUpdater
    {
        public static void FirstStepBack(this GlobalPreferences globalPreferences, List<string> values, List<string> targets, DangerLevel Pri, DangerLevel Ava,
            DangerLevel Int)
        {
            globalPreferences.InitialSecurityLevel.PrivacyViolationDanger = Pri;
            globalPreferences.InitialSecurityLevel.IntegrityViolationDanger = Int;
            globalPreferences.InitialSecurityLevel.AvailabilityViolationDanger = Ava;
            globalPreferences.Source = globalPreferences.Source.Select(x => (x.Item1, values.Contains(x.Item1))).ToList();
            globalPreferences.Targets = globalPreferences.Targets.Select(x => (x.Item1, targets.Contains(x.Item1))).ToList();
            var temp = globalPreferences.AllItems
                .Where(x => x.Source
                    .Any(y => globalPreferences.Source
                        .Where(x2 => x2.Item2)
                        .Select(x1 => x1.Item1)
                        .Contains(y)))
                .Where(x =>
                    x.ExposureSubject
                        .Any(y => globalPreferences.Targets
                            .Where(x2 => x2.Item2)
                            .Select(k => k.Item1)
                            .Contains(y)))
                .OrderBy(x => x.Id)
                .ToList();
            foreach (var threatModel in temp)
            {
                var curr = globalPreferences.Items.FirstOrDefault(x => threatModel.Id == x.Id);
                if (curr != null)
                {
                    threatModel.RiskProbabilities = curr.RiskProbabilities;
                }
            }

            globalPreferences.Items = temp;

            foreach (var line in globalPreferences.AllDangers)
            {
                if (line.Properties.Contains("конф"))
                {
                    line.DangerLevel = Pri;
                }

                if (line.Properties.Contains("дост") && line.DangerLevel < Ava)
                {
                    line.DangerLevel = Ava;
                }

                if (line.Properties.Contains("цело") && line.DangerLevel < Int)
                {
                    line.DangerLevel = Int;
                }
            }
        }

        public static void SecondStep(this GlobalPreferences globalPreferences, LocationCharacteristic LocationCharacteristic,
            NetworkCharacteristic NetworkCharacteristic,
            PersonalDataActionCharacteristics PersonalDataActionCharacteristics,
            PersonalDataPermissionSplit PersonalDataPermissionSplit,
            OtherDBConnections OtherDBConnections,
            AnonymityLevel AnonymityLevel,
            PersonalDataSharingLevel PersonalDataSharingLevel)
        {
            globalPreferences.InitialSecurityLevel.AnonymityLevel = AnonymityLevel;
            globalPreferences.InitialSecurityLevel.NetworkCharacteristic = NetworkCharacteristic;
            globalPreferences.InitialSecurityLevel.OtherDbConnections = OtherDBConnections;
            globalPreferences.InitialSecurityLevel.PersonalDataActionCharacteristics = PersonalDataActionCharacteristics;
            globalPreferences.InitialSecurityLevel.PersonalDataPermissionSplit = PersonalDataPermissionSplit;
            globalPreferences.InitialSecurityLevel.PersonalDataSharingLevel = PersonalDataSharingLevel;
            globalPreferences.InitialSecurityLevel.TerritorialLocation = LocationCharacteristic;
        }

        public static void SecondStepBack(this GlobalPreferences globalPreferences, List<RiskProbabilities> Risks)
        {
            for (var i = 0; i < globalPreferences.Items.Count; i++)
            {
                globalPreferences.Items[i].RiskProbabilities = Risks[i];
            }
        }

        public static void ThirdStep(this GlobalPreferences globalPreferences, List<string> values, List<string> targets, DangerLevel Pri, DangerLevel Ava,
            DangerLevel Int)
        {
            globalPreferences.InitialSecurityLevel.PrivacyViolationDanger = Pri;
            globalPreferences.InitialSecurityLevel.IntegrityViolationDanger = Int;
            globalPreferences.InitialSecurityLevel.AvailabilityViolationDanger = Ava;
            globalPreferences.Source = globalPreferences.Source.Select(x => (x.Item1, values.Contains(x.Item1))).ToList();
            globalPreferences.Targets = globalPreferences.Targets.Select(x => (x.Item1, targets.Contains(x.Item1))).ToList();
            var temp = globalPreferences.AllItems
                .Where(x => x.Source
                    .Any(y => globalPreferences.Source
                        .Where(x2 => x2.Item2)
                        .Select(x1 => x1.Item1)
                        .Contains(y)))
                .Where(x =>
                    x.ExposureSubject
                        .Any(y => globalPreferences.Targets
                            .Where(x2 => x2.Item2)
                            .Select(k => k.Item1)
                            .Contains(y)))
                .OrderBy(x => x.Id)
                .ToList();
            foreach (var threatModel in temp)
            {
                var curr = globalPreferences.Items.FirstOrDefault(x => threatModel.Id == x.Id);
                if (curr != null)
                {
                    threatModel.RiskProbabilities = curr.RiskProbabilities;
                }
            }

            globalPreferences.Items = temp;

            foreach (var line in globalPreferences.AllDangers)
            {
                if (line.Properties.Contains("конф"))
                {
                    line.DangerLevel = Pri;
                }

                if (line.Properties.Contains("дост") && line.DangerLevel < Ava)
                {
                    line.DangerLevel = Ava;
                }

                if (line.Properties.Contains("цело") && line.DangerLevel < Int)
                {
                    line.DangerLevel = Int;
                }
            }

            try
            {
                var projectPath = Path.GetDirectoryName(Directory.GetCurrentDirectory());
                var mlContext = new MLContext();
                var trainedModel = mlContext.Model.Load($"{projectPath}\\model.zip", out var modelSchema);


                foreach (var preferencesItem in globalPreferences.Items)
                {
                    var modelPart = Resolver.GetMlModel(globalPreferences);
                    modelPart.ThreatName = globalPreferences.AllItems
                        .OrderBy(x => x.Name)
                        .Select((x, i) => (x, i))
                        .FirstOrDefault(x => x.x.Name == preferencesItem.Name).i;

                    var predictionEngine =
                        mlContext.Model.CreatePredictionEngine<PossibilityMLModel, RiskPrediction>(trainedModel);
                    var prediction = predictionEngine.Predict(modelPart);

                    RiskProbabilities risk;
                    if (prediction.Risk < 0) risk = RiskProbabilities.Unlikely;
                    else if (prediction.Risk >= 4) risk = RiskProbabilities.High;
                    else
                    {
                        risk = (RiskProbabilities)((int)Math.Round(Convert.ToDouble(prediction.Risk)));
                    }

                    preferencesItem.RiskProbabilities = risk;
                }
            }
            catch
            {
                // ignored
            }
        }

        public static void ThirdStepBack(this GlobalPreferences globalPreferences, List<DangerLevel> Dangers)
        {
            for (var i = 0; i < globalPreferences.Dangers.Count; i++)
            {
                globalPreferences.Dangers[i].DangerLevel = Dangers[i];
            }
        }

        public static void ForthStep(this GlobalPreferences globalPreferences, List<RiskProbabilities> Risks)
        {
            for (var i = 0; i < globalPreferences.Items.Count; i++)
            {
                globalPreferences.Items[i].RiskProbabilities = Risks[i];
            }

            globalPreferences.Dangers = globalPreferences.AllDangers
                .Where(x =>
                    globalPreferences.Source
                        .Where(y => y.Item2)
                        .Select(y => y.Item1)
                        .Contains(x.Source) &&
                    globalPreferences.Items
                        .Select(y => y.Name)
                        .Contains(x.ThreatName))
                .OrderBy(x => x.ThreatName)
                .ToList();
        }

        public static void Preview(this GlobalPreferences globalPreferences, List<DangerLevel> Dangers)
        {
            for (var i = 0; i < globalPreferences.Dangers.Count; i++)
            {
                globalPreferences.Dangers[i].DangerLevel = Dangers[i];
            }
        }
    }
}