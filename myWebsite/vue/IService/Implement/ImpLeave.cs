using System.Collections.Generic;
using System.Linq;
using vue.DBModel;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.IService.Implement
{
    public class ImpLeave : ILeave
    {
        private HRCContext db = new HRCContext();

        public ReturnViewModel<IEnumerable<Leave>> GetCheckLeaves() => new ReturnViewModel<IEnumerable<Leave>>
        {
            code = (int)codes.Success,
            data = db.Leave.Where(x => x.LeaveState == 3)
        };

        /// <summary>
        /// 基于用户id获取请假表
        /// </summary>
        /// <returns></returns>
        public ReturnViewModel<IEnumerable<Leave>> GetLeaveByUserId(string id) => new ReturnViewModel<IEnumerable<Leave>>
        {
            code = (int)codes.Success,
            data = db.Leave.Where(x => x.UserId == id)
        };

        /// <summary>
        /// 获取请假表
        /// </summary>
        /// <returns></returns>
        public ReturnViewModel<IEnumerable<Leave>> GetLeaves() => new ReturnViewModel<IEnumerable<Leave>>
        {
            code = (int)codes.Success,
            data = db.Leave
        };

        /// <summary>
        /// 获取签到的CategoryItem
        /// </summary>
        /// <returns></returns>
        public ReturnViewModel<IEnumerable<CategoryItems>> GetLeaveStartCategory() => new ReturnViewModel<IEnumerable<CategoryItems>>
        {
            code = (int)codes.Success,
            data = db.CategoryItems.Where(x => x.CCategory == "LeaveStart"),
        };

    }
}
