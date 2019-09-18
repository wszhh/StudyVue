using System;
using System.Collections.Generic;

namespace vue.DBModel
{
    public partial class SystemLog
    {
        public int LogId { get; set; }
        public string LogName { get; set; }
        public DateTime? LogTime { get; set; }
        public string LogOperation { get; set; }
    }
}
