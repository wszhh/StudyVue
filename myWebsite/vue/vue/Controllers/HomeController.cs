using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModel;
using vue.DBModel;
using vue.IService;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.Controllers
{
    /// <summary>
    /// 主页控制器
    /// </summary>
    //[EnableCors("AllowSameDomain")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : Controller
    {

        private readonly IStuList _stuList;
        public HomeController(IStuList stuList)
        {
            _stuList = stuList;
        }


        [Authorize(Policy = "Admin")]
        [HttpGet]
        public ReturnCMDViewModel<string> GetTime()
        {
            return new ReturnCMDViewModel<string>()
            {
                code = (int)codes.Success,
                data = DateTime.Now.ToString(),
            };
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult GetStuList([FromBody]PaginationRequestViewModel pagination)
        {
            var result = _stuList.GetList(pagination);
            return Ok(new
            {
                code = codes.Success,
                data = new
                {
                    result.list,
                    result.total,
                },
            });
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult EditStuList([FromBody]StuList stuinfo)
        {

            return Ok(new { code = codes.Success, stuinfo });
        }
    }
}