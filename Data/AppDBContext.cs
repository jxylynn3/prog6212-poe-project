using Microsoft.EntityFrameworkCore;
using ST10448420_CMCsystem.Models;
using System.Security.Claims;

namespace ST10448420_CMCsystem.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
           : base(options) { }

        public DbSet<Lecturer> Lecturer { get; set; }
        public DbSet<ProgrammeCoordinator> ProgrammeCoordinator { get; set; }
        public DbSet<AcademicManager> AcademicManager { get; set; }
        public DbSet<Claims> Claims { get; set; }
        public DbSet<SupportingDocx> SupportingDocument { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        //part 03
        public DbSet<HR> HR { get; set; }
        public DbSet<Tracking> Tracking { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//seed data for HR table
            modelBuilder.Entity<HR>().HasData(
            new HR
            {
            HRID = "HR000001",
            FirstName = "System",
            Surname = "Administrator",
            Email = "hr@system.com",
            Username = "HRadmin",
            Password = "#HRadmin123#"
            }
        );
        }

    }
}
