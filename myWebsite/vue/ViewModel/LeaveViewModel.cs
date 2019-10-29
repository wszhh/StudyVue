using System;
using System.Collections.Generic;

namespace vue.ViewModel
{
    public class LeaveViewModel
    {
        public string LeaveHalfDay { get; set; }
        public string LeaveReason { get; set; }
        public DateTime LeaveTime { get; set; }
        public List<string> LeaveTimeSpan { get; set; }
    }
}
