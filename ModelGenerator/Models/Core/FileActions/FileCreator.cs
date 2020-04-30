using System;
using System.IO;
using System.Linq;
using System.Net;
using OfficeOpenXml;
using ThreatsParser.Entities;
using ThreatsParser.Exceptions;


namespace ThreatsParser.FileActions
{
    static class FileCreator
    {
        public static GlobalPreferences GetParsedData()
        {
            GlobalPreferences excelData;
            try
            {
                excelData = Parse("data.xlsx");
                return excelData;
            }
            catch
            {
                //TODO create error
                return null;
            }
        }

        private static GlobalPreferences Parse(string path)
        {
            var excelData = new GlobalPreferences();
            try
            {
                byte[] bin = File.ReadAllBytes(path);
                using (MemoryStream stream = new MemoryStream(bin))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    var sheet = excelPackage.Workbook.Worksheets[1];
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
            catch
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

        private static void RowHandler(this GlobalPreferences globalPreferences , string[] row)
        {
            var currentThreat = new Threat(row);
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
                        globalPreferences.Dangers.Add(new DangerousLevelLine(source, currentThreat.Name, currentThreat.Properties));
                        globalPreferences.AllDangers.Add(new DangerousLevelLine(source, currentThreat.Name, currentThreat.Properties));
                    }
                });

        }
    }
}