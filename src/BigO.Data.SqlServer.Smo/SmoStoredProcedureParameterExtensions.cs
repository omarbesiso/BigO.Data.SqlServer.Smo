using System.Data;
using JetBrains.Annotations;
using Microsoft.SqlServer.Management.Smo;

namespace BigO.Data.SqlServer.Smo;

/// <summary>
///     Contains extension methods for the <see cref="StoredProcedureParameter" /> class.
/// </summary>
/// <remarks>
///     This class provides additional functionality for working with SQL Server stored procedure parameters using the
///     <see cref="StoredProcedureParameter" /> class.
/// </remarks>
[PublicAPI]
public static class SmoStoredProcedureParameterExtensions
{
    /// <summary>
    ///     Returns the <see cref="SqlDbType" /> of a <see cref="StoredProcedureParameter" />.
    /// </summary>
    /// <param name="parameter">The <see cref="StoredProcedureParameter" /> instance.</param>
    /// <returns>The <see cref="SqlDbType" /> corresponding to the data type of the parameter.</returns>
    /// <remarks>
    ///     The <c>SqlDbType</c> method provides a way to convert the data type of a stored procedure parameter from
    ///     <see cref="SqlDataType" /> used by SMO to <see cref="SqlDbType" /> used by ADO.NET.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// StoredProcedureParameter parameter = new StoredProcedureParameter();
    /// parameter.DataType = new DataType(SqlDataType.VarChar, 50);
    /// SqlDbType sqlDbType = parameter.SqlDbType();
    /// // sqlDbType: SqlDbType.VarChar
    /// ]]></code>
    /// </example>
    public static SqlDbType SqlDbType(this StoredProcedureParameter parameter)
    {
        return parameter.DataType.SqlDataType.ToSqlDbType();
    }
}