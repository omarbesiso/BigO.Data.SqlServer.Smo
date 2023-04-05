using BigO.Core.Validation;
using JetBrains.Annotations;
using Microsoft.SqlServer.Management.Smo;

namespace BigO.Data.SqlServer.Smo;

/// <summary>
///     Provides useful utilities for SMO objects.
/// </summary>
[PublicAPI]
public static class SmoExtensions
{
    /// <summary>
    ///     Retrieves a table with a given name and schema name from a database.
    /// </summary>
    /// <param name="database">The <see cref="Database" /> to search for the table in.</param>
    /// <param name="tableName">The name of the table to retrieve.</param>
    /// <param name="schemaName">The name of the schema the table belongs to. The default value is "dbo".</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="database" />, <paramref name="tableName" /> or
    ///     <paramref name="schemaName" /> is <c>null</c> or whitespace.
    /// </exception>
    /// <returns>The <see cref="Table" /> with the specified name and schema name, or <c>null</c> if no such table is found.</returns>
    /// <remarks>
    ///     The search is case-insensitive.
    ///     The method is checking the parameters passed in if they are null or empty strings.
    /// </remarks>
    public static Table? GetTable(this Database database, string tableName, string schemaName = "dbo")
    {
        Guard.NotNull(database);
        Guard.NotNullOrWhiteSpace(tableName);
        Guard.NotNullOrWhiteSpace(schemaName);

        var databaseTables = database.Tables.Cast<Table>();
        var table = databaseTables.FirstOrDefault(
            t =>
                t.Schema.Equals(schemaName, StringComparison.OrdinalIgnoreCase) &&
                t.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));

        return table;
    }
}