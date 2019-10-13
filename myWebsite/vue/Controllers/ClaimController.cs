﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Claim_Get")]
        public async Task<IActionResult> GetClaimTree([FromBody] IdentityRole role)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            //所有子结点
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
                new TreeViewModel{Id=601,Type="Salary",Label="薪资管理"},
                new TreeViewModel{Id=701,Type="Leave",Label="请假管理"},
                new TreeViewModel{Id=801,Type="kaoqing",Label="考勤管理"},
                new TreeViewModel{Id=901,Type="Department",Label="部门管理"}
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

            }); ;
        }


        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Claim_Get")]
        public ReturnCMDViewModel<IEnumerable<RolesViewModel>> GetRoles() => new ReturnCMDViewModel<IEnumerable<RolesViewModel>>()
        {
            code = (int)codes.Success,
            data = _claim.Roles
        };


        /// <summary>
        /// 编辑角色的声明
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Claim_Set")]
        public async Task<ReturnCMDViewModel<List<ClaimsViewModel>>> SetRoleClaim([FromBody] List<ClaimsViewModel> claims)
        {
            //bool flag1 = false, flag2 = false, flag3 = false;
            if (!claims.Any())
            {
                return new ReturnCMDViewModel<List<ClaimsViewModel>>()
                {
                    code = (int)codes.ChangeClaimError,
                    message = "编辑失败"
                };
            }
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
            return new ReturnCMDViewModel<List<ClaimsViewModel>>()
            {
                code = (int)codes.Success,
                message = $"\"{role.Name}\"编辑成功"
            };

        }

        /// <summary>
        /// 获取角色的用户列表
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetUsersInRole([FromBody] IdentityRole role)
        {
            IList<NewUser> thisRoleUser = await _userManager.GetUsersInRoleAsync(role.Name);//当前需要分配用户的角色所查的用户表
            IList<NewUser> staffRoleUser = await _userManager.GetUsersInRoleAsync("Staff");//Staff用户表/未分配权限表 
            //IEnumerable<NewUser> AllUser = _userManager.Users;//所有用户表
            return Ok(new
            {
                code = codes.Success,
                data = staffRoleUser.Union(thisRoleUser).Select(x => new { label = x.UserName, key = x.Id }),
                value = thisRoleUser.Select(x => x.Id)
            });
        }


        [HttpPost]
        public IActionResult SetRoleUsers(string direction, List<string> movedKeys)
        {
            return Content(direction + movedKeys);
        }
    }
}
