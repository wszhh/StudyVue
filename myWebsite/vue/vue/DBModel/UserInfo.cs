using System;
using System.Collections.Generic;

namespace vue.DBModel
{
    public partial class UserInfo
    {
        public int UserId { get; set; }
        public int? DepartmentId { get; set; }
        public string UserNumber { get; set; }
        public string RealName { get; set; }
        public DateTime? UserBirthday { get; set; }
        public int? UserSex { get; set; }
        public string UserTel { get; set; }
        public string UserAddress { get; set; }
        public string Id { get; set; }
        public string UserRemarks { get; set; }
        public int? UserStatr { get; set; }
        public DateTime? EntryTime { get; set; }
        public DateTime? DimissionTime { get; set; }
        public decimal? BasePay { get; set; }
        public string Avatar { get; set; }
    }
}
