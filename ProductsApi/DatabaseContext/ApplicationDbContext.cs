using Microsoft.EntityFrameworkCore;
using ProductsApi.Models;


namespace ProductsApi.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        // Sequence for ProductId generation.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // For product ids (6-digit starting from 100000)
            modelBuilder.HasSequence<int>("ProductSequence", "dbo")
                .StartsAt(100000)
                .IncrementsBy(1);

            modelBuilder.Entity<Product>(entity =>
            {
                // Auto-generating ProductId without using a sequence
                entity.Property(p => p.ProductId)
                      .HasDefaultValueSql("NEXT VALUE FOR dbo.ProductSequence");
                // Specifying precision and scale for Price
                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");
            });
        }
    }
}
