using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vue.DBModel;
using vue.IService;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.Controllers
{
    /// <summary>
    /// 请假表
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LeaveController : Controller
    {
        private readonly ILeave _leave;
        public LeaveController(ILeave leave)
        {
            this._leave = leave;
        }

        [HttpGet]
        [Authorize(Policy = "CEO")]
        public ReturnViewModel<List<Leave>> GetLeaves()
        {
            return new ReturnViewModel<List<Leave>>()
            {
                code = (int)codes.Success,
                data = _leave.GetLeave(),
            };
        }
    }
}