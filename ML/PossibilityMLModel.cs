using Microsoft.ML.Data;

namespace ML
{
    public class PossibilityMLModel
    {
        [LoadColumn(0)]
        public float TerritorialLocation { get; set; }

        [LoadColumn(1)]
        public float NetworkCharacteristic { get; set; }

        [LoadColumn(2)]
        public float PersonalDataActionCharacteristics { get; set; }

        [LoadColumn(3)]
        public float PersonalDataPermissionSplit { get; set; }

        [LoadColumn(4)]
        public float OtherDbConnections { get; set; }

        [LoadColumn(5)]
        public float AnonymityLevel { get; set; }

        [LoadColumn(6)]
        public float PersonalDataSharingLevel { get; set; }

        [LoadColumn(7, 13)]
        [VectorType(7)]
        public float[] Sources { get; set; }

        [LoadColumn(14)]
        public float Privacy { get; set; }

        [LoadColumn(15)]
        public float Availability { get; set; }

        [LoadColumn(16)]
        public float Integrity { get; set; }

        [LoadColumn(17, 100)]
        [VectorType(84)]
        public float[] Targets { get; set; }

        [LoadColumn(101)]
        public float ThreatName { get; set; }

        [LoadColumn(102)]
        public float Source { get; set; }

        [LoadColumn(103)]
        [ColumnName("Label")]
        public float Danger { get; set; }
    }
}