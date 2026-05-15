
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


    }
}
