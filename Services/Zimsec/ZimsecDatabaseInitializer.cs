using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;

namespace PindahWebsite3.Services.Zimsec;

public static class ZimsecDatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ZimsecContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        await context.Database.EnsureCreatedAsync();

        var connectionString = configuration.GetConnectionString("ZimsecContextConnection")
            ?? "Data Source=zimsec.db";
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        await MigrateStudentPhoneColumnAsync(connection);

        await using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = """
                CREATE VIRTUAL TABLE IF NOT EXISTS DocumentSearch USING fts5(
                    DocumentId UNINDEXED,
                    Title,
                    Subject,
                    Level,
                    FileName,
                    Body,
                    tokenize='porter unicode61'
                );
                """;
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public static async Task SyncLibraryAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ZimsecLibraryIndexer>>();
        var indexer = scope.ServiceProvider.GetRequiredService<IZimsecLibraryIndexer>();
        try
        {
            var report = await indexer.SyncAsync();
            logger.LogInformation(
                "Zimsec library indexed: +{Added} ~{Updated} -{Removed} total={Total} failed={Failed}",
                report.Added, report.Updated, report.Removed, report.Total, report.Failed);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Zimsec library indexing failed");
        }
    }

    private static async Task MigrateStudentPhoneColumnAsync(SqliteConnection connection)
    {
        var hasPhone = await ColumnExistsAsync(connection, "Students", "PhoneNumber");
        if (hasPhone) return;

        var hasStudentNumber = await ColumnExistsAsync(connection, "Students", "StudentNumber");
        if (hasStudentNumber)
        {
            await using var rename = connection.CreateCommand();
            rename.CommandText = "ALTER TABLE Students RENAME COLUMN StudentNumber TO PhoneNumber;";
            await rename.ExecuteNonQueryAsync();
        }
    }

    private static async Task<bool> ColumnExistsAsync(SqliteConnection connection, string table, string column)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = $"PRAGMA table_info({table});";
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            if (string.Equals(reader.GetString(1), column, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }
}
