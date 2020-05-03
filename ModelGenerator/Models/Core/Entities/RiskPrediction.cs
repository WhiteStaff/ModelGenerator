using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.ML.Data;

namespace ModelGenerator.Models.Core.Entities
{
    public class RiskPrediction
    {
        [ColumnName("Score")]
        public float Risk { get; set; }
    }
}