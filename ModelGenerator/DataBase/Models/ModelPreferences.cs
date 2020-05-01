using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ModelGenerator.DataBase.Models.Enums;

namespace ModelGenerator.DataBase.Models
{
    public class ModelPreferences
    {
        [Key]
        public Guid Id { get; set; }

        public List<ModelPreferencesTarget> ModelPreferencesTarget { get; set; } = new List<ModelPreferencesTarget>();

        public List<ModelPreferencesSource> ModelPreferencesSource { get; set; } = new List<ModelPreferencesSource>();

        public ICollection<ThreatPossibility> ThreatPossibilities { get; set; }

        public ICollection<ThreatDanger> ThreatDangers { get; set; }

        public AnonymityLevel AnonymityLevel { get; set; }

        public LocationCharacteristic LocationCharacteristic { get; set; }

        public NetworkCharacteristic NetworkCharacteristic { get; set; }

        public OtherDBConnections OtherDBConnections { get; set; }

        public PersonalDataActionCharacteristics PersonalDataActionCharacteristics { get; set; }

        public PersonalDataPermissionSplit PersonalDataPermissionSplit { get; set; }

        public PersonalDataSharingLevel PersonalDataSharingLevel { get; set; }

        public DangerLevel PrivacyViolationDanger { get; set; }

        public DangerLevel IntegrityViolationDanger { get; set; }

        public DangerLevel AvailabilityViolationDanger { get; set; }
    }
}