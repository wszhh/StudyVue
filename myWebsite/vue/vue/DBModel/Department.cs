using System;
using System.Collections.Generic;

namespace vue.DBModel
{
    public partial class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? People { get; set; }
    }
}
