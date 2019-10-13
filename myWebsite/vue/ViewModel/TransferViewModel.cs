using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vue.ViewModel
{
    public class TransferViewModel
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public string direction { get; set; }
        /// <summary>
        /// 变动的值
        /// </summary>
        public List<string> movedKeys { get; set; }
    }
}
