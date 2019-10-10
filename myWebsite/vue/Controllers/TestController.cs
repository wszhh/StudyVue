using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using vue.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace vue.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class TestController : Controller
    {

        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public TestController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RequestToken([FromBody]LoginViewModel request)
        {
            if (request != null)
            {
                //验证账号密码,这里只是为了demo，正式场景应该是与DB之类的数据源比对
                if ("smilesb101".Equals(request.UserName) && "123456".Equals(request.Password))
                {
                    var claims = new[] {
                        //加入用户的名称
                        new Claim(ClaimTypes.Name,request.UserName)
                    };
                    var audienceConfig = Configuration.GetSection("Audience");
                    var symmetricKeyAsBase64 = audienceConfig["Secret"];
                    var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
                    var key = new SymmetricSecurityKey(keyByteArray);
                    //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecurityKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var authTime = DateTime.UtcNow;
                    var expiresAt = authTime.AddDays(7);
                    var token = new JwtSecurityToken(
                        issuer: "vue",
                        audience: "zh",
                        claims: claims,
                        expires: expiresAt,
                        signingCredentials: creds);
                    // var token2 =new JwtSecurityToken();
                    return Ok(new
                    {
                        access_token = new JwtSecurityTokenHandler().WriteToken(token),
                        token_type = "Bearer",
                        profile = new
                        {
                            name = request.UserName,
                            auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                            expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
                        }
                    });
                }
            }

            return BadRequest("Could not verify username and password.Pls check your information.");
        }


    }
}