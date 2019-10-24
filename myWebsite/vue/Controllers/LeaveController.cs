using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using vue.Auth;
using vue.DBModel;
using vue.IService;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LeaveController : Controller
    {
        private readonly ICategoryItems _categoryItems;
        private readonly ILeave _leave;
        public LeaveController(ILeave leave, ICategoryItems categoryItems)
        {
            _leave = leave;
            _categoryItems = categoryItems;
        }

        /// <summary>
        /// 基于用户id获取请假表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnViewModel<IEnumerable<Leave>> GetLeavesById() => _leave.GetLeaveByUserId(GetIdByToken());

        /// <summary>
        /// 申请请假
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnViewModel<IEnumerable<Leave>> AddLeave(Leave leave)
        {
            return null;
        }

        /// <summary>
        /// 获取审批表
        /// 2019年10月24日 10时05分23秒
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnViewModel<IEnumerable<Leave>> GetCheckLeaves() => _leave.GetCheckLeaves();

        /// <summary>
        /// 审批请假
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnViewModel<IEnumerable<Leave>> CheckLeave(Leave leave)
        {
            return null;
        }

        /// <summary>
        /// 获取请假表
        /// 2019年10月24日 10时05分44秒
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnViewModel<IEnumerable<Leave>> GetLeaves() => _leave.GetLeaves();

        /// <summary>
        /// 获取LeaveStart的Category
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnViewModel<IEnumerable<CategoryItems>> GetCategory() => _categoryItems.GetCategoryByName("LeaveStart");

        /// <summary>
        /// 返回Token
        /// </summary>
        /// <returns></returns>
        public string GetIdByToken() => JwtHelper.SerializeJwt(Request.Headers["Authorization"].ToString().Substring(7)).ID;
    }
}