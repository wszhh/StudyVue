using System;
using System.Collections.Generic;

namespace vue.DBModel
{
    public partial class Leave
    {
        public int LeaveId { get; set; }
        public int? UserId { get; set; }
        public int? LeaveState { get; set; }
        public DateTime? LeaveTime { get; set; }
        public DateTime? LeaveStartTime { get; set; }
        public DateTime? LeaveEndTime { get; set; }
        public string LeaveHalfDay { get; set; }
        public int? LeaveDays { get; set; }
        public string LeaveReason { get; set; }
        public int? ApproverId { get; set; }
        public DateTime? ApprovalTime { get; set; }
        public string ApproverReason { get; set; }
    }
}
