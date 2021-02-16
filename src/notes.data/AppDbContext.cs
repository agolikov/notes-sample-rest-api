using Microsoft.EntityFrameworkCore;
using notes.data.Entities;

namespace notes.data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => { entity.HasIndex(e => e.Email).IsUnique(); });

            builder.Entity<Tag>(entity => { entity.HasIndex(e => e.Value).IsUnique(); });
        }
    }
}