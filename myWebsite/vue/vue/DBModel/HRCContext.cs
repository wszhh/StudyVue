using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace vue.DBModel
{
    public partial class HRCContext : DbContext
    {
        public HRCContext()
        {
        }

        public HRCContext(DbContextOptions<HRCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Assessment> Assessment { get; set; }
        public virtual DbSet<AttendanceSheet> AttendanceSheet { get; set; }
        public virtual DbSet<CategoryItems> CategoryItems { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Leave> Leave { get; set; }
        public virtual DbSet<Notice> Notice { get; set; }
        public virtual DbSet<Overtime> Overtime { get; set; }
        public virtual DbSet<OvertimeCheck> OvertimeCheck { get; set; }
        public virtual DbSet<PayRise> PayRise { get; set; }
        public virtual DbSet<PaySlip> PaySlip { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Salary> Salary { get; set; }
        public virtual DbSet<StuList> StuList { get; set; }
        public virtual DbSet<SystemLog> SystemLog { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=HRC;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Birthday).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.JoinTime).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.Salary).HasDefaultValueSql("((0.000000000000000e+000))");

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Assessment>(entity =>
            {
                entity.Property(e => e.AssessmentId).HasColumnName("AssessmentID");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ExaminationItems)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NextStageObjectives)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PerformanceTime).HasColumnType("datetime");

                entity.Property(e => e.Perstate).HasColumnName("perstate");

                entity.Property(e => e.UpperGoal).IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.WorkSummary).IsUnicode(false);
            });

            modelBuilder.Entity<AttendanceSheet>(entity =>
            {
                entity.HasKey(e => e.AttendanceId)
                    .HasName("PK__Attendan__8B69263CF5A6F119");

                entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");

                entity.Property(e => e.AttendanceStartTime).HasColumnType("datetime");

                entity.Property(e => e.ClockOutTime).HasColumnType("datetime");

                entity.Property(e => e.ClockTime).HasColumnType("datetime");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Remake)
                    .HasColumnName("remake")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CategoryItems>(entity =>
            {
                entity.HasKey(e => e.Cid)
                    .HasName("PK__Category__C1F8DC5941E0D6A3");

                entity.Property(e => e.Cid).HasColumnName("CID");

                entity.Property(e => e.CCategory)
                    .HasColumnName("C_Category")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CiId).HasColumnName("CI_ID");

                entity.Property(e => e.CiName)
                    .HasColumnName("CI_Name")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Leave>(entity =>
            {
                entity.Property(e => e.LeaveId).HasColumnName("LeaveID");

                entity.Property(e => e.ApprovalTime).HasColumnType("datetime");

                entity.Property(e => e.ApproverId).HasColumnName("ApproverID");

                entity.Property(e => e.ApproverReason)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LeaveEndTime).HasColumnType("datetime");

                entity.Property(e => e.LeaveHalfDay)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LeaveReason)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LeaveStartTime).HasColumnType("datetime");

                entity.Property(e => e.LeaveTime).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Notice>(entity =>
            {
                entity.Property(e => e.NoticeId).HasColumnName("NoticeID");

                entity.Property(e => e.NoticeContent).IsUnicode(false);

                entity.Property(e => e.NoticeEndTime).HasColumnType("datetime");

                entity.Property(e => e.NoticeStateTime).HasColumnType("datetime");

                entity.Property(e => e.NoticeTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Overtime>(entity =>
            {
                entity.Property(e => e.OvertimeId).HasColumnName("OvertimeID");

                entity.Property(e => e.ApplyTime).HasColumnType("datetime");

                entity.Property(e => e.ApproverReason)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.OvertimeEndTime).HasColumnType("datetime");

                entity.Property(e => e.OvertimeStateTime).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<OvertimeCheck>(entity =>
            {
                entity.HasKey(e => e.ApprovalId)
                    .HasName("PK__Overtime__328477D43704E98C");

                entity.Property(e => e.ApprovalId).HasColumnName("ApprovalID");

                entity.Property(e => e.DepartmentalAuditRemarks)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.GeneralManagerAuditRemarks)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LeaveId).HasColumnName("LeaveID");

                entity.Property(e => e.ManagerAuditRemarks)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<PayRise>(entity =>
            {
                entity.Property(e => e.PayRiseId).HasColumnName("PayRiseID");

                entity.Property(e => e.ApplicationTime).HasColumnType("datetime");

                entity.Property(e => e.ApprovalContent)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ApprovalTime).HasColumnType("datetime");

                entity.Property(e => e.PayRiseMoney).HasColumnType("money");

                entity.Property(e => e.Reason).IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<PaySlip>(entity =>
            {
                entity.Property(e => e.PaySlipId).HasColumnName("PaySlipID");

                entity.Property(e => e.Absence).HasColumnType("money");

                entity.Property(e => e.AdvanceMoney).HasColumnType("money");

                entity.Property(e => e.Fine)
                    .HasColumnName("fine")
                    .HasColumnType("money");

                entity.Property(e => e.LateMoney).HasColumnType("money");

                entity.Property(e => e.LeaveMoney).HasColumnType("money");

                entity.Property(e => e.OvertimeMoney).HasColumnType("money");

                entity.Property(e => e.Prize).HasColumnType("money");

                entity.Property(e => e.SaBonus)
                    .HasColumnName("Sa_Bonus")
                    .HasColumnType("money");

                entity.Property(e => e.SaTime)
                    .HasColumnName("Sa_Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.SaTotalSalary)
                    .HasColumnName("Sa_TotalSalary")
                    .HasColumnType("money");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AttendanceBonus).HasColumnType("money");

                entity.Property(e => e.BasicSalary).HasColumnType("money");

                entity.Property(e => e.Fine).HasColumnType("money");

                entity.Property(e => e.SalarySum).HasColumnType("money");

                entity.Property(e => e.SalaryTime).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StuList>(entity =>
            {
                entity.HasKey(e => e.Bmh)
                    .HasName("PK__StuList__DE97B1704E1ED7FA");

                entity.Property(e => e.Bmh)
                    .HasColumnName("bmh")
                    .ValueGeneratedNever();

                entity.Property(e => e.Dqdm).HasColumnName("dqdm");

                entity.Property(e => e.Hjszd)
                    .IsRequired()
                    .HasColumnName("hjszd")
                    .HasMaxLength(255);

                entity.Property(e => e.Mzmc)
                    .IsRequired()
                    .HasColumnName("mzmc")
                    .HasMaxLength(255);

                entity.Property(e => e.Score)
                    .IsRequired()
                    .HasColumnName("score")
                    .HasMaxLength(255);

                entity.Property(e => e.Sfzh)
                    .IsRequired()
                    .HasColumnName("sfzh")
                    .HasMaxLength(255);

                entity.Property(e => e.Xm)
                    .IsRequired()
                    .HasColumnName("xm")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<SystemLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__SystemLo__5E5499A8EF9D39D6");

                entity.Property(e => e.LogId).HasColumnName("LogID");

                entity.Property(e => e.LogName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LogOperation)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LogTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserInfo__1788CCAC4427A628");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Avatar).IsUnicode(false);

                entity.Property(e => e.BasePay).HasColumnType("money");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.DimissionTime).HasColumnType("datetime");

                entity.Property(e => e.EntryTime).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.RealName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserBirthday).HasColumnType("datetime");

                entity.Property(e => e.UserNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserRemarks)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UserTel)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
