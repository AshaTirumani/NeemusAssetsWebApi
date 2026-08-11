
using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI.Models;
using static NeemusAssetWebAPI.Models.CustodianChangeRequestModel;
using static NeemusAssetWebAPI.Models.DocumentMappingModel;


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
        public DbSet<AssetParkingModel> AssetParkings { get; set; }
        public DbSet<SAPUpdateLogInfoModel> SAPUpdateLogInfos { get; set; }
        public DbSet<AssetDocumentMapping> AssetDocumentMappings { get; set; }
        public DbSet<AuditDetailsModel> AuditDetailsModels { get; set; }
        //public DbSet<GenerateQRModel> GenerateQRModels { get; set; }
        public DbSet<AssetAuditHistory> AssetAuditHistories { get; set; }
        public DbSet<ComplaintRegistration> ComplaintRegistrations { get; set; }
      

        public DbSet<ServiceTypeApproverModel> ServiceTypeApproverModels { get; set; }
        public DbSet<ServiceTypeEngineerModel> ServiceTypeEngineerModels { get; set; }
        public DbSet<ComplaintTransaction> ComplaintTransactions { get; set; }

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

            modelBuilder.Entity<AuditDetailsModel>()
               .Property(x => x.Date)
               .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<AssetAuditHistory>()
               .Property(x => x.AuditedDate)
               .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<AssetAuditHistory>()
               .Property(x => x.ApprovedDate)
               .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<AssetAuditHistory>()
               .Property(x => x.AdminDate)
               .HasColumnType("timestamp without time zone");
        }
       

    }
}
