using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalApp.DA.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }

        //private const string AdminUserId = "Admin-USER-001";

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            string adminUserId = "Admin-USER-001";
            string adminRoleId = "ADMIN-ROLE-001";
            string doctorRoleId = "DOCTOR-ROLE-001";
            string patientRoleId = "PATIENT-ROLE-001";
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = doctorRoleId,
                    Name = "Doctor",
                    NormalizedName = "DOCTOR"
                },
                new IdentityRole
                {
                    Id = patientRoleId,
                    Name = "Patient",
                    NormalizedName = "PATIENT"
                }
            );

            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = adminUserId,
                UserName = "sarahyasser979@gmail.com",
                NormalizedUserName = "SARAHYASSER979@GMAIL.COM",
                Email = "sarahyasser979@gmail.com",
                NormalizedEmail = "SARAHYASSER979@GMAIL.COM",
                EmailConfirmed = true,
                //Admin@123
                PasswordHash = "AQAAAAIAAYagAAAAEIjJh6/LXD2Bg+3MJGc+CmiaE471FJWBEmlTQ/1OhqkFw0NIgG/beU7wkTfmnuQ/sQ==",
                SecurityStamp = "STATIC-SECURITY-STAMP-001",
                ConcurrencyStamp = "STATIC-CONCURRENCY-STAMP-001",
                FirstName = "Sara",
                LastName = "Yasser",
                PhoneNumber = "01159757952"
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
new IdentityUserRole<string>
{
    RoleId = adminRoleId,
    UserId = adminUserId
}
 );
        }
    }
}
