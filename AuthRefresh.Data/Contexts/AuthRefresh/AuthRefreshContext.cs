using Microsoft.EntityFrameworkCore;
using AuthRefresh.Data.Contexts.AuthRefresh.Models;

namespace AuthRefresh.Data
{
    public class AuthRefreshContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }

        public AuthRefreshContext(DbContextOptions<AuthRefreshContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(x => x.Name).HasMaxLength(25).IsRequired();
            });

            modelBuilder.Entity<Claim>(entity =>
            {
                entity.Property(x => x.Name).HasMaxLength(25).IsRequired();
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(x => x.UscId).HasMaxLength(10).IsRequired();
            });
        }

    }
}