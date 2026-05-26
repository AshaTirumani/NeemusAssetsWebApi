
using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI.Models;


namespace NeemusAssetWebAPI.Data
{
    public class PostgreDBContext : DbContext
    {
        public PostgreDBContext(DbContextOptions<PostgreDBContext> options) : base(options)
        {

        }

        // Tables
        public DbSet<LocationMaster> LocationMasters { get; set; }
        public DbSet<Department> Departments{ get; set; }
        public DbSet<DocumentModel> DocumentModels { get; set; }
        public DbSet<ServiceTypeModel> ServiceTypeModels { get; set; }
        public DbSet<EmployeeMaster> EmployeeMasters { get; set; }
        public DbSet<AuditMaster> AuditMasters { get; set; }
        public DbSet<StatusMaster> StatusMasters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocationMaster>()
                .Property(x => x.CreatedDate)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<EmployeeMaster>()
                .Property(x => x.CreateDate)
                .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<DocumentModel>()
               .Property(x => x.CreatedDate)
               .HasColumnType("timestamp without time zone");
        }
       

    }
}
