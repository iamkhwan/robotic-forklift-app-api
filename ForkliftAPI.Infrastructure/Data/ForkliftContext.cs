using ForkliftAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ForkliftAPI.Infrastructure.Data
{
    public class ForkliftContext : DbContext
    {
        public ForkliftContext(DbContextOptions<ForkliftContext> options) : base(options) { }

        public DbSet<Forklift> Forklifts => Set<Forklift>();
    }

    // Design-time factory for EF Core tools
    public class ForkliftContextFactory : IDesignTimeDbContextFactory<ForkliftContext>
    {
        public ForkliftContext CreateDbContext(string[] args)
        {
            // Load config from appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ForkliftAPI.API"))
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ForkliftContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new ForkliftContext(optionsBuilder.Options);
        }
    }
}
