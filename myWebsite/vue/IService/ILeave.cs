using System.Collections.Generic;
using vue.DBModel;

namespace vue.IService
{
    public interface ILeave
    {
        /// <summary>
        /// 获取请假表
        /// </summary>
        /// <returns></returns>
        List<Leave> GetLeave();
    }
}
