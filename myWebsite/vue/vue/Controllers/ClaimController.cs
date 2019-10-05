using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using vue.Areas.Identity.Data;
using vue.IService;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IClaim _claim;
        private readonly UserManager<NewUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ClaimController(IClaim claim, UserManager<NewUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _claim = claim;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> GetClaimTree()
        {
            List<TreeViewModel> Title = new List<TreeViewModel>() {
                new TreeViewModel{id=101,type="Home",label="首页"},
                new TreeViewModel{id=201,type="Staff",label="员工管理"},
                new TreeViewModel{id=301,type="UserInfo",label="个人信息"},
                new TreeViewModel{id=401,type="Colleague",label="查找同事"},
                new TreeViewModel{id=701,type="Department",label="部门管理"}
            };
            //这是前台实现的总的权限表 Fake
            var data = Title.Select(x => new TreeViewModel()
            {
                id = x.id,
                label = x.label,
                children = _claim.GetClaimList().Where(z => z.ClaimType == x.type).Select(a => new TreeViewModel()
                {
                    id = a.Id,
                    type = a.ClaimType,
                    value = a.ClaimValue,
                    label = a.Label
                })
            });
            //总表Real
            var table = _claim.GetClaimList().Select(a => new TreeViewModel()
            {
                id = a.Id,
                type = a.ClaimType,
                value = a.ClaimValue,
                label = a.Label
            });


            //这是角色的权限 Real
            IList<Claim> ClaimResult = await _roleManager.GetClaimsAsync(await _roleManager.FindByNameAsync("Ceo"));
            //对比两者取交集
            List<int> Result = new List<int>();

            foreach (var All in table)
            {
                foreach (var Claim in ClaimResult)
                {
                    if (All.type.Equals(Claim.Type) && All.value.Equals(Claim.Value))
                    {
                        Result.Add(All.id);
                    }

                }
            }
            //var data = new List<TreeViewModel>() {
            //    new TreeViewModel{id="1",label="一"},
            //    new TreeViewModel{id="2",label="二",children=new List<TreeViewModel>(){

            //    new TreeViewModel{id="2.1",label="二电一"},

            //    } },
            //    new TreeViewModel{id="3",label="三"},
            //};


            return Ok(new
            {
                code = codes.Success,
                data = new
                {
                    All = data,
                    Result
                }

            }); ;
        }


        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ReturnCMDViewModel<IEnumerable<RolesViewModel>> GetRoles() => new ReturnCMDViewModel<IEnumerable<RolesViewModel>>()
        {
            code = (int)codes.Success,
            data = _claim.Roles
        };

    }
}

