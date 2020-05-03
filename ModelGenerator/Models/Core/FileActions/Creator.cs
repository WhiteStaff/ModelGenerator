using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using ModelGenerator.DataBase;
using ModelGenerator.DataBase.Models;
using OfficeOpenXml;
using ThreatsParser.Entities;
using ThreatsParser.Exceptions;
using TreatsParser.Core;
using TreatsParser.Core.Helpers;
using ModelLine = ThreatsParser.Entities.ModelLine;
using DbModelLine = ModelGenerator.DataBase.Models.ModelLine;


namespace ThreatsParser.FileActions
{
    static class Creator
    {
        private static GlobalPreferences GetParsedData()
        {
            GlobalPreferences excelData;
            try
            {
                Download("data.xlsx");
                excelData = Parse("data.xlsx");
                return excelData;
            }
            catch
            {
                //TODO create error
                return null;
            }
        }

        public static void SetParsedData(ThreatsDbContext context)
        {
            var data = GetParsedData();

            foreach (var threatModel in data.AllItems)
            {
                var model = threatModel.ToDbModel();

                context.Threat.Add(model);

                foreach (var target in threatModel.ExposureSubject)
                {
                    Target tModel;
                    if (context.Target.Any(x => x.Name == target))
                    {
                        tModel = context.Target.FirstOrDefault(x => x.Name == target);
                    }
                    else
                    {
                        tModel = target.ToDbTargetModel();
                        context.Target.Add(tModel);
                    }

                    var tar2th = new ThreatTarget {Target = tModel, Threat = model};

                    tModel.ThreatTarget.Add(tar2th);

                    context.SaveChanges();
                }

                foreach (var source in threatModel.Source)
                {
                    Source tModel;
                    if (context.Source.Any(x => x.Name == source))
                    {
                        tModel = context.Source.FirstOrDefault(x => x.Name == source);
                    }
                    else
                    {
                        tModel = source.ToDbSourceModel();
                        context.Source.Add(tModel);
                    }

                    var tar2th = new ThreatSource {Source = tModel, Threat = model};

                    tModel.ThreatSource.Add(tar2th);

                    context.SaveChanges();
                }
            }

            context.SaveChanges();
        }

        private static GlobalPreferences Parse(string path)
        {
            var excelData = new GlobalPreferences();
            try
            {
                byte[] bin = File.ReadAllBytes(path);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (MemoryStream stream = new MemoryStream(bin))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    var sheet = excelPackage.Workbook.Worksheets[0];
                    for (int i = 3; i <= sheet.Dimension.End.Row; i++)
                    {
                        var row = new string[8];
                        for (int j = 1; j <= 8; j++)
                        {
                            var value = sheet.Cells[i, j].Value.ToString();
                            row[j - 1] = value;
                        }

                        excelData.RowHandler(row);
                    }
                }
            }
            catch (Exception e)
            {
                Download(path);
                excelData = Parse(path);
            }

            return excelData;
        }

