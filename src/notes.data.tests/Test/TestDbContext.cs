using Microsoft.EntityFrameworkCore;

namespace notes.data.tests.Test
{
    public class TestDbContext : AppDbContext
    {
        public DbSet<TestEntity> TestEntities { get; set; }

        public TestDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
