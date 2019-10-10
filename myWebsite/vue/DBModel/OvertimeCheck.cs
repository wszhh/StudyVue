using System;
using System.Collections.Generic;

namespace vue.DBModel
{
    public partial class OvertimeCheck
    {
        public int ApprovalId { get; set; }
        public int? LeaveId { get; set; }
        public int? UserId { get; set; }
        public int? ApprovalType { get; set; }
        public int? DepartmentalAudit { get; set; }
        public string DepartmentalAuditRemarks { get; set; }
        public int? ManagerAudit { get; set; }
        public string ManagerAuditRemarks { get; set; }
        public int? GeneralManagerAudit { get; set; }
        public string GeneralManagerAuditRemarks { get; set; }
    }
}
