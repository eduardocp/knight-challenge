using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Knight.Infrastructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Domain.Models.Knight> Knights { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Domain.Models.Knight>().OwnsMany(x => x.Weapons, a =>
            {
                a.ToTable("KnightWeapons");
            });

            base.OnModelCreating(builder);
        }
    }
}