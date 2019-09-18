using System;
using System.Collections.Generic;

namespace vue.DBModel
{
    public partial class Assessment
    {
        public int AssessmentId { get; set; }
        public DateTime? PerformanceTime { get; set; }
        public int? UserId { get; set; }
        public string WorkSummary { get; set; }
        public string UpperGoal { get; set; }
        public double? CompletionDegree { get; set; }
        public string ExaminationItems { get; set; }
        public string NextStageObjectives { get; set; }
        public double? PerformanceScore { get; set; }
        public string Comments { get; set; }
        public int? Perstate { get; set; }
    }
}
