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
    }
}
