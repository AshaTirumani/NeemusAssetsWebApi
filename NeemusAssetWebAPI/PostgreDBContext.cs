
using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI.Models;
using static NeemusAssetWebAPI.Models.CustodianChangeRequestModel;


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

        public DbSet<EmployeeAssetRequest> EmployeeAssetRequests { get; set; }
        public DbSet<CustodianChangeRequest> CustodianChangeRequests { get; set; }
        public DbSet<EmployeeLocationChange> EmployeeLocationChanges{ get; set; }
        public DbSet<EmployeeAssetBuyback> EmployeeAssetBuybacks { get; set; }
        public DbSet<EmployeeAssetReturn> EmployeeAssetReturns { get; set; }
        public DbSet<RoleMasterModel> RoleMasterModels { get; set; }

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
            modelBuilder.Entity<RoleMasterModel>()
             .Property(x => x.CREATE_DATE)
             .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<RoleMasterModel>()
               .HasKey(x => x.ROLE_ID);
        }
       

    }
}
