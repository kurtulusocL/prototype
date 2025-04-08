using Entities.Entities;
using Entities.Entities.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework.Context.MSSQL
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppUserRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Audit> Audits { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ExceptionLogger> ExceptionLoggers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<UnitInStock> UnitInStocks { get; set; }
    }
}
