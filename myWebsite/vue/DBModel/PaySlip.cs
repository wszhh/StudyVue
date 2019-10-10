using System;
using System.Collections.Generic;

namespace vue.DBModel
{
    public partial class PaySlip
    {
        public int PaySlipId { get; set; }
        public int? UserId { get; set; }
        public decimal? Prize { get; set; }
        public decimal? LeaveMoney { get; set; }
        public decimal? OvertimeMoney { get; set; }
        public decimal? LateMoney { get; set; }
        public decimal? AdvanceMoney { get; set; }
        public decimal? Absence { get; set; }
        public decimal? Fine { get; set; }
        public decimal? SaBonus { get; set; }
        public DateTime? SaTime { get; set; }
        public decimal? SaTotalSalary { get; set; }
    }
}
