﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelGenerator.DataBase.Models.Enums;
using TreatsParser.Core.Helpers;

namespace ThreatsParser.Entities
{
    public class Threat
    {
        public int Id { get; }

        public string Name { get; }

        public string Description { get; }

        public List<string> Source { get; }

        public List<string> ExposureSubject { get; }

        public bool IsHasPrivacyViolation { get; }

        public bool IsHasIntegrityViolation { get; }

        public bool IsHasAvailabilityViolation { get; }

        public RiskProbabilities RiskProbabilities { get; set; }
        

        public string GetPossibility
        {
            get
            {
                switch (RiskProbabilities)
                {
                    case RiskProbabilities.Unlikely:
                        return "Маловероятно";
                    case RiskProbabilities.Low:
                        return "Низкая";
                    case RiskProbabilities.Medium:
                        return "Средняя";
                    default:
                        return "Очень высокая";
                }
            }
        }

        public string Properties
        {
            get
            {
                var result = new List<string>();
                if (IsHasPrivacyViolation)
                {
                    result.Add("Нарушение конфиденциальности");
                }

                if (IsHasIntegrityViolation)
                {
                    result.Add("Нарушение целостности");
                }

                if (IsHasAvailabilityViolation)
                {
                    result.Add("Нарушение доступности");
                }

                return result.Count > 0 ? string.Join(", ", result) : "Нарушения отсуствуют";
            }
        }

        public Threat(string[] rowValues)
        {
            Id = int.Parse(rowValues[0]);
            Name = rowValues[1].Replace("_x000d_", "\n");
            Description = rowValues[2].Replace("_x000d_", "\n");
            Source = Resolver.ResolveSourceType(rowValues[3].Replace("_x000d_", "\n"));
            ExposureSubject = Resolver.ResolveTargetsType(rowValues[4].Replace("_x000d_", "\n"));
            IsHasPrivacyViolation = rowValues[5].Equals("1");
            IsHasIntegrityViolation = rowValues[6].Equals("1");
            IsHasAvailabilityViolation = rowValues[7].Equals("1");
        }

        public string[] GetValuesAsArray()
        {
            var privacy = (IsHasPrivacyViolation
                ? "Присутствует"
                : "Отсутствует");
            var integrity = (IsHasIntegrityViolation
                ? "Присутствует"
                : "Отсутствует");
            var available = (IsHasAvailabilityViolation
                ? "Присутствует"
                : "Отсутствует");
            return new[]
            {
                $"УБИ.{Id}", $"{Name}", $"{Description}", $"{Source}", $"{ExposureSubject}", privacy, integrity,
                available
            };
        }

        public override string ToString()
        {
            var x = new StringBuilder();
            x.AppendLine($"Идентификатор угрозы: УБИ.{Id}");
            x.AppendLine();
            x.AppendLine($"Название угрозы: {Name}");
            x.AppendLine();
            x.AppendLine($"Описание: {Description}");
            x.AppendLine();
            x.AppendLine($"Источник: {string.Join(", ", Source)}");
            x.AppendLine();
            x.AppendLine($"Объект воздействия: {string.Join(", ", ExposureSubject)}");
            x.AppendLine();
            x.AppendLine(IsHasPrivacyViolation
                ? "Нарушение конфиденциальности присутствует"
                : "Нарушение конфиденциальности отсутствует");
            x.AppendLine();
            x.AppendLine(IsHasIntegrityViolation
                ? "Нарушение целостности присутствует"
                : "Нарушение целостности отсутствует");
            x.AppendLine();
            x.AppendLine(IsHasAvailabilityViolation
                ? "Нарушение доступности присутствует"
                : "Нарушение доступности отсутствует");
            return x.ToString();
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Threat threat)) return false;
            var x1 = Id == threat.Id;
            var x2 = Name.Replace("\r", "").Replace("\n", "") == threat.Name.Replace("\r", "").Replace("\n", "");
            var x3 = Description.Replace("\r", "").Replace("\n", "") ==
                     threat.Description.Replace("\r", "").Replace("\n", "");
            var x4 = Source.All(x => threat.Source.Contains(x));
            var x5 = ExposureSubject.All(x => threat.ExposureSubject.Contains(x));
            var x6 = IsHasPrivacyViolation == threat.IsHasPrivacyViolation;
            var x7 = IsHasAvailabilityViolation == threat.IsHasAvailabilityViolation;
            var x8 = IsHasIntegrityViolation == threat.IsHasIntegrityViolation;
            return x1 && x2 && x3 && x4 && x5 && x6 && x7 && x8;
        }
    }
}