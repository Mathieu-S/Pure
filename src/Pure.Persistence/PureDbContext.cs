using Microsoft.EntityFrameworkCore;
using Pure.Domain.Models;

namespace Pure.Persistence
{
    /// <summary>
    /// The main DbContext of Pure application.
    /// </summary>
    /// <remarks>
    /// The default <see cref="QueryTrackingBehavior"/> is NoTracking.
    /// </remarks>
    public sealed class PureDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        
        public PureDbContext()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        
        public PureDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Is only false when used with the EFCore CLI.
            // Used only for made a migration. The connection string is useless in production.
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=PureWebApi;Username=postgres;Password=admin");
            }
        }
    }
}