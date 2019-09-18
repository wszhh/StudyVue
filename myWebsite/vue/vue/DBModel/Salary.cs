using System;
using System.Collections.Generic;

namespace vue.DBModel
{
    public partial class Salary
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? AttendanceBonus { get; set; }
        public decimal? Fine { get; set; }
        public DateTime? SalaryTime { get; set; }
        public decimal? SalarySum { get; set; }
    }
}
