using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Test.Helpers;

public class TestFixture : IDisposable
{
    private readonly SqliteConnection _conn;

    // Initial context setup.
    public TestFixture()
    {
        _conn = new SqliteConnection("DataSource=:memory:");
        _conn.Open();
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(_conn)
            .Options;
        Context = new TestDbContext(options);
        Context.Database.EnsureCreated();
    }

    public TestDbContext Context { get; }

    // Reset the database after the tests.
    public void Dispose()
    {
        Context.Dispose();
        _conn.Dispose();
        GC.SuppressFinalize(this);
    }
}