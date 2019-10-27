using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using vue.Areas.Identity.Data;
using vue.DBModel;
using vue.IService;
using vue.IService.Implement;
using static vue.Areas.Identity.Data.NewUser;

namespace vue
{

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            #region 依赖注入
            services.AddSingleton<ILeave, ImpLeave>();
            services.AddSingleton<IDepartment, ImpDepartment>();
            services.AddSingleton<IAspNetUsers, ImpAspNetUsers>();
            services.AddSingleton<IClaim, ImpClaim>();
            services.AddSingleton<IAttendance, ImpAttendance>();
            services.AddSingleton<ICategoryItems, ImpCategoryItems>();
            #endregion

            #region 数据库、IdentityService
            services.AddDbContext<HRCContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("vue")));
            services.AddDefaultIdentity<NewUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            #endregion

            #region 授权
            // 然后这么写 [Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Ceo", policy => policy.RequireRole("Ceo").Build());
                options.AddPolicy("HRManager", policy => policy.RequireRole("HRManager").Build());
                options.AddPolicy("D.Manager", policy => policy.RequireRole("D.Manager").Build());
                options.AddPolicy("HRAssistant", policy => policy.RequireRole("Ceo").Build());
                options.AddPolicy("Staff", policy => policy.RequireRole("Staff").Build());
                options.AddPolicy("CeoOrHRManager", policy => policy.RequireRole("Ceo", "HRManager"));
                //主页
                options.AddPolicy("Home_Get", policy => policy.RequireClaim("Home", "Get"));
                //员工资料管理
                //-个人信息
                options.AddPolicy("MyInfo_Get", policy => policy.RequireClaim("MyInfo", "Get"));
                options.AddPolicy("MyInfo_Set", policy => policy.RequireClaim("MyInfo", "Set"));
                options.AddPolicy("MyInfo_SetPwd", policy => policy.RequireClaim("MyInfo", "SetPwd"));
                //-查找同事
                options.AddPolicy("Colleague_Get", policy => policy.RequireClaim("Colleague", "Get"));
                options.AddPolicy("Colleague_Find", policy => policy.RequireClaim("Colleague", "Find"));
                //-员工管理
                options.AddPolicy("Staff_Get", policy => policy.RequireClaim("Staff", "Get"));
                options.AddPolicy("Staff_Add", policy => policy.RequireClaim("Staff", "Add"));
                options.AddPolicy("Staff_Del", policy => policy.RequireClaim("Staff", "Del"));
                options.AddPolicy("Staff_Set", policy => policy.RequireClaim("Staff", "Set"));
                options.AddPolicy("Staff_Find", policy => policy.RequireClaim("Staff", "Find"));
                //部门管理
                options.AddPolicy("Department_Get", policy => policy.RequireClaim("Department", "Get"));
                options.AddPolicy("Department_Set", policy => policy.RequireClaim("Department", "Set"));
                options.AddPolicy("Department_Del", policy => policy.RequireClaim("Department", "Del"));
                options.AddPolicy("Department_Add", policy => policy.RequireClaim("Department", "Add"));
                //权限管理
                options.AddPolicy("Claim_Get", policy => policy.RequireClaim("Claim", "Get"));
                options.AddPolicy("Claim_Add", policy => policy.RequireClaim("Claim", "Add"));
                options.AddPolicy("Claim_Del", policy => policy.RequireClaim("Claim", "Del"));
                options.AddPolicy("Claim_Set", policy => policy.RequireClaim("Claim", "Set"));
                options.AddPolicy("Claim_SetRole", policy => policy.RequireClaim("Claim", "SetRole"));//分配角色
            });

            #endregion

            #region CORS
            //跨域第二种方法，声明策略，记得下边app中配置
            services.AddCors(c =>
            {
                //↓↓↓↓↓↓↓注意正式环境不要使用这种全开放的处理↓↓↓↓↓↓↓↓↓↓
                c.AddPolicy("AllRequests", policy =>
                {
                    policy
                    .AllowAnyOrigin()//允许任何源
                    .AllowAnyMethod()//允许任何方式
                    .AllowAnyHeader()//允许任何头
                    .AllowCredentials();//允许cookie
                });
                //↑↑↑↑↑↑↑注意正式环境不要使用这种全开放的处理↑↑↑↑↑↑↑↑↑↑


                //一般采用这种方法
                c.AddPolicy("LimitRequests", policy =>
                {
                    // 支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的
                    // 注意，http://127.0.0.1:1818 和 http://localhost:1818 是不一样的，尽量写两个
                    policy
                    .WithOrigins("http://192.168.1.238:8080", "http://radiosilence.cn", "http://120.78.170.40:8080", "http://localhost:9528", "http://192.168.1.6:9528")
                    .AllowAnyHeader()//Ensures that the policy allows any header.
                    .AllowAnyMethod();
                    //.WithExposedHeaders("Token-Expired");
                });
            });

            //跨域第一种办法，注意下边 Configure 中进行配置
            //services.AddCors();
            #endregion

            #region 认证
            //读取配置文件
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            //认证
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,//参数配置
                    // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                    ValidateIssuer = true,
                    ValidIssuer = audienceConfig["Issuer"],//发行人
                    ValidateAudience = true,
                    ValidAudience = audienceConfig["Audience"],//订阅人
                    // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    ValidateLifetime = true,
                    // 允许的服务器时间偏移量
                    ClockSkew = TimeSpan.FromSeconds(5),
                    // 是否要求Token的Claims中必须包含Expires
                    RequireExpirationTime = true
                };
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // 如果过期，则把<是否过期>添加到，返回头信息中
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            #endregion

            #region 密码、登录等相关规则

            //密码、登录等相关规则
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            #region CORS
            //跨域第二种方法，使用策略，详细策略信息在ConfigureService中
            app.UseCors("LimitRequests");//将 CORS 中间件添加到 web 应用程序管线中, 以允许跨域请求。


            #region 跨域第一种版本
            //跨域第一种版本，请要ConfigureService中配置服务 services.AddCors();
            //    app.UseCors(options => options.WithOrigins("http://localhost:8021").AllowAnyHeader()
            //.AllowAnyMethod());  
            #endregion

            #endregion
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}