        private static void Download(string name)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile("https://bdu.fstec.ru/files/documents/thrlist.xlsx", name);
                }
            }
            catch
            {
                throw new NoConnectionException();
            }
        }

        private static void RowHandler(this GlobalPreferences globalPreferences, string[] row)
        {
            var currentThreat = new ThreatModel(row);
            globalPreferences.Items.Add(currentThreat);
            globalPreferences.AllItems.Add(currentThreat);

            currentThreat.Source.ForEach(source =>
            {
                if (!globalPreferences.Source.Select(x => x.Item1).Contains(source))
                {
                    globalPreferences.Source.Add((source, true));
                }
            });

            currentThreat.ExposureSubject.ForEach(target =>
            {
                if (!globalPreferences.Targets.Select(x => x.Item1).Contains(target))
                {
                    globalPreferences.Targets.Add((target, true));
                }
            });

            currentThreat.Source
                .ForEach(source =>
                {
                    if (!globalPreferences.Dangers.Any(x =>
                        x.Equal(currentThreat.Name, source, currentThreat.Properties)))
                    {
                        globalPreferences.Dangers.Add(new DangerousLevelLine(source, currentThreat.Name,
                            currentThreat.Properties));
                        globalPreferences.AllDangers.Add(new DangerousLevelLine(source, currentThreat.Name,
                            currentThreat.Properties));
                    }
                });
        }

        public static GlobalPreferences Initialize(ThreatsDbContext context, Guid modelId)
        {
            var globalPreferences = new GlobalPreferences();
            globalPreferences.InitialSecurityLevel = new InitialSecurityLevel();
            globalPreferences.AllItems =
                context.Threat
                    .Include(x => x.ThreatSource)
                    .ThenInclude(x => x.Source)
                    .Include(x => x.ThreatTarget)
                    .ThenInclude(x => x.Target)
                    .ToList()
                    .Select(x => new ThreatModel(x))
                    .ToList();
            if (modelId == Guid.Empty)
            {
                globalPreferences.Items = globalPreferences.AllItems;


                var sources = context.Source.Select(x => x.Name).ToList();
                globalPreferences.Source = sources
                    .Select(x => (x, true))
                    .ToList();


                var targets = context.Target.Select(x => x.Name).ToList();
                globalPreferences.Targets = targets
                    .ToList()
                    .OrderBy(x => x)
                    .Select(x => (x, true))
                    .ToList();


                globalPreferences.AllItems
                    .ForEach(currentThreat => currentThreat.Source
                        .ForEach(source =>
                        {
                            if (!globalPreferences.Dangers.Any(x =>
                                x.Equal(currentThreat.Name, source, currentThreat.Properties)))
                            {
                                globalPreferences.Dangers.Add(new DangerousLevelLine(source, currentThreat.Name,
                                    currentThreat.Properties));
                                globalPreferences.AllDangers.Add(new DangerousLevelLine(source, currentThreat.Name,
                                    currentThreat.Properties));
                            }
                        }));
            }
            else
            {
                var preferences = context.Model.Include(x => x.Preferences).FirstOrDefault(x => x.Id == modelId).Preferences;

                globalPreferences.InitialSecurityLevel.TerritorialLocation = preferences.LocationCharacteristic;
                globalPreferences.InitialSecurityLevel.NetworkCharacteristic = preferences.NetworkCharacteristic;
                globalPreferences.InitialSecurityLevel.PersonalDataActionCharacteristics = preferences.PersonalDataActionCharacteristics;
                globalPreferences.InitialSecurityLevel.PersonalDataSharingLevel = preferences.PersonalDataSharingLevel;
                globalPreferences.InitialSecurityLevel.OtherDbConnections = preferences.OtherDBConnections;
                globalPreferences.InitialSecurityLevel.AnonymityLevel = preferences.AnonymityLevel;
                globalPreferences.InitialSecurityLevel.PersonalDataPermissionSplit = preferences.PersonalDataPermissionSplit;
                globalPreferences.InitialSecurityLevel.PrivacyViolationDanger = preferences.PrivacyViolationDanger;
                globalPreferences.InitialSecurityLevel.IntegrityViolationDanger = preferences.IntegrityViolationDanger;
                globalPreferences.InitialSecurityLevel.AvailabilityViolationDanger = preferences.AvailabilityViolationDanger;

                var full = context.ModelPreferences
                    .Include(x => x.ThreatPossibilities).ThenInclude(x => x.Threat)
                    .Include(x => x.ThreatDangers).ThenInclude(x => x.Source)
                    .Include(x => x.ModelPreferencesSource)
                    .FirstOrDefault(x => x.Id == preferences.Id);

                var notFull = context.ModelPreferences
                    .Include(x => x.ModelPreferencesTarget)
                    .ThenInclude(x => x.Target)
                    .FirstOrDefault(x => x.Id == preferences.Id);

                var sources = full.ModelPreferencesSource.Select(x => x.Source.Name);
                globalPreferences.Source = context.Source
                    .ToList()
                    .Select(x => (x.Name, sources.Contains(x.Name)))
                    .ToList();

                var targets = notFull.ModelPreferencesTarget.Select(x => x.Target.Name);
                globalPreferences.Targets =
                    context.Target
                        .ToList()
                        .OrderBy(x => x.Name)
                        .Select(x => (x.Name, targets.Contains(x.Name)))
                        .ToList();

                globalPreferences.AllItems
                    .ForEach(currentThreat => currentThreat.Source
                        .ForEach(source =>
                        {
                            if (!globalPreferences.Dangers.Any(x =>
                                x.Equal(currentThreat.Name, source, currentThreat.Properties)))
                            {
                                globalPreferences.AllDangers.Add(new DangerousLevelLine(source, currentThreat.Name,
                                    currentThreat.Properties));
                            }
                        }));

                globalPreferences.Items = full.ThreatPossibilities.ToList().Select(x => new ThreatModel
                {
                    Id = x.Threat.ThreatId,
                    Name = x.Threat.Name,
                    Description = x.Threat.Description,
                    ExposureSubject = globalPreferences.AllItems
                        .FirstOrDefault(y => y.Id == x.Threat.ThreatId)
                        .ExposureSubject,
                    Source = globalPreferences.AllItems
                        .FirstOrDefault(y => y.Id == x.Threat.ThreatId).Source,
                    IsHasAvailabilityViolation = x.Threat.IsHasAvailabilityViolation,
                    IsHasIntegrityViolation = x.Threat.IsHasIntegrityViolation,
                    IsHasPrivacyViolation = x.Threat.IsHasPrivacyViolation,
                    RiskProbabilities = x.RiskProbability
                }).ToList();

                globalPreferences.Dangers = full.ThreatDangers.ToList()
                    .Select(x => new DangerousLevelLine(x.Source.Name, x.Threat.Name, x.Properties){DangerLevel = x.DangerLevel}).ToList();
            }

            globalPreferences.ModelId = modelId;

            return globalPreferences;
        }

        public static void SaveModel(ThreatsDbContext context, GlobalPreferences preferences, List<ModelLine> model,
            Guid userId, string name)
        {
            if (preferences.ModelId != Guid.Empty)
            {
                DeleteModel(context, preferences.ModelId);
            }

            var allThreats = context.Threat.ToList();
            var allSources = context.Source.ToList();
            var allTargets = context.Target.ToList();

            var threatsDangers = preferences.Dangers.Select(x => new ThreatDanger
            {
                Id = Guid.NewGuid(),
                ThreatId = allThreats.FirstOrDefault(y => y.Name == x.ThreatName).Id,
                SourceId = allSources.FirstOrDefault(y => y.Name == x.Source).Id,
                Properties = x.Properties,
                DangerLevel = x.DangerLevel
            }).ToList();

            var threatPossibilities = preferences.Items.Select(x => new ThreatPossibility
            {
                Id = Guid.NewGuid(),
                ThreatId = allThreats.FirstOrDefault(y => y.ThreatId == x.Id).Id,
                RiskProbability = x.RiskProbabilities
            }).ToList();

            var modelPreferences = new ModelPreferences
            {
                Id = Guid.NewGuid(),
                ThreatPossibilities = threatPossibilities,
                ThreatDangers = threatsDangers,
                AnonymityLevel = preferences.InitialSecurityLevel.AnonymityLevel,
                LocationCharacteristic = preferences.InitialSecurityLevel.TerritorialLocation,
                NetworkCharacteristic = preferences.InitialSecurityLevel.NetworkCharacteristic,
                OtherDBConnections = preferences.InitialSecurityLevel.OtherDbConnections,
                PersonalDataActionCharacteristics = preferences.InitialSecurityLevel.PersonalDataActionCharacteristics,
                PersonalDataPermissionSplit = preferences.InitialSecurityLevel.PersonalDataPermissionSplit,
                PersonalDataSharingLevel = preferences.InitialSecurityLevel.PersonalDataSharingLevel,
                PrivacyViolationDanger = preferences.InitialSecurityLevel.PrivacyViolationDanger,
                IntegrityViolationDanger = preferences.InitialSecurityLevel.IntegrityViolationDanger,
                AvailabilityViolationDanger = preferences.InitialSecurityLevel.AvailabilityViolationDanger,
            };

            foreach (var source in preferences.Source.Where(x => x.Item2).Select(x => x.Item1))
            {
                var Dbsource = allSources.FirstOrDefault(x => x.Name == source);
                modelPreferences.ModelPreferencesSource.Add(new ModelPreferencesSource
                    {ModelPreferences = modelPreferences, Source = Dbsource});
            }

            foreach (var target in preferences.Targets.Where(x => x.Item2).Select(x => x.Item1))
            {
                var DbTarget = allTargets.FirstOrDefault(x => x.Name == target);
                modelPreferences.ModelPreferencesTarget.Add(new ModelPreferencesTarget()
                    {ModelPreferences = modelPreferences, Target = DbTarget});
            }

            var newModel = new Model
            {
                Id = Guid.NewGuid(),
                Name = name,
                CreationTime = DateTime.Now,
                Preferences = modelPreferences
            };

            var currentUser = context.User.FirstOrDefault(x => x.Id == userId);
            if (currentUser.Model == null)
            {
                currentUser.Model = new List<Model>() {newModel};
            }
            else
            {
                currentUser.Model.Add(newModel);
            }


            var lines = model.Select(modelLine => new DbModelLine
                {
                    Id = Guid.NewGuid(),
                    ModelId = newModel.Id,
                    LineId = int.Parse(modelLine.Id),
                    ThreatId = allThreats.FirstOrDefault(x => x.Name == modelLine.ThreatName).Id,
                    TargetId = allTargets.FirstOrDefault(x => x.Name == modelLine.Target).Id,
                    SourceId = allSources.FirstOrDefault(x => x.Name == modelLine.Source).Id,
                    Possibility = modelLine.Possibility.ResolvePossibility(),
                    RealisationCoefficient = modelLine.Y,
                    DangerLevel = modelLine.Danger.ResolveDanger(),
                    IsActual = modelLine.GetActual
                })
                .ToList();

            context.ThreatDanger.AddRange(threatsDangers);
            context.ThreatPossibility.AddRange(threatPossibilities);
            context.ModelPreferences.Add(modelPreferences);
            context.Model.Add(newModel);
            context.ModelLine.AddRange(lines);
            context.User.Update(currentUser);

            context.SaveChanges();
        }

        public static void DeleteModel(ThreatsDbContext context, Guid modelId)
        {
            var model = context.Model.FirstOrDefault(x => x.Id == modelId);
            context.Model.Remove(model);
            context.SaveChanges();
        }
    }
}