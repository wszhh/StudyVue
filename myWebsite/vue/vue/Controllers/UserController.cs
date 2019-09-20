using vue.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using vue.Auth;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;
using codes = ViewModel.StateCodes.StateCode;
using System.IO;
using vue.Areas.Identity.Data;

namespace vue.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly SignInManager<NewUser> _signInManager;
        private readonly UserManager<NewUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly IConfiguration _configuration;
        //private int seconds = Convert.ToInt32(Appsettings.app(new string[] { "Exp", "Num" }));

        public UserController(SignInManager<NewUser> signInManager, UserManager<NewUser> userManager
        //IConfiguration configuration
        , RoleManager<IdentityRole> roleManager
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            //_configuration = configuration;
            _roleManager = roleManager;
        }

        /// <summary>
        /// 用户登录方法
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            #region 临时添加使用
            //for (int i = 0; i < 6; i++)
            //{
            //    await _roleManager.CreateAsync(new IdentityRole { Name = "HRManager" + i.ToString() });
            //}
            //var role = await _userManager.AddToRoleAsync(user, "ceo");
            //var role = await _userManager.AddClaimAsync(user, new Claim("删除", "delete"));
            //var aaaa = await _roleManager.GetClaimsAsync(new IdentityRole { Name = "Admin" });
            //var role2 = await _roleManager.AddClaimAsync(new IdentityRole { Name = "Admin" }, new Claim("删除", "delete"));
            //await Regiseter(new RegisterViewModel()
            //{
            //    UserName = "Admin",
            //    Email = "Q.Q@com",
            //    Password = "Qq111`"
            //});
            #endregion
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, true);
                if (result.Succeeded)
                {
                    //Token的制作与发放
                    string roles = RoleToStr(await _userManager.GetRolesAsync(user));
                    TokenModelJwt tokenModel = new TokenModelJwt();
                    tokenModel.ID = user.Id;
                    tokenModel.Role = roles;
                    var token = JwtHelper.IssueJwt(tokenModel);
                    return Ok(new
                    {
                        code = codes.Success,
                        data = token,
                    });
                }
                if (result.IsLockedOut)
                {
                    return Ok(new
                    {
                        code = codes.IsLocked,
                        message = "账户已被临时锁定,请稍后再试"
                    });
                }
            }
            return Ok(new
            {
                code = codes.NameOrPwdError,
                message = "登录失败，请检查用户名或密码"
            });
        }

        /// <summary>
        /// 把权限集合转成字符串
        /// </summary>
        /// <param name="Role">权限集合</param>
        /// <returns></returns>
        private static string RoleToStr(IList<string> Role)
        {
            if (Role != null)
            {
                string Str = string.Empty;

                foreach (string item in Role)
                {
                    Str += item + ",";
                }
                return Str;
            }
            return "Client";
        }


        /// <summary>
        /// 注册的方法
        /// </summary>
        /// <param name="registerViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Regiseter([FromBody]RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new NewUser
                {
                    UserName = registerViewModel.UserName
                };
                var RegisterUserResult = await _userManager.CreateAsync(user, registerViewModel.Password);
                if (RegisterUserResult.Succeeded)
                {
                    var SetEmailResult = await _userManager.SetEmailAsync(user, registerViewModel.Email);
                    return Ok("1");
                }
            }
            return Ok("0");
        }


        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        //[Route("RefreshToken")]
        public async Task<ObjectResult> RefreshToken(string oldToken = "")
        {
            if (string.IsNullOrEmpty(oldToken))
            {
                return Ok(new
                {
                    code = (int)codes.TokenError,
                    message = "token无效，请重新登录！"
                });
            }
            var tokenModel = JwtHelper.SerializeJwt(oldToken);
            if (tokenModel != null && tokenModel.ID != null)
            {
                var user = await _userManager.FindByIdAsync(tokenModel.ID);
                if (user != null)
                {
                    IList<string> Role = await _userManager.GetRolesAsync(user);
                    TokenModelJwt refreshTokenModel = new TokenModelJwt();
                    refreshTokenModel.ID = user.Id;
                    refreshTokenModel.Role = RoleToStr(Role);
                    var token = JwtHelper.IssueJwt(refreshTokenModel);
                    return Ok(new
                    {
                        data = token,
                        code = (int)codes.Success
                    });
                }
            }
            return Ok(new
            {
                code = (int)codes.TokenRefreshError,
                message = "Token刷新失败"
            });
        }


        /// <summary>
        /// 获取用户详情根据token
        /// 【无权限】
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ObjectResult> info(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var tokenModel = JwtHelper.SerializeJwt(token);
                if (tokenModel != null && tokenModel.ID != null)
                {
                    var userinfo = await _userManager.FindByIdAsync(tokenModel.ID);
                    string[] roles = RoleToStr(await _userManager.GetRolesAsync(userinfo)).Split(",");
                    if (userinfo != null)
                    {
                        return Ok(
                            new
                            {
                                code = codes.Success,
                                data = new
                                {
                                    name = userinfo.UserName,
                                    avatar = Convert.ToBase64String(System.IO.File.ReadAllBytes($"{Directory.GetCurrentDirectory()}\\wwwroot\\Avatar\\default.jpg")),
                                    roles,
                                    userinfo.Photo,
                                    userinfo.RealName
                                }
                            }
                    ); ;
                    }
                }
            }
            return Ok(new
            {
                code = codes.GetUserInfoError,
                message = "获取用户信息失败"
            });

        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ReturnCMDViewModel<string> logout(string token)
        {
            return new ReturnCMDViewModel<string>()
            {
                code = (int)codes.Success,
            };

        }
    }
}
