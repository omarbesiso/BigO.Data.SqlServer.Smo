using System.Data;
using BigO.Data.SqlServer.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace BigO.Data.SqlServer.Smo.Utilities;

/// <summary>
///     Provides methods for reading the results of SQL Server stored procedures using the <see cref="StoredProcedure" />
///     class.
/// </summary>
public static class SmoProcReader
{
    /// <summary>
    ///     Executes the specified stored procedure using a new SQL connection and returns the results.
    /// </summary>
    /// <param name="connectionString">The connection string to use when creating a new SQL connection.</param>
    /// <param name="storedProcedure">The <see cref="StoredProcedure" /> to execute.</param>
    /// <returns>An instance of <see cref="SqlResultReaderResponse" /> containing the results of the executed stored procedure.</returns>
    /// <remarks>
    ///     This method opens a new SQL connection using the provided <paramref name="connectionString" /> and executes the
    ///     specified <paramref name="storedProcedure" />. The results are returned as an instance of
    ///     <see cref="SqlResultReaderResponse" />.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// string connectionString = "your_connection_string_here";
    /// StoredProcedure storedProcedure = GetStoredProcedure(); // Replace with your logic to get a StoredProcedure instance
    /// SqlResultReaderResponse response = SmoProcReader.GetSqlResults(connectionString, storedProcedure);
    /// // Process the response object
    /// ]]></code>
    /// </example>
    public static SqlResultReaderResponse GetSqlResults(string connectionString, StoredProcedure storedProcedure)
    {
        using var connection = new SqlConnection(connectionString);
        return GetSqlResults(connection, storedProcedure);
    }

    /// <summary>
    ///     Executes the specified stored procedure using an existing SQL connection and returns the results.
    /// </summary>
    /// <param name="connection">The <see cref="SqlConnection" /> to use for executing the stored procedure.</param>
    /// <param name="storedProcedure">The <see cref="StoredProcedure" /> to execute.</param>
    /// <returns>An instance of <see cref="SqlResultReaderResponse" /> containing the results of the executed stored procedure.</returns>
    /// <remarks>
    ///     This method uses the provided <paramref name="connection" /> and executes the specified
    ///     <paramref name="storedProcedure" />. The results are returned as an instance of
    ///     <see cref="SqlResultReaderResponse" />.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// SqlConnection connection = GetSqlConnection(); // Replace with your logic to get an SqlConnection instance
    /// StoredProcedure storedProcedure = GetStoredProcedure(); // Replace with your logic to get a StoredProcedure instance
    /// SqlResultReaderResponse response = SmoProcReader.GetSqlResults(connection, storedProcedure);
    /// // Process the response object
    /// ]]></code>
    /// </example>
    public static SqlResultReaderResponse GetSqlResults(SqlConnection connection, StoredProcedure storedProcedure)
    {
        var sqlParameterList = new SqlParameterList();

        foreach (StoredProcedureParameter parameter in storedProcedure.Parameters)
        {
            var sqlParameter = new SqlParameter(parameter.Name, parameter.SqlDbType());
            SetParameterValue(parameter, sqlParameter);
            sqlParameterList.Add(sqlParameter);
        }

        return SqlResultReader.GetSqlResults(connection, storedProcedure.Name, CommandType.StoredProcedure,
            sqlParameterList);
    }

    private static void SetParameterValue(StoredProcedureParameter parameter, SqlParameter sqlParameter)
    {
        switch (parameter.SqlDbType())
        {
            case SqlDbType.BigInt:
                sqlParameter.Value = 1;
                break;
            case SqlDbType.Bit:
                sqlParameter.Value = false;
                break;
            case SqlDbType.Date:
            case SqlDbType.DateTime:
            case SqlDbType.DateTime2:
            case SqlDbType.SmallDateTime:
            case SqlDbType.Time:
                sqlParameter.Value = DateTime.Now;
                break;
            case SqlDbType.Decimal:
            case SqlDbType.Float:
            case SqlDbType.Money:
            case SqlDbType.SmallMoney:
                sqlParameter.Value = 0.0;
                break;
            case SqlDbType.Int:
                sqlParameter.Value = 0;
                break;
            case SqlDbType.NVarChar:
            case SqlDbType.VarChar:
            case SqlDbType.NChar:
            case SqlDbType.Char:
                sqlParameter.Value = "A";
                break;
            case SqlDbType.UniqueIdentifier:
                sqlParameter.Value = Guid.Empty;
                break;
            case SqlDbType.Structured:
                sqlParameter.Value = new DataTable(); // create an empty DataTable
                break;
            default:
                sqlParameter.Value = DBNull.Value;
                break;
        }
    }
}