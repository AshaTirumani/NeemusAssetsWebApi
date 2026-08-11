using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Data
{
    public class ERPDBContext : DbContext
    {
        public ERPDBContext(DbContextOptions<ERPDBContext> options)
            : base(options)
        {
        }

        public DbSet<ErpAssetModel> ErpAssets { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<ErpAssetModel>()
        //        .Property(x => x.CreationDate)
        //        .HasColumnType("timestamp without time zone");

        //    modelBuilder.Entity<ErpAssetModel>()
        //        .Property(x => x.FirstAcquisitionDate)
        //        .HasColumnType("timestamp without time zone");

        //    modelBuilder.Entity<ErpAssetModel>()
        //        .Property(x => x.AssetCapitalizationDate)
        //        .HasColumnType("timestamp without time zone");

        //    modelBuilder.Entity<ErpAssetModel>()
        //        .Property(x => x.WarrantyDate)
        //        .HasColumnType("timestamp without time zone");
        //}
    }
}