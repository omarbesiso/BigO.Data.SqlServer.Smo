using JetBrains.Annotations;
using Microsoft.SqlServer.Management.Smo;

namespace BigO.Data.SqlServer.Smo;

/// <summary>
///     Contains extension methods for the <see cref="StoredProcedure" /> class.
/// </summary>
/// <remarks>
///     This class provides additional functionality for working with SQL Server stored procedures using the
///     <see cref="StoredProcedure" /> class.
/// </remarks>
[PublicAPI]
public static class SmoStoredProcedureExtensions
{
    /// <summary>
    ///     Returns the full name of a stored procedure, including its schema.
    /// </summary>
    /// <param name="smoStoredProcedure">The <see cref="StoredProcedure" /> instance.</param>
    /// <returns>A string containing the full name of the stored procedure.</returns>
    /// <remarks>
    ///     The <c>FullName</c> method concatenates the schema and the name of the stored procedure, separated by a dot.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// StoredProcedure sp = new StoredProcedure();
    /// sp.Schema = "dbo";
    /// sp.Name = "GetAllCustomers";
    /// string fullName = sp.FullName();
    /// // fullName: "dbo.GetAllCustomers"
    /// ]]></code>
    /// </example>
    public static string FullName(this StoredProcedure smoStoredProcedure)
    {
        var output = $"{smoStoredProcedure.Schema}.{smoStoredProcedure.Name}";
        return output;
    }

    /// <summary>
    ///     Returns the qualified full name of a stored procedure, including its schema and square brackets.
    /// </summary>
    /// <param name="smoStoredProcedure">The <see cref="StoredProcedure" /> instance.</param>
    /// <returns>A string containing the qualified full name of the stored procedure.</returns>
    /// <remarks>
    ///     The <c>FullNameQualified</c> method concatenates the schema and the name of the stored procedure, separated by a
    ///     dot, and wraps each part in square brackets. This can be useful when working with stored procedure names that
    ///     contain special characters or reserved keywords.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// StoredProcedure sp = new StoredProcedure();
    /// sp.Schema = "dbo";
    /// sp.Name = "GetAllCustomers";
    /// string fullQualifiedName = sp.FullQualifiedName();
    /// // fullQualifiedName: "[dbo].[GetAllCustomers]"
    /// ]]></code>
    /// </example>
    public static string FullQualifiedName(this StoredProcedure smoStoredProcedure)
    {
        var output = $"[{smoStoredProcedure.Schema}].[{smoStoredProcedure.Name}]";
        return output;
    }
}