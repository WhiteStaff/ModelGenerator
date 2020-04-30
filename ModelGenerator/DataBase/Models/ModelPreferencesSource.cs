using System;
using System.ComponentModel.DataAnnotations;

namespace ModelGenerator.DataBase.Models
{
    public class ModelPreferencesSource
    {
        public Guid SourceId { get; set; }

        public Source Source { get; set; }

        public Guid ModelPreferencesId { get; set; }

        public ModelPreferences ModelPreferences { get; set; }
    }
}