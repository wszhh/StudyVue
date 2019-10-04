using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using vue.DBModel;

namespace vue.Areas.Identity.Data
{
    public class NewUser : IdentityUser
    {
        private HRCContext db = new HRCContext();


        public NewUser()
        {
            var result = db.AspNetUsers.Max(x => x.Id);
            Id = result == null ? "1000000001" : $"{int.Parse(result) + 1}";
        }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 头像相对路径
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 照片相对路径
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// 住址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>
        public int DepartmentId { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 薪资
        /// </summary>
        public double Salary { get; set; }
        /// <summary>
        /// 个人介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime JoinTime { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        public class ApplicationDbContext : IdentityDbContext<NewUser>
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
            }
        }
    }
}
