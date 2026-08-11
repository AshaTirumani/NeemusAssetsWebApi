using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI
{
    public class AssetSAPDBContext:DbContext
    {
        public AssetSAPDBContext(DbContextOptions<AssetSAPDBContext> options) : base(options)
        {

        }
        public DbSet<AssetClass> AssetClasss { get; set; }
        public DbSet<AssetTypeModel> AssetTypeModels { get; set; }
        public DbSet<AssetModel> AssetModels { get; set; }
        public DbSet<SAPUpdateLogInfoModel> SAPUpdateLogInfos { get; set; }
        public DbSet<LocationMaster> LocationMasters { get; set; }
        public DbSet<RfidMapping> RFIDMappingHistories { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetModel>()
                .Property(x => x.CreationDate)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<AssetModel>()
                .Property(x => x.FirstAcquisitionDate)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<AssetModel>()
                .Property(x => x.AssetCapitalizationDate)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<AssetModel>()
                .Property(x => x.WarrantyDate)
                .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<AssetTypeModel>()
               .Property(x => x.CreatedDate)
               .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<AssetClass>()
               .Property(x => x.CreatedDate)
               .HasColumnType("timestamp without time zone");
        }
    }
}
