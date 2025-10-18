using Microsoft.EntityFrameworkCore;
using ST10448420_CMCsystem.Models;
using System.Security.Claims;

namespace ST10448420_CMCsystem.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
           : base(options) { }

        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<ProgrammeCoordinator> ProgrammeCoordinators { get; set; }
        public DbSet<AcademicManager> AcademicManagers { get; set; }
        public DbSet<Claims> Claims { get; set; }
        public DbSet<SupportingDocx> SupportingDocuments { get; set; }
        public DbSet<Approval> Approvals { get; set; }
    }
}
