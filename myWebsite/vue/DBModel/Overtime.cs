using System;
using System.Collections.Generic;

namespace vue.DBModel
{
    public partial class Overtime
    {
        public int OvertimeId { get; set; }
        public DateTime? OvertimeStateTime { get; set; }
        public DateTime? OvertimeEndTime { get; set; }
        public int? OvertimeDuration { get; set; }
        public int? UserId { get; set; }
        public DateTime? ApplyTime { get; set; }
        public int? OvertimeState { get; set; }
        public string ApproverReason { get; set; }
    }
}
