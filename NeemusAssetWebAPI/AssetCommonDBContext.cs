
using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI
{
    public class AssetCommonDBContext: DbContext
    {
        public AssetCommonDBContext(DbContextOptions<AssetCommonDBContext> options) : base(options)
        {

        }
        //public DbSet<Department> DepartmentMasters { get; set; }
    }
}
