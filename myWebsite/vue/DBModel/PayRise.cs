using System;
using System.Collections.Generic;

namespace vue.DBModel
{
    public partial class PayRise
    {
        public int PayRiseId { get; set; }
        public int? UserId { get; set; }
        public decimal? PayRiseMoney { get; set; }
        public string Reason { get; set; }
        public DateTime? ApplicationTime { get; set; }
        public string ApprovalContent { get; set; }
        public int? ApprovalState { get; set; }
        public DateTime? ApprovalTime { get; set; }
    }
}
