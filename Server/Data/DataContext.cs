using Microsoft.EntityFrameworkCore;
using SouthAfricanNumbers.Shared;

namespace SouthAfricanNumbers.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Number> Numbers { get; set; }
    }
}
