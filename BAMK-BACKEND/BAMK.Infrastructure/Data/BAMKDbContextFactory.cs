using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BAMK.Infrastructure.Data
{
    public class BAMKDbContextFactory : IDesignTimeDbContextFactory<BAMKDbContext>
    {
        public BAMKDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BAMKDbContext>();
            
            // Development connection string
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=BAMK_DB_Dev;Trusted_Connection=true;TrustServerCertificate=true;";
            
            optionsBuilder.UseSqlServer(connectionString);
            
            return new BAMKDbContext(optionsBuilder.Options);
        }
    }
}
