using System.Threading.Tasks;
using BookingsWebApi.Test.Context;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Test.Helpers;

public static class TestFixture
{
    // Initial context setup.
    public static TestBookingsDbContext CreateContext()
    {
        DbContextOptions<TestBookingsDbContext> options =
            new DbContextOptionsBuilder<TestBookingsDbContext>()
                .UseInMemoryDatabase("InMemoryDb")
                .Options;
        return new TestBookingsDbContext(options);
    }

    // Reset the database before the test.
    public static async Task ClearDatabase<TEntity>(
        this TestBookingsDbContext context,
        DbSet<TEntity> dbSet
    )
        where TEntity : class
    {
        dbSet.RemoveRange(dbSet);
        await context.SaveChangesAsync();
    }
}
