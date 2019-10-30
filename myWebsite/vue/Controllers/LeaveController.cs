using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewModel;
using vue.Areas.Identity.Data;
using vue.Auth;
using vue.DBModel;
using vue.IService;
using vue.Migrations;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LeaveController : Controller
    {
        private readonly UserManager<NewUser> _userManager;
        private readonly ICategoryItems _categoryItems;
        private readonly ILeave _leave;
        public LeaveController(UserManager<NewUser> userManager, ILeave leave, ICategoryItems categoryItems)
        {
            _userManager = userManager;
            _leave = leave;
            _categoryItems = categoryItems;
        }

        /// <summary>
        /// 申请请假-申请请假
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "ApplyLeave_Add")]
        public async Task<ReturnViewModel<bool>> AddLeave([FromBody]LeaveViewModel leave)
        {
            return _leave.AddLeave(leave, await _userManager.FindByIdAsync(GetIdByToken()));
        }

        /// <summary>
        /// 申请请假-基于用户id获取请假表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "ApplyLeave_Get")]
        public ReturnViewModel<PaginationResponeViewModel<IEnumerable<Leave>>> GetLeavesById([FromBody]PaginationRequestViewModel pagination)
        {
            return _leave.GetLeavesById(pagination, GetIdByToken());
        }

        /// <summary>
        /// 请假审核-审批请假
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "CheckLeave_Set")]
        public async Task<ReturnViewModel<bool>> CheckLeave([FromBody]Leave leave)
        {
            return _leave.CheckLeave(leave, await _userManager.FindByIdAsync(GetIdByToken()));
        }

        /// <summary>
        /// 请假审核-获取待审批表
        /// 2019年10月24日 10时05分23秒
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "CheckLeave_Get")]
        public ReturnViewModel<PaginationResponeViewModel<IEnumerable<Leave>>> GetCheckLeaves(PaginationRequestViewModel pagination)
        {
            return _leave.GetCheckLeaves(pagination);
        }

        /// <summary>
        /// 请假记录-获取请假表
        /// 2019年10月24日 10时05分44秒
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "AllLeave_Get")]
        public ReturnViewModel<PaginationResponeViewModel<IEnumerable<Leave>>> GetLeaves([FromBody]PaginationRequestViewModel pagination)
        {
            return _leave.GetLeaves(pagination);
        }

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