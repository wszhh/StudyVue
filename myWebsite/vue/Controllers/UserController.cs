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
using vue.IService;
using Microsoft.AspNetCore.Http;
using ViewModel;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Cors;

namespace vue.Controllers
{


    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private const string DefaultUserPhoto = "\\wwwroot\\Photo\\default.jpg";
        private const string DefaultUserAvatar = "\\wwwroot\\Avatar\\default.jpg";
        private readonly SignInManager<NewUser> _signInManager;
        private readonly UserManager<NewUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //private readonly IConfiguration _configuration;
        private readonly IAspNetUsers _aspNetUsers;
        //private int seconds = Convert.ToInt32(Appsettings.app(new string[] { "Exp", "Num" }));

        public UserController(SignInManager<NewUser> signInManager, UserManager<NewUser> userManager, RoleManager<IdentityRole> roleManager, IAspNetUsers aspNetUsers
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _aspNetUsers = aspNetUsers;
        }

        #region 临时添加使用
        //for (int i = 0; i < 6; i++)
        //{
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "HRManager" + i.ToString() });
        //}
        //var role = await _userManager.AddToRoleAsync(user, "Staff");
        //var role = await _userManager.AddClaimAsync(user, new Claim("删除", "delete"));
        //var aaaa = await _roleManager.GetClaimsAsync(new IdentityRole { Name = "Admin" });
        //var role2 = await _roleManager.AddClaimAsync(new IdentityRole { Name = "Admin" }, new Claim("删除", "delete"));
        //await Regiseter(new RegisterViewModel()
        //{
        //    UserName = "xiaoming",
        //    Email = "Q1.Q@com",
        //    Password = "xiaoming`"
        //});
        //await Regiseter(new RegisterViewModel()
        //{
        //    UserName = "zhangwu",
        //    Email = "Q2.Q@com",
        //    Password = "Qq111`"
        //});
        //var Ceo = await _roleManager.FindByNameAsync("Ceo");
        //await _roleManager.AddClaimAsync(Ceo, new Claim("Department", "Get"));
        //for (int i = 1; i <= 20; i++)
        //{
        //    await Regiseter(new RegisterViewModel()
        //    {
        //        UserName = "zhang" + i,
        //        Email = $"q{i}@qq.com",
        //        Password = "Qq111."
        //    });

        //}
        #endregion
        /// <summary>
        /// 用户登录方法
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new
                {
                    code = codes.NameOrPwdError,
                    message = "登录失败，请检查用户名或密码"
                });
            }
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, true);
                if (result.Succeeded)
                {

                    //Token的制作与发放
                    var roleName = (await _userManager.GetRolesAsync(user));
                    IList<Claim> ClaimResult = await _roleManager.GetClaimsAsync(await _roleManager.FindByNameAsync(roleName.Count == 0 ? "Staff" : roleName[0]));
                    TokenModelJwt tokenModel = new TokenModelJwt();
                    tokenModel.ID = user.Id;
                    tokenModel.Claims = ClaimResult;
                    var token = JwtHelper.IssueJwt(tokenModel);
                    return Ok(new
                    {
                        code = codes.Success,
                        data = token,
                        message = $"登录成功，欢迎\"{user.UserName}\""
                    });
                }
                if (result.IsLockedOut)
                {

                    return Ok(new
                    {
                        code = codes.IsLocked,
                        message = $"账户已被临时锁定,请稍后再试{await _signInManager.UserManager.GetLockoutEndDateAsync(user)}"
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
                    await _userManager.SetEmailAsync(user, registerViewModel.Email);
                    _aspNetUsers.setUserPhoto(user.Id, DefaultUserPhoto);//默认照片
                    await _userManager.AddToRoleAsync(user, "Staff");//默认添加Staff权限
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
                    var roleName = (await _userManager.GetRolesAsync(user));
                    IList<Claim> ClaimResult = await _roleManager.GetClaimsAsync(await _roleManager.FindByNameAsync(roleName.Count == 0 ? "Staff" : roleName[0]));
                    tokenModel.ID = user.Id;
                    tokenModel.Claims = ClaimResult;
                    var token = JwtHelper.IssueJwt(tokenModel);
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
        /// 登录后获取用户基本信息
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Info(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var tokenModel = JwtHelper.SerializeJwt(token);
                if (tokenModel != null && tokenModel.ID != null)
                {
                    List<string> roles = new List<string>();
                    var userinfo = await _userManager.FindByIdAsync(tokenModel.ID);
                    if (userinfo == null)
                    {
                        return Ok(new { code = 50008 });
                    }
                    var roleName = (await _userManager.GetRolesAsync(userinfo));
                    IList<Claim> ClaimResult = await _roleManager.GetClaimsAsync(await _roleManager.FindByNameAsync(roleName.Count == 0 ? "Staff" : roleName[0]));
                    if (ClaimResult.Count() > 0)
                    {
                        foreach (Claim item in ClaimResult)
                        {
                            roles.Add(item.Type + "_" + item.Value);
                        }
                    }
                    else
                    {
                        roles.Add("No");
                    }
                    if (userinfo != null)
                    {
                        return Ok(
                            new
                            {
                                code = codes.Success,
                                data = new
                                {
                                    name = userinfo.UserName,
                                    avatar = PhotoToBase64String(DefaultUserAvatar),
                                    roles,
                                }
                            }
                    );
                    }
                }
                return Ok(new
                {
                    code = (int)codes.TokenError,
                    message = "token无效，请重新登录！"
                });
            }
            return Ok(new
            {
                code = codes.GetUserInfoError,
                message = "获取用户信息失败"
            });

        }

        /// <summary>
        /// 个人信息-获取个人信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "MyInfo_Get")]
        public async Task<ReturnViewModel<UserInfoViewModel>> GetUserinfo(string token)
        {

            if (!string.IsNullOrEmpty(token))
            {
                var tokenModel = JwtHelper.SerializeJwt(token);
                if (tokenModel != null && tokenModel.ID != null)
                {
                    var userInfo = await _userManager.FindByIdAsync(tokenModel.ID);
                    return new ReturnViewModel<UserInfoViewModel>()
                    {
                        code = (int)codes.Success,
                        data = new UserInfoViewModel()
                        {
                            RealName = userInfo.RealName,
                            DepartmentId = userInfo.DepartmentId,
                            PhoneNumber = userInfo.PhoneNumber,
                            Address = userInfo.Address,
                            Salary = userInfo.Salary,
                            Introduction = userInfo.Introduction,
                            JoinTime = userInfo.JoinTime,
                            Photo = PhotoToBase64String(userInfo.Photo),
                            Birthday = userInfo.Birthday,
                            Sex = userInfo.Sex
                        }
                    };
                }
            }
            return new ReturnViewModel<UserInfoViewModel>()
            {
                code = (int)codes.GetUserInfoError,
                message = "获取个人数据失败"
            };
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ReturnViewModel<string> logout(string token)
        {
            return new ReturnViewModel<string>()
            {
                code = (int)codes.Success,
                message = "已退出"
            };

        }

        /// <summary>
        /// JEPG图片转BASE64
        /// </summary>
        /// <param name="fileRelativePath"></param>
        /// <returns></returns>
        private static string PhotoToBase64String(string fileRelativePath)
        {
            return "data:image/jpeg;base64," +
                Convert.ToBase64String(System.IO.File.ReadAllBytes($"{Directory.GetCurrentDirectory()}{fileRelativePath}"));

        }

        /// <summary>
        /// 个人信息-用户自己修改照片
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "MyInfo_Set")]
        public async Task<ReturnViewModel<IActionResult>> SetPhoto([FromForm]IFormFile file)
        {
            string token = Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token) && file.ContentType == "image/jpeg")
            {
                var tokenModel = JwtHelper.SerializeJwt(token.Substring(7));
                if (tokenModel != null && tokenModel.ID != null)
                {
                    string newFilname = Guid.NewGuid().ToString();
                    var userInfo = await _userManager.FindByIdAsync(tokenModel.ID);
                    string savePath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\photo\\{userInfo.UserName}__{newFilname}.jpg";
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return _aspNetUsers.setUserPhoto(userInfo.Id, $"\\wwwroot\\photo\\{userInfo.UserName}__{newFilname}.jpg");
                }
            }
            return new ReturnViewModel<IActionResult>() { code = (int)codes.TokenOrFileError, message = "token或者文件有误" };
        }


        /// <summary>
        /// 员工管理-添加员工时添加照片
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Staff_Set")]
        public async Task<ReturnViewModel<IActionResult>> SetStaffphoto([FromForm]string UserName, IFormFile file)
        {
            if (!string.IsNullOrEmpty(UserName) && file.ContentType == "image/jpeg")
            {
                var userInfo = await _userManager.FindByNameAsync(UserName);
                if (userInfo != null)
                {
                    string newFilname = Guid.NewGuid().ToString();
                    string savePath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\photo\\{userInfo.UserName}__{newFilname}.jpg";
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return _aspNetUsers.setUserPhoto(userInfo.Id, $"\\wwwroot\\photo\\{userInfo.UserName}__{newFilname}.jpg");
                }
            }
            return new ReturnViewModel<IActionResult>() { code = (int)codes.TokenOrFileError, message = "token或者文件有误" };
        }

        /// <summary>
        /// 个人信息-修改部分个人信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "MyInfo_Set")]
        public ReturnViewModel<IActionResult> SetUserInfo([FromBody]UserInfoViewModel NewUserInfo)
        {
            string token = Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token) && NewUserInfo.RealName != null)
            {
                var tokenModel = JwtHelper.SerializeJwt(token.Substring(7));
                if (tokenModel != null && tokenModel.ID != null)
                {
                    return _aspNetUsers.setUserInfo(tokenModel.ID, NewUserInfo);
                    //var UserInfo = await _userManager.FindByIdAsync(tokenModel.ID);
                }
            }
            return new ReturnViewModel<IActionResult>()
            {
                code = (int)codes.ChangeUserInfoError,
                message = "更改个人信息失败"
            };
        }

        /// <summary>
        /// 个人信息-修改密码
        /// </summary>
        /// <param name="passwords">新旧密码</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "MyInfo_SetPwd")]
        public async Task<ReturnViewModel<IActionResult>> ChangePassword([FromBody]ChangePasswordViewModel passwords)
        {
            if (!ModelState.IsValid)
            {
                new ReturnViewModel<IActionResult>()
                {
                    code = (int)codes.ChangePasswordError,
                    message = "更改密码失败,请检查是否填写正确"
                };
            }
            string token = Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token) && passwords.OldPassword != null)
            {
                var tokenModel = JwtHelper.SerializeJwt(token.Substring(7));
                var UserInfo = await _userManager.FindByIdAsync(tokenModel.ID);
                if (tokenModel != null && UserInfo.Id != null)
                {
                    var result = await _userManager.ChangePasswordAsync(UserInfo, passwords.OldPassword, passwords.NewPassword);
                    if (result.Succeeded)
                    {
                        return new ReturnViewModel<IActionResult>()
                        {
                            code = (int)codes.Success,
                            message = "更改密码成功"
                        };

                    }

                }
            }
            return new ReturnViewModel<IActionResult>()
            {
                code = (int)codes.ChangePasswordError,
                message = "更改密码失败，请检查原密码是否正确"
            };
        }

        /// <summary>
        /// 查找同事-获取同事列表
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Colleague_Get")]
        public ReturnViewModel<PaginationResponeViewModel<IEnumerable<UserInfoViewModel>>> GetColleaguesList([FromBody]PaginationRequestViewModel pagination)
        {
            return new ReturnViewModel<PaginationResponeViewModel<IEnumerable<UserInfoViewModel>>>()
            {
                code = (int)codes.Success,
                data = _aspNetUsers.getColleaguesList(pagination),
            };
        }
        /// <summary>
        /// 员工管理-搜索同事列表
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Staff_Find")]
        public ReturnViewModel<PaginationResponeViewModel<IEnumerable<UserInfoViewModel>>> FindColleagueByName([FromBody]PaginationRequestViewModel<string> pagination)
        {
            return new ReturnViewModel<PaginationResponeViewModel<IEnumerable<UserInfoViewModel>>>()
            {
                code = (int)codes.Success,
                data = _aspNetUsers.findColleagueByName(pagination),
            };
        }

        /// <summary>
        /// 员工管理-排序获取员工列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Staff_Get")]
        public ReturnViewModel<PaginationResponeViewModel<IEnumerable<UserInfoViewModel>>> GetStaffListByOrder([FromBody]PaginationRequestViewModel<OrderPropKeywordViewModel> pagination)
        {
            return new ReturnViewModel<PaginationResponeViewModel<IEnumerable<UserInfoViewModel>>>()
            {
                code = (int)codes.Success,
                data = _aspNetUsers.getStaffListByOrder(pagination),
            };
        }

        /// <summary>
        /// 员工管理-添加员工
        /// </summary>
        /// <param name="registerViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Staff_Add")]
        public async Task<ReturnViewModel<IActionResult>> AddStaff([FromBody] NewUser newUser)
        {
            if (ModelState.IsValid)
            {
                var user = new NewUser
                {
                    UserName = newUser.UserName
                };
                var RegisterUserResult = await _userManager.CreateAsync(user, "Qq123.");
                if (RegisterUserResult.Succeeded)
                {
                    newUser.Photo = DefaultUserPhoto;
                    newUser.Avatar = DefaultUserAvatar;
                    //用户创建成功后开始添加个人资料
                    return _aspNetUsers.setStaffInfo(newUser.UserName, newUser);
                }
            }
            return new ReturnViewModel<IActionResult>() { code = (int)codes.AddStaffError, message = "添加员工失败" };
        }

        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CheckUserName([FromBody]NewUser user)
        {
            if (!string.IsNullOrEmpty(user.UserName))
            {
                if (await _userManager.FindByNameAsync(user.UserName) != null)
                {
                    return Ok(new
                    {
                        code = (int)codes.Success,
                        data = 19999//只能这么搞了，他前端那个框架会拦截code!=20000的，不能直接获取code的再判断，只能从data传了
                    });
                }
                else
                {
                    return Ok(new
                    {
                        code = (int)codes.Success,
                    });
                }
            }
            return Ok(new
            {
                code = (int)codes.UserNameError,
                message = "用户名为空"
            });
        }

        /// <summary>
        /// 员工管理-修改员工信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Staff_Set")]
        public ReturnViewModel<IActionResult> SetStaffInfo([FromBody]UserInfoViewModel NewUserInfo)
        {
            if (NewUserInfo != null)
            {
                return _aspNetUsers.setStaffInfos(NewUserInfo.Id, NewUserInfo);
            }
            return new ReturnViewModel<IActionResult>()
            {
                code = (int)codes.ChangeUserInfoError,
                message = "更改个人信息失败，请检查是否填写正确"
            };
        }

        /// <summary>
        /// 删除一个员工
        /// </summary>
        /// <param name="aUser"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Staff_Del")]
        public async Task<ReturnViewModel<IActionResult>> DeleteAStaff([FromBody]NewUser aUser)
        {
            if (aUser.Id != null)
            {
                NewUser user = await _userManager.FindByIdAsync(aUser.Id);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return new ReturnViewModel<IActionResult> { code = (int)codes.Success, message = $"用户\"{aUser.RealName}\"删除成功" };
                    }
                }
            };
            return new ReturnViewModel<IActionResult> { code = (int)codes.DeleteUserError, message = $"用户删除失败" };
        }

        /// <summary>
        /// 获取员工照片
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Staff_Get")]
        public async Task<ReturnViewModel<IActionResult>> GetStaffPhoto([FromBody]NewUser aUser)
        {
            if (aUser.Id != null)
            {
                NewUser user = await _userManager.FindByIdAsync(aUser.Id);
                if (user != null)
                {
                    return new ReturnViewModel<IActionResult> { code = (int)codes.Success, data = Ok(PhotoToBase64String(user.Photo)), };
                }
            };
            return new ReturnViewModel<IActionResult> { code = (int)codes.DeleteUserError, message = $"用户删除失败" };
        }
    }
}
