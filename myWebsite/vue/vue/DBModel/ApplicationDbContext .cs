using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace vue.Areas.Identity.Data
{
    public class NewUser : IdentityUser
    {
        public string RealName { get; set; }
        public string Avatar { get; set; } = @"\wwwroot\Avatar\default.jpg";
        public string Photo { get; set; } = @"\wwwroot\photo\default.jpg";
        public string Address { get; set; }
        public int DepartmentId { get; set; }
        public int Sex { get; set; } = 1;
        public double Salary { get; set; }
        public string Introduction { get; set; }
        public DateTime JoinTime { get; set; }
        public DateTime Birthday { get; set; }

        public class ApplicationDbContext : IdentityDbContext<NewUser>
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
            }
        }
    }
}
