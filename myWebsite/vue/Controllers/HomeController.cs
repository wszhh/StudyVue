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
    /// 主页
    /// </summary>
    [Route("api/[controller]/[action]")]
    //[Authorize(Policy = "CEO")]
    [ApiController]
    public class HomeController : Controller
    {



        [Authorize(Policy = "Home_Get")]
        [HttpGet]
        public ReturnViewModel<string> GetTime()
        {

            return new ReturnViewModel<string>()
            {
                code = (int)codes.Success,
                data = DateTime.Now.ToString()
            };
        }

    }
}