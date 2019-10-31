using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using vue.Areas.Identity.Data;
using vue.Auth;
using vue.IService;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
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

        /// <summary>
        /// 获取权限Tree
        /// 2019-10-31 09:25:25
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Claim_Get")]
        public async Task<IActionResult> GetClaimTree([FromBody] IdentityRole role)
        {
            //所有子节点
            var ChildrenClaims = _claim.ClaimList.Select(a => new TreeViewModel()
            {
                Id = a.Id,
                Type = a.ClaimType,
                Value = a.ClaimValue,
                Label = a.Label,
            });
            //这是父+子总的Tree
            var ClaimTree = new List<TreeViewModel>() { //所有的父节点,直接写死，不从数据库走一道了
                new TreeViewModel{Id=101,Type="Home",Label="首页"},
                new TreeViewModel{Id=201,Type="Staff",Label="员工管理"},
                new TreeViewModel{Id=301,Type="MyInfo",Label="个人信息"},
                new TreeViewModel{Id=401,Type="Colleague",Label="查找同事"},
                new TreeViewModel{Id=501,Type="Claim",Label="权限管理"},
                //new TreeViewModel{Id=601,Type="Salary",Label="薪资管理"},
                new TreeViewModel{Id=701,Type="ApplyLeave",Label="申请请假"},
                new TreeViewModel{Id=801,Type="CheckLeave",Label="审核请假"},
                new TreeViewModel{Id=901,Type="AllLeave",Label="请假记录"},
                new TreeViewModel{Id=1001,Type="Signin",Label="签到"},
                new TreeViewModel{Id=1101,Type="CheckAttendance",Label="查看考勤记录"},
                new TreeViewModel{Id=1201,Type="Department",Label="部门管理"}
            }.Select(x => new TreeViewModel()
            {
                Id = x.Id,
                Label = x.Label,
                Children = ChildrenClaims.Where(z => z.Type == x.Type).Select(a => new TreeViewModel()
                {
                    Id = a.Id,
                    Type = a.Type,
                    Value = a.Value,
                    Label = a.Label,
                    RoleId = role.Id,//把角色名带回去，编辑的时候传回来用
                })
            });
            //这是角色的权限
            IList<Claim> ClaimResult = await _roleManager.GetClaimsAsync(role);
            //对比两者取交集
            List<int> Result = new List<int>();
            foreach (var All in ChildrenClaims)
            {
                foreach (var Claim in ClaimResult)
                {
                    if (All.Type.Equals(Claim.Type) && All.Value.Equals(Claim.Value))
                    {
                        Result.Add(All.Id);
                    }
                }
            }
            return Ok(new
            {
                code = codes.Success,
                data = new
                {
                    ClaimTree,//所有的声明表
                    Result//角色所拥有的声明
                }
            });
        }


        /// <summary>
        /// 获取角色列表
        /// 不分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Claim_Get")]
        public ReturnViewModel<IEnumerable<RolesViewModel>> GetRoles() => new ReturnViewModel<IEnumerable<RolesViewModel>>()
        {
            code = (int)codes.Success,
            data = _claim.Roles
        };


        /// <summary>
        /// 编辑角色的声明
        /// 2019-10-31 09:24:53
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Claim_Set")]
        public async Task<ReturnViewModel<bool>> SetRoleClaim([FromBody] List<ClaimsViewModel> claims)
        {
            if (!claims.Any())
            {
                return new ReturnViewModel<bool>()
                {
                    code = (int)codes.ChangeClaimError,
                    data = false,
                    message = "编辑失败"
                };
            }
            try
            {
                IdentityRole role = await _roleManager.FindByIdAsync(claims.Where(x => x.RoleId != null).First().RoleId);
                //先取出来
                var DbClaims = await _roleManager.GetClaimsAsync(role);
                //把原有的全部删掉
                foreach (Claim item in DbClaims)
                {
                    var result = await _roleManager.RemoveClaimAsync(role, item);

                }
                //把更改后的全部添加进去
                foreach (Claim item in claims.Where(x => x.RoleId != null).Select(x => new Claim(type: x.Type, value: x.Value)))
                {
                    var result = await _roleManager.AddClaimAsync(role, item);

                }
            }
            catch (System.Exception)
            {
                return new ReturnViewModel<bool>()
                {
                    code = (int)codes.ChangeClaimError,
                    data = false,
                    message = "编辑失败"
                };
            }
            return new ReturnViewModel<bool>()
            {
                code = (int)codes.Success,
                data = true,
                message = "编辑成功"
            };
        }

        /// <summary>
        /// 获取拥有此角色的用户列表-Transfer
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Claim_Get")]

        public async Task<IActionResult> GetUsersInRole([FromBody] NAndOIentityRoleViewModel role)
        {
            IList<NewUser> thisRoleUser = await _userManager.GetUsersInRoleAsync(role.RightIdentityRole.Name);//当前需要分配用户的角色所查的用户表
            IList<NewUser> staffRoleUser = await _userManager.GetUsersInRoleAsync(role.LeftIdentityRole == null ? "Staff" : role.LeftIdentityRole.Name);
            return Ok(new
            {
                code = codes.Success,
                data = staffRoleUser.Union(thisRoleUser).Select(x => new { label = x.UserName, key = x.Id }),
                value = thisRoleUser.Select(x => x.Id)
            });
        }


        /// <summary>
        /// 分配角色
        /// </summary>
        /// <param name="transfer"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Claim_SetRole")]
        public async Task<ReturnViewModel<IActionResult>> SetRoleUsers([FromBody]TransferViewModel transfer)
        {
            try
            {
                if ("right".Equals(transfer.Direction))
                {
                    foreach (var item in transfer.MovedKeys)
                    {
                        //先根据id把用户都查出来
                        NewUser aUser = await _userManager.FindByIdAsync(item);
                        if (aUser != null)
                        {
                            //不管原来是什么角色都先移除
                            var removeResult = await _userManager.RemoveFromRoleAsync(aUser, transfer.Roles.LeftIdentityRole.NormalizedName == null ? "Staff" : transfer.Roles.LeftIdentityRole.Name);
                            if (removeResult.Succeeded)
                            {
                                //左边是空的话那就是Staff
                                var addResult = await _userManager.AddToRoleAsync(aUser, transfer.Roles.RightIdentityRole.Name);
                            }
                        }
                    }
                }
                else if ("left".Equals(transfer.Direction))
                {
                    foreach (var item in transfer.MovedKeys)
                    {
                        NewUser aUser = await _userManager.FindByIdAsync(item);
                        if (aUser != null)
                        {
                            var removeResult = await _userManager.RemoveFromRoleAsync(aUser, transfer.Roles.RightIdentityRole.Name);
                            if (removeResult.Succeeded)
                            {
                                var addResult = await _userManager.AddToRoleAsync(aUser, transfer.Roles.LeftIdentityRole.NormalizedName == null ? "Staff" : transfer.Roles.LeftIdentityRole.Name);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                return new ReturnViewModel<IActionResult>()
                {
                    code = (int)codes.SetRoleUsersError,
                    message = $"修改失败({e.Message})"
                };
            }
            return new ReturnViewModel<IActionResult>()
            {
                code = (int)codes.Success,
                message = "修改成功"
            };
        }


        public string GetIdByToken() => JwtHelper.SerializeJwt(Request.Headers["Authorization"].ToString().Substring(7)).ID;
    }
}

