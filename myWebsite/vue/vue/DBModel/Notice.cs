using System;
using System.Collections.Generic;

namespace vue.DBModel
{
    public partial class Notice
    {
        public int NoticeId { get; set; }
        public int? NoticeType { get; set; }
        public string NoticeTitle { get; set; }
        public string NoticeContent { get; set; }
        public int? UserId { get; set; }
        public DateTime? NoticeStateTime { get; set; }
        public DateTime? NoticeEndTime { get; set; }
        public int? NoticeState { get; set; }
    }
}
