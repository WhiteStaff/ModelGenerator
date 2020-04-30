using System;
using ModelGenerator.DataBase.Models;
using ThreatsParser.Entities;

namespace TreatsParser.Core
{
    public static class Extensions
    {
        public static Threat ToDbModel(this ThreatModel model)
        {
            return new Threat
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                ThreatId = model.Id,
                IsHasAvailabilityViolation = model.IsHasAvailabilityViolation,
                IsHasIntegrityViolation = model.IsHasIntegrityViolation,
                IsHasPrivacyViolation = model.IsHasPrivacyViolation
            };
        }
        
        public static Source ToDbSourceModel(this string source)
        {
            return new Source()
            {
                Id = Guid.NewGuid(),
                Name = source
            };
        }

        public static Target ToDbTargetModel(this string target)
        {
            return new Target()
            {
                Id = Guid.NewGuid(),
                Name = target
            };
        }
    }
}