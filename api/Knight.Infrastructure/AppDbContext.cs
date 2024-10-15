using Microsoft.EntityFrameworkCore;

namespace Knight.Infrastructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Domain.Models.Entities.Knight> Knights { get; set; }
    }
}