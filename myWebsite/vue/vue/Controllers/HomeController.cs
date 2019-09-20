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
    [Authorize(Policy = "CEO")]
    [ApiController]
    public class HomeController : Controller
    {

        private readonly IStuList _stuList;
        public HomeController(IStuList stuList)
        {
            _stuList = stuList;
        }


        [HttpGet]
        public ReturnCMDViewModel<string> GetTime()
        {
            return new ReturnCMDViewModel<string>()
            {
                code = (int)codes.Success,
                data = DateTime.Now.ToString(),
            };
        }

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

        [HttpPost]
        public IActionResult EditStuList([FromBody]StuList stuinfo)
        {

            return Ok(new { code = codes.Success, stuinfo });
        }
    }
}