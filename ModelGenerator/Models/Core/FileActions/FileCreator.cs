using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ModelGenerator.DataBase;
using ModelGenerator.DataBase.Models;
using OfficeOpenXml;
using ThreatsParser.Entities;
using ThreatsParser.Exceptions;
using TreatsParser.Core;


namespace ThreatsParser.FileActions
{
    static class FileCreator
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

            foreach (var threatModel in data.Items)
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

                    var tar2th = new ThreatTarget {Target = tModel, Threat = model };

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

                    var tar2th = new ThreatSource {Source = tModel, Threat = model };

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
            globalPreferences.Items = globalPreferences.AllItems =
                context.Threat
                    .Include(x => x.ThreatSource)
                    .ThenInclude(x => x.Source)
                    .Include(x => x.ThreatTarget)
                    .ThenInclude(x => x.Target)
                    .ToList()
                    .Select(x => new ThreatModel(x))
                    .ToList();

            var sources = context.Source.Select(x => x.Name).ToList();
            globalPreferences.Source = modelId == Guid.Empty
                ? sources
                    .Select(x => (x, true))
                    .ToList()
                : context.Model
                    .FirstOrDefault(x => x.Id == modelId)
                    .Preferences.ModelPreferencesSource
                    .Select(x => x.Source.Name)
                    .Select(x => (x, sources.Contains(x)))
                    .ToList();

            var targets = context.Target.Select(x => x.Name).ToList();
            globalPreferences.Targets = modelId == Guid.Empty
                ? targets
                    .Select(x => (x, true))
                    .ToList()
                : context.Model
                    .FirstOrDefault(x => x.Id == modelId)
                    .Preferences.ModelPreferencesTarget
                    .Select(x => x.Target.Name)
                    .Select(x => (x, targets.Contains(x)))
                    .ToList();

            if (modelId == Guid.Empty)
            {
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
                globalPreferences.Dangers = globalPreferences.AllDangers =
                    context.Model.Include(x => x.Preferences.ThreatDangers).FirstOrDefault(x => x.Id == modelId)
                        .Preferences.ThreatDangers.ToList()
                        .Select(x => new DangerousLevelLine(x.Source.Name, x.Threat.Name, x.Properties)).ToList();
            }

            return globalPreferences;
        }
    }
}