using System;
using System.Collections.Generic;

namespace vue.DBModel
{
    public partial class Claims
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string Label { get; set; }
    }
}
