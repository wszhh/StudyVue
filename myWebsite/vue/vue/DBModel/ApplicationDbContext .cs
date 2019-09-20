using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace vue.Areas.Identity.Data
{
    public class NewUser : IdentityUser
    {
        public string RealName { get; set; }
        public string Avatar { get; set; }
        public string Photo { get; set; }
        public string Address { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<NewUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
