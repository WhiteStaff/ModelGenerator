using System;
using System.Collections.Generic;
using System.Linq;
using ModelGenerator.DataBase.Models.Enums;
using ModelGenerator.Models.Core.Entities;

namespace ModelGenerator.Models.Core.Helpers
{
    public static class Resolver
    {
        public static string ResolveRealizeCoef(double coef)
        {
            if (coef <= 0.3) return $"{coef} - низкая";
            if (coef <= 0.6) return $"{coef} - средняя";
            if (coef <= 0.8) return $"{coef} - высокая";
            return $"{coef} - очень высокая";
        }

        public static string ResolveDanger(this DangerLevel level)
        {
            switch (level)
            {
                case DangerLevel.Low:
                    return "Низкая";
                case DangerLevel.Medium:
                    return "Средняя";
                default:
                    return "Высокая";
            }
        }

        public static DangerLevel ResolveDanger(this string level)
        {
            switch (level)
            {
                case "Низкая":
                    return DangerLevel.Low;
                case "Средняя":
                    return DangerLevel.Medium;
                default:
                    return DangerLevel.High;
            }
        }

        public static bool ResolveActual(DangerLevel dangerLevel, RiskProbabilities risk)
        {
            if (risk == RiskProbabilities.Unlikely) return false;
            if ((dangerLevel == DangerLevel.Low || dangerLevel == DangerLevel.Medium) &&
                risk == RiskProbabilities.Low) return false;
            if (dangerLevel == DangerLevel.Low && risk == RiskProbabilities.Medium) return false;

            return true;
        }

        public static List<string> ResolveSourceType(string value)
        {
            var result = new List<string>();
            value.Split(',').ToList().ForEach(x => result.Add(UppercaseFirst(x.Trim() == "" ? "Отсутствует" : x.Trim())));

            return result;
        }

        public static List<string> ResolveTargetsType(string value)
        {
            var result = new List<string>();
            string[] splitted = null;
            if (value.ToLower().Contains("микропрограммное, системное и прикладное программное обеспечение"))
            {
                result.Add("Микропрограммное, системное и прикладное программное обеспечение");
                splitted = value.Split(new string[] { "микропрограммное, системное и прикладное программное обеспечение", ","}, StringSplitOptions.RemoveEmptyEntries);
            }
            if (value.ToLower().Contains("виртуальные устройства хранения, обработки и передачи данных"))
            {
                result.Add("Виртуальные устройства хранения, обработки и передачи данных");
                splitted = value.Split(new string[] { "Виртуальные устройства хранения, обработки и передачи данных", "," }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (value.ToLower().Contains("информационная система, иммигрированная в облако"))
            {
                result.Add("Информационная система, иммигрированная в облако");
                splitted = value.Split(new string[] { "Виртуальные устройства хранения, обработки и передачи данных", ","}, StringSplitOptions.RemoveEmptyEntries);
            }
            if (value.ToLower() == "вычислительный узел суперкомпьютера")
            {
                result.Add("Вычислительные узлы суперкомпьютера");
                return result;
            }
            if (value == "Технические средства воздушного кондиционирования, включая трубопроводные системы для циркуляции охлаждённого воздуха в ЦОД, программируемые логические контроллеры, распределённые системы контроля, управленческие системы и другие программные средства контроля")
            {
                result.Add("Технические средства воздушного кондиционирования, включая трубопроводные системы для циркуляции охлаждённого воздуха в ЦОД, программируемые логические контроллеры, распределённые системы контроля, управленческие системы и другие программные средства контроля");
                return result;
            }


            splitted = value.Contains('(') ? new []{value} : value.Split(',');
            splitted.ToList().ForEach(x => result.Add(UppercaseFirst(x.Trim())));

            return result;
        }

        private static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static PossibilityMLModel GetMlModel(GlobalPreferences preferences)
        {
            var result = new PossibilityMLModel();
            result.TerritorialLocation = (int)preferences.InitialSecurityLevel.TerritorialLocation;
            result.NetworkCharacteristic = (int)preferences.InitialSecurityLevel.NetworkCharacteristic;
            result.PersonalDataActionCharacteristics = (int)preferences.InitialSecurityLevel.PersonalDataActionCharacteristics;
            result.PersonalDataPermissionSplit = (int)preferences.InitialSecurityLevel.PersonalDataPermissionSplit;
            result.OtherDbConnections = (int)preferences.InitialSecurityLevel.OtherDbConnections;
            result.AnonymityLevel = (int)preferences.InitialSecurityLevel.AnonymityLevel;
            result.PersonalDataSharingLevel = (int)preferences.InitialSecurityLevel.PersonalDataSharingLevel;
            result.Privacy = (int)preferences.InitialSecurityLevel.PrivacyViolationDanger;
            result.Availability = (int)preferences.InitialSecurityLevel.AvailabilityViolationDanger;
            result.Integrity = (int)preferences.InitialSecurityLevel.IntegrityViolationDanger;

            result.Sources = preferences.Source
                .OrderBy(x => x.Item1)
                .Select(x => x.Item2 ? 1f : 0f)
                .ToArray();

            result.Targets = preferences.Targets
                .OrderBy(x => x.Item1)
                .Select(x => x.Item2 ? 1f : 0f)
                .ToArray();

            return result;
        }

        public static RiskProbabilities ResolvePossibility(this string s)
        {
            switch (s)
            {
                case "Низкая":
                    return RiskProbabilities.Low;
                    break;
                case "Средняя":
                    return RiskProbabilities.Medium;
                    break;
                case "Высокая":
                    return RiskProbabilities.High;
                    break;
                default:
                    return RiskProbabilities.Unlikely;
            }
        }
    }
}