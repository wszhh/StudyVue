using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ViewModel;
using vue.DBModel;
using vue.IService;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController : Controller
    {

        private IHubContext<SignalrHubs> _hubContext;

        public HomeController(IHubContext<SignalrHubs> hubContext)
        {
            _hubContext = hubContext;
        }

        [Authorize(Policy = "Home_Get")]
        [HttpGet]
        public IActionResult GetTime() => Ok(new
        {
            code = 20000,
            data = DateTime.Now.ToString()

        });


        /// <summary>
        /// 单个connectionid推送
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AnyOne(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "服务器", message);
            return Ok(new
            {
                code = 20000
            });
        }

    }
}