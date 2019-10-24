using System.Collections.Generic;
using vue.DBModel;
using vue.ViewModel;

namespace vue.IService
{
    public interface ILeave
    {
        /// <summary>
        /// 基于用户id获取请假表
        /// </summary>
        /// <returns></returns>
        ReturnViewModel<IEnumerable<Leave>> GetLeaveByUserId(string id);

        /// <summary>
        /// 获取请假表
        /// </summary>
        /// <returns></returns>
        ReturnViewModel<IEnumerable<Leave>> GetLeaves();

        /// <summary>
        /// 获取审批表
        /// </summary>
        /// <returns></returns>
        ReturnViewModel<IEnumerable<Leave>> GetCheckLeaves();
    }
}
