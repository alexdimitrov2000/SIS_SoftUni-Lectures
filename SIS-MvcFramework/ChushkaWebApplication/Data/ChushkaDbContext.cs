using ChushkaWebApplication.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ChushkaWebApplication.Data
{
    public class ChushkaDbContext : DbContext
    {
        public ChushkaDbContext()
        {
        }

        public ChushkaDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(@"Server=DESKTOP-TI6GEI6\SQLEXPRESS;Database=Chushka;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }
    }
}
