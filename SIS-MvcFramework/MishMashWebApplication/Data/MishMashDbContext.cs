using Microsoft.EntityFrameworkCore;
using MishMashWebApplication.Models;

namespace MishMashWebApplication.Data
{
    public class MishMashDbContext : DbContext
    {
        public MishMashDbContext()
        {
        }

        public MishMashDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Channel> Channels { get; set; }

        public DbSet<UserChannel> UsersChannels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Server=DESKTOP-TI6GEI6\SQLEXPRESS;Database=MishMash;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserChannel>()
                .HasKey(x => new { x.ChannelId, x.UserId });

            modelBuilder.Entity<UserChannel>()
                .HasOne(uc => uc.Channel)
                .WithMany(c => c.Followers);

            modelBuilder.Entity<UserChannel>()
                .HasOne(uc => uc.User)
                .WithMany(c => c.FollowedChannels);
        }
    }
}
