using System.Collections.Generic;
using ViewModel;
using vue.Areas.Identity.Data;
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
        ReturnViewModel<PaginationResponeViewModel<IEnumerable<Leave>>> GetLeavesById(PaginationRequestViewModel pagination, string id);

        /// <summary>
        /// 获取请假表
        /// </summary>
        /// <returns></returns>
        ReturnViewModel<PaginationResponeViewModel<IEnumerable<Leave>>> GetLeaves(PaginationRequestViewModel pagination);

        /// <summary>
        /// 获取审批表
        /// </summary>
        /// <returns></returns>
        ReturnViewModel<PaginationResponeViewModel<IEnumerable<Leave>>> GetCheckLeaves(PaginationRequestViewModel pagination);

        /// <summary>
        /// 申请请假
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        ReturnViewModel<bool> AddLeave(LeaveViewModel leave, NewUser user);

        /// <summary>
        /// 审批请假
        /// </summary>
        /// <param name="leave"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        ReturnViewModel<bool> CheckLeave(Leave leave, NewUser user);
    }
}
