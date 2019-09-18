using System;
using System.Collections.Generic;

namespace vue.DBModel
{
    public partial class UserInfo
    {
        public int UserId { get; set; }
        public int? DepartmentId { get; set; }
        public int? RoleId { get; set; }
        public string UserNumber { get; set; }
        public string LoginName { get; set; }
        public string LoginPwd { get; set; }
        public string UserName { get; set; }
        public int? UserAge { get; set; }
        public int? UserSex { get; set; }
        public string UserTel { get; set; }
        public string UserAddress { get; set; }
        public string UserIphone { get; set; }
        public string UserRemarks { get; set; }
        public int? UserStatr { get; set; }
        public DateTime? EntryTime { get; set; }
        public DateTime? DimissionTime { get; set; }
        public decimal? BasePay { get; set; }
    }
}
