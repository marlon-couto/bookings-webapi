using System;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Test.Helpers;

public class TestFixture : IDisposable
{
    // Initial context setup.
    public TestFixture()
    {
        DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid()
                .ToString()) // Each test class has its database, preventing side effects.
            .Options;
        Context = new TestDbContext(options);
        Context.Database.EnsureCreated();
    }

    public TestDbContext Context { get; }

    // Reset the database after the tests.
    public void Dispose()
    {
        Context.Dispose();
        GC.SuppressFinalize(this);
    }
}