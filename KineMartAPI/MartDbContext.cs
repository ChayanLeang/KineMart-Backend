using KineMartAPI.ModelEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KineMartAPI
{
    public class MartDbContext : IdentityDbContext<ApplicationUser>
    {
        public MartDbContext(DbContextOptions<MartDbContext> option) : base(option) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductProperty> ProductProperties { get; set; }
        public DbSet<Import> Imports { get; set; }
        public DbSet<Export> Exports { get; set; }
        public DbSet<ProductImport> ProductImports { get; set; }
        public DbSet<ProductExport> ProductExports { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }
    }
}
