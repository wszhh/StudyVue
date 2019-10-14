using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace vue.ViewModel
{
    public class TransferViewModel
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// 变动的值
        /// </summary>
        public List<string> MovedKeys { get; set; }

        public NAndOIentityRoleViewModel Roles { get; set; }
    }
}
