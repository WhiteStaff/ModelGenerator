using System;
using System.ComponentModel.DataAnnotations;

namespace ModelGenerator.DataBase.Models
{
    public class ModelPreferencesTarget
    {
        public Guid TargetId { get; set; }

        public Target Target { get; set; }

        public Guid ModelPreferencesId { get; set; }

        public ModelPreferences ModelPreferences { get; set; }
    }
}