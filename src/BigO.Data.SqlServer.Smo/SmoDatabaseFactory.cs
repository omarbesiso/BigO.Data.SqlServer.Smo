using BigO.Core.Validation;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace BigO.Data.SqlServer.Smo;

/// <summary>
///     Provides utilities for creating <see cref="Microsoft.SqlServer.Management.Smo.Database" /> instances.
/// </summary>
[PublicAPI]
public static class SmoDatabaseFactory
{
    /// <summary>
    ///     Creates an instance of the <see cref="Database" /> class using the specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to the database. Cannot be <c>null</c> or whitespace.</param>
    /// <returns>An instance of the <see cref="Database" /> class.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the database specified in the connection string could not be
    ///     found.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown when the connectionString parameter is <c>null</c> or whitespace.</exception>
    /// <remarks>
    ///     This method attempts to create an instance of the <see cref="Database" /> class by connecting to the database
    ///     specified in the connection string.
    ///     It first validates the connection string to ensure it is not <c>null</c> or whitespace.
    ///     Then it creates a new <see cref="SqlConnection" /> object using the connection string and a new
    ///     <see cref="Server" /> object using the connection.
    ///     It then uses a <see cref="SqlConnectionStringBuilder" /> to extract the database name from the connection string
    ///     and searches the server's databases for a match.
    ///     If a match is found, an instance of the <see cref="Database" /> class is returned. If no match is found, an
    ///     <see cref="ArgumentOutOfRangeException" /> is thrown.
    /// </remarks>
    public static Database CreateInstance(string connectionString)
    {
        Guard.NotNullOrWhiteSpace(connectionString);

        var connection = new SqlConnection(connectionString);
        var server = new Server(new ServerConnection(connection));

        var builder = new SqlConnectionStringBuilder(connectionString);
        var databaseName = builder.InitialCatalog;

        var database = server.Databases.Cast<Database>()
            .FirstOrDefault(e => e.Name.Equals(databaseName, StringComparison.OrdinalIgnoreCase));

        if (database == null)
        {
            throw new ArgumentOutOfRangeException(databaseName,
                "The database specified in the connection string could not be found.");
        }

        return database;
    }

    /// <summary>
    ///     Creates a new instance of a <see cref="Database" /> object using the specified <paramref name="sqlConnection" />.
    /// </summary>
    /// <param name="sqlConnection">A <see cref="SqlConnection" /> object representing the connection to the database.</param>
    /// <returns>A <see cref="Database" /> object representing the database associated with the specified connection.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the database specified in the connection string cannot be found.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the <paramref name="sqlConnection" /> parameter is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method creates a new instance of a <see cref="Database" /> object using the specified
    ///     <paramref name="sqlConnection" />.
    ///     It first creates a new instance of a <see cref="Server" /> object using the specified connection, then it retrieves
    ///     the name of the database
    ///     from the connection string and searches for a database with that name among the databases on the server. If a
    ///     database with the specified name
    ///     is found, it is returned. Otherwise, an exception is thrown.
    /// </remarks>
    public static Database CreateInstance(SqlConnection sqlConnection)
    {
        Guard.NotNull(sqlConnection);

        var server = new Server(new ServerConnection(sqlConnection));

        var builder = new SqlConnectionStringBuilder(sqlConnection.ConnectionString);
        var databaseName = builder.InitialCatalog;

        var database = server.Databases.Cast<Database>()
            .FirstOrDefault(e => e.Name.Equals(databaseName, StringComparison.OrdinalIgnoreCase));

        if (database == null)
        {
            throw new ArgumentOutOfRangeException(databaseName,
                "The database specified in the connection string could not be found.");
        }

        return database;
    }

    /// <summary>
    ///     Creates an instance of the <see cref="Database" /> class using the specified server and database name.
    /// </summary>
    /// <param name="server">The server containing the database. Cannot be <c>null</c>.</param>
    /// <param name="databaseName">The name of the database. Cannot be <c>null</c> or whitespace.</param>
    /// <returns>An instance of the <see cref="Database" /> class.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified database could not be found in the server.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the server or databaseName parameter is <c>null</c>.</exception>
    /// <remarks>
    ///     This method attempts to create an instance of the <see cref="Database" /> class by searching the specified server's
    ///     databases for a match with the provided database name.
    ///     It first validates the server and database name to ensure they are not <c>null</c>.
    ///     Then it searches the server's databases for a match using the provided database name.
    ///     If a match is found, an instance of the <see cref="Database" /> class is returned. If no match is found, an
    ///     <see cref="ArgumentOutOfRangeException" /> is thrown.
    /// </remarks>
    public static Database GetDatabase(this Server server, string databaseName)
    {
        Guard.NotNull(server);
        Guard.NotNullOrWhiteSpace(databaseName);

        var database = server.Databases.Cast<Database>()
            .FirstOrDefault(e => e.Name.Equals(databaseName, StringComparison.OrdinalIgnoreCase));

        if (database == null)
        {
            throw new ArgumentOutOfRangeException(databaseName,
                "The database specified in the connection string could not be found.");
        }

        return database;
    }
}