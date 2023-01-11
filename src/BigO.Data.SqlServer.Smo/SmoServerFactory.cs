using BigO.Core.Validation;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace BigO.Data.SqlServer.Smo;

/// <summary>
///     Provides utilities for creating <see cref="Microsoft.SqlServer.Management.Smo.Server" /> instances.
/// </summary>
[PublicAPI]
public static class SmoServerFactory
{
    /// <summary>
    ///     Creates a new instance of <see cref="Server" /> using the specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to be used to connect to the server.</param>
    /// <returns>A new instance of <see cref="Server" />.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="connectionString" /> is <c>null</c> or white space.</exception>
    /// <remarks>
    ///     This method creates a new <see cref="SqlConnection" /> using the specified connection string and
    ///     then creates a new instance of <see cref="Server" /> using a new instance of <see cref="ServerConnection" />
    ///     initialized with the new <see cref="SqlConnection" />.
    /// </remarks>
    public static Server CreateInstance(string connectionString)
    {
        Guard.NotNullOrWhiteSpace(connectionString);

        var connection = new SqlConnection(connectionString);
        var serverConnection = new ServerConnection(connection);
        var server = new Server(serverConnection);
        return server;
    }

    /// <summary>
    ///     Creates an instance of <see cref="Server" /> using the specified <paramref name="sqlConnection" />.
    /// </summary>
    /// <param name="sqlConnection">
    ///     The <see cref="SqlConnection" /> to be used for creating the <see cref="Server" />
    ///     instance.
    /// </param>
    /// <returns>An instance of <see cref="Server" />.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="sqlConnection" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     The <paramref name="sqlConnection" /> must be opened before calling this method.
    ///     The <see cref="Server" /> instance returned by this method will use the specified <paramref name="sqlConnection" />
    ///     for all its operations.
    /// </remarks>
    public static Server CreateInstance(SqlConnection sqlConnection)
    {
        Guard.NotNull(sqlConnection);

        var serverConnection = new ServerConnection(sqlConnection);
        var server = new Server(serverConnection);
        return server;
    }

    /// <summary>
    ///     Creates a new Database object using the provided connection string
    /// </summary>
    /// <param name="connectionString">
    ///     The connection string to use to connect to the database. It must include the Initial
    ///     Catalog property
    /// </param>
    /// <returns>An instance of <see cref="Database" />.</returns>
    /// <exception cref="SqlException">Thrown if there is an error when connecting to the database</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="connectionString" /> is <c>null</c> or empty</exception>
    /// <remarks>
    ///     This method uses the Initial Catalog property of the provided connection string to identify the database to connect
    ///     to.
    /// </remarks>
    public static Database CreateDatabase(string connectionString)
    {
        var connection = new SqlConnection(connectionString);
        var builder = new SqlConnectionStringBuilder(connectionString);

        var serverConnection = new ServerConnection(connection);
        var sqlServer = new Server(serverConnection);

        return sqlServer.Databases[builder.InitialCatalog];
    }
}