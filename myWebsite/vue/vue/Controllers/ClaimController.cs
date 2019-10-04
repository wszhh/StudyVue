using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vue.ViewModel;

namespace vue.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {

        [HttpPost]
        public IActionResult GetClaimTree()
        {
            var result = db.AspNetRoleClaims.Select(x => new List<TreeViewModel>() {
                new TreeViewModel{id=x.ClaimValue ,label=$"{x.ClaimType+"_"+x.ClaimValue}" }
            });
        }
    }
}