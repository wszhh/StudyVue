using System;
using System.Collections.Generic;

namespace vue.DBModel
{
    public partial class AttendanceSheet
    {
        public int AttendanceId { get; set; }
        public DateTime? AttendanceStartTime { get; set; }
        public int? AttendanceType { get; set; }
        public int? UserId { get; set; }
        public int? DepartmentId { get; set; }
        public string RealName { get; set; }
        public DateTime? ClockTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public int? Workinghours { get; set; }
        public string Remake { get; set; }
        public int? Late { get; set; }
        public int? Absenteeism { get; set; }
    }
}
