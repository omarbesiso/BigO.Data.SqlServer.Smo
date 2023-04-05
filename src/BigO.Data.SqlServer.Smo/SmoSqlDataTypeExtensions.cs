using System.Data;
using JetBrains.Annotations;
using Microsoft.SqlServer.Management.Smo;

// ReSharper disable InvalidXmlDocComment

namespace BigO.Data.SqlServer.Smo;

[PublicAPI]
public static class SmoSqlDataTypeExtensions
{
    /// <summary>
    ///     Extension method that returns the SQL declaration for a <see cref="DataType" />.
    /// </summary>
    /// <param name="dataType">The <see cref="DataType" /> for which to generate the SQL declaration.</param>
    /// <returns>A string containing the SQL declaration for the <see cref="DataType" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the <see cref="SqlDataType" /> of
    ///     <paramref name="dataType" /> is not a valid enumeration value.
    /// </exception>
    /// <remarks>
    ///     This method uses the <c>SqlDataType</c> property of the <paramref name="dataType" /> object to determine the
    ///     appropriate SQL declaration to return.
    ///     It also takes into account any additional properties such as <c>MaximumLength</c>, <c>NumericPrecision</c> and
    ///     <c>NumericScale</c> if applicable.
    /// </remarks>
    public static string SqlTypeDeclaration(this DataType dataType)
    {
        var sqlDataType = dataType.SqlDataType;

        switch (sqlDataType)
        {
            case SqlDataType.BigInt:
                return "BIGINT";
            case SqlDataType.Binary:
                return $"BINARY({dataType.MaximumLength})";
            case SqlDataType.Bit:
                return "BIT";
            case SqlDataType.Char:
                return $"CHAR({dataType.MaximumLength})";
            case SqlDataType.Date:
                return "DATE";
            case SqlDataType.DateTime:
                return "DATETIME";
            case SqlDataType.DateTime2:
                return "DATETIME2";
            case SqlDataType.DateTimeOffset:
                return "DATETIMEOFFSET";
            case SqlDataType.Decimal:
                return $"DECIMAL({dataType.NumericPrecision}, {dataType.NumericScale})";
            case SqlDataType.Float:
                return "FLOAT";
            case SqlDataType.Geography:
                return "GEOGRAPHY";
            case SqlDataType.Geometry:
                return "GEOMETRY";
            case SqlDataType.HierarchyId:
                return "HIERARCHYID";
            case SqlDataType.Image:
                return "IMAGE";
            case SqlDataType.Int:
                return "INT";
            case SqlDataType.Money:
                return "MONEY";
            case SqlDataType.NChar:
                return $"NCHAR({dataType.MaximumLength})";
            case SqlDataType.NText:
                return "NTEXT";
            case SqlDataType.Numeric:
                return $"NUMERIC({dataType.NumericPrecision}, {dataType.NumericScale})";
            case SqlDataType.NVarChar:
                return $"NVARCHAR({dataType.MaximumLength})";
            case SqlDataType.NVarCharMax:
                return "NVARCHAR(MAX)";
            case SqlDataType.Real:
                return "REAL";
            case SqlDataType.SmallDateTime:
                return "SMALLDATETIME";
            case SqlDataType.SmallInt:
                return "SMALLINT";
            case SqlDataType.SmallMoney:
                return "SMALLMONEY";
            case SqlDataType.SysName:
                return "sysname";
            case SqlDataType.Text:
                return "TEXT";
            case SqlDataType.Time:
                return "TIME";
            case SqlDataType.Timestamp:
                return "TIMESTAMP";
            case SqlDataType.TinyInt:
                return "TINYINT";
            case SqlDataType.VarBinary:
                return $"VARBINARY({dataType.MaximumLength})";
            case SqlDataType.VarBinaryMax:
                return "VARBINARY(MAX)";
            case SqlDataType.VarChar:
                return $"VARCHAR({dataType.MaximumLength})";
            case SqlDataType.VarCharMax:
                return "VARCHAR(MAX)";
            case SqlDataType.Variant:
                return "SQL_VARIANT";
            case SqlDataType.UniqueIdentifier:
                return "UNIQUEIDENTIFIER";
            case SqlDataType.None:
            case SqlDataType.UserDefinedDataType:
            case SqlDataType.UserDefinedType:
            case SqlDataType.UserDefinedTableType:
            default:
                throw new ArgumentOutOfRangeException(nameof(sqlDataType), sqlDataType,
                    $"The value of {nameof(sqlDataType)} is not supported.");
        }
    }

    /// <summary>
    ///     Converts a <see cref="SqlDataType" /> value to its corresponding <see cref="SqlDbType" /> value.
    /// </summary>
    /// <param name="sqlDataType">The <see cref="SqlDataType" /> value to convert.</param>
    /// <returns>The corresponding <see cref="SqlDbType" /> value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the provided <paramref name="sqlDataType" /> is not
    ///     supported.
    /// </exception>
    /// <remarks>
    ///     This method provides a way to convert <see cref="SqlDataType" /> values, which are typically used with SMO objects,
    ///     to their equivalent <see cref="SqlDbType" /> values, which are used with ADO.NET objects.
    ///     Some <see cref="SqlDataType" /> values do not have direct equivalents in <see cref="SqlDbType" />. In these cases,
    ///     the method will throw an <see cref="ArgumentOutOfRangeException" />. Unsupported values include:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>
    ///                 <see cref="SqlDataType.None" />
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 <see cref="SqlDataType.SysName" />
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 <see cref="SqlDataType.HierarchyId" />
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 <see cref="SqlDataType.Geometry" />
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 <see cref="SqlDataType.Geography" />
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// SqlDataType sqlDataType = SqlDataType.VarChar;
    /// SqlDbType sqlDbType = sqlDataType.ASqlDbType();
    /// // sqlDbType: SqlDbType.VarChar
    /// ]]></code>
    /// </example>
    /// <remarks />
    public static SqlDbType ASqlDbType(this SqlDataType sqlDataType)
    {
        switch (sqlDataType)
        {
            case SqlDataType.BigInt:
                return SqlDbType.BigInt;
            case SqlDataType.Binary:
                return SqlDbType.Binary;
            case SqlDataType.Bit:
                return SqlDbType.Bit;
            case SqlDataType.Char:
                return SqlDbType.Char;
            case SqlDataType.DateTime:
                return SqlDbType.DateTime;
            case SqlDataType.Decimal:
                return SqlDbType.Decimal;
            case SqlDataType.Float:
                return SqlDbType.Float;
            case SqlDataType.Image:
                return SqlDbType.Image;
            case SqlDataType.Int:
                return SqlDbType.Int;
            case SqlDataType.Money:
                return SqlDbType.Money;
            case SqlDataType.NChar:
                return SqlDbType.NChar;
            case SqlDataType.NText:
                return SqlDbType.NText;
            case SqlDataType.NVarChar:
                return SqlDbType.NVarChar;
            case SqlDataType.NVarCharMax:
                return SqlDbType.NVarChar;
            case SqlDataType.Real:
                return SqlDbType.Real;
            case SqlDataType.SmallDateTime:
                return SqlDbType.SmallDateTime;
            case SqlDataType.SmallInt:
                return SqlDbType.SmallInt;
            case SqlDataType.SmallMoney:
                return SqlDbType.SmallMoney;
            case SqlDataType.Text:
                return SqlDbType.Text;
            case SqlDataType.Timestamp:
                return SqlDbType.Timestamp;
            case SqlDataType.TinyInt:
                return SqlDbType.TinyInt;
            case SqlDataType.UniqueIdentifier:
                return SqlDbType.UniqueIdentifier;
            case SqlDataType.UserDefinedDataType:
                return SqlDbType.Udt;
            case SqlDataType.UserDefinedType:
                return SqlDbType.Udt;
            case SqlDataType.VarBinary:
                return SqlDbType.VarBinary;
            case SqlDataType.VarBinaryMax:
                return SqlDbType.VarBinary;
            case SqlDataType.VarChar:
                return SqlDbType.VarChar;
            case SqlDataType.VarCharMax:
                return SqlDbType.VarChar;
            case SqlDataType.Variant:
                return SqlDbType.Variant;
            case SqlDataType.Xml:
                return SqlDbType.Xml;
            case SqlDataType.Numeric:
                return SqlDbType.Real;
            case SqlDataType.Date:
                return SqlDbType.Date;
            case SqlDataType.Time:
                return SqlDbType.Time;
            case SqlDataType.DateTimeOffset:
                return SqlDbType.DateTimeOffset;
            case SqlDataType.DateTime2:
                return SqlDbType.DateTime2;
            case SqlDataType.UserDefinedTableType:
                return SqlDbType.Structured;
            case SqlDataType.HierarchyId:
            case SqlDataType.Geometry:
            case SqlDataType.Geography:
            case SqlDataType.SysName:
            case SqlDataType.None:
            default:
                throw new ArgumentOutOfRangeException(nameof(SqlDataType), sqlDataType,
                    $"The value of {nameof(sqlDataType)} is not supported.");
        }
    }

    /// <summary>
    ///     Converts a <see cref="SqlDbType" /> value to its corresponding <see cref="SqlDataType" /> value.
    /// </summary>
    /// <param name="sqlDbType">The <see cref="SqlDbType" /> value to convert.</param>
    /// <returns>The corresponding <see cref="SqlDataType" /> value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided <paramref name="sqlDbType" /> is not supported.</exception>
    /// <remarks>
    ///     This method provides a way to convert <see cref="SqlDbType" /> values, which are typically used with ADO.NET
    ///     objects, to their equivalent <see cref="SqlDataType" /> values, which are used with SMO objects.
    ///     Some <see cref="SqlDbType" /> values do not have direct equivalents in <see cref="SqlDataType" />. In these cases,
    ///     the method will throw an <see cref="ArgumentOutOfRangeException" />. Unsupported values include:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>
    ///                 <see cref="SqlDbType.None" />
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 <see cref="SqlDbType.SysName" />
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 <see cref="SqlDbType.HierarchyId" />
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 <see cref="SqlDbType.Geometry" />
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 <see cref="SqlDbType.Geography" />
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// SqlDbType sqlDbType = SqlDbType.VarChar;
    /// SqlDataType sqlDataType = sqlDbType.ASqlDataType();
    /// // sqlDataType: SqlDataType.VarChar
    /// ]]></code>
    /// </example>
    /// <remarks />
    public static SqlDataType ASqlDbType(this SqlDbType sqlDbType)
    {
        switch (sqlDbType)
        {
            case SqlDbType.BigInt:
                return SqlDataType.BigInt;
            case SqlDbType.Binary:
                return SqlDataType.Binary;
            case SqlDbType.Bit:
                return SqlDataType.Bit;
            case SqlDbType.Char:
                return SqlDataType.Char;
            case SqlDbType.DateTime:
                return SqlDataType.DateTime;
            case SqlDbType.Decimal:
                return SqlDataType.Decimal;
            case SqlDbType.Float:
                return SqlDataType.Float;
            case SqlDbType.Image:
                return SqlDataType.Image;
            case SqlDbType.Int:
                return SqlDataType.Int;
            case SqlDbType.Money:
                return SqlDataType.Money;
            case SqlDbType.NChar:
                return SqlDataType.NChar;
            case SqlDbType.NText:
                return SqlDataType.NText;
            case SqlDbType.NVarChar:
                return SqlDataType.NVarChar;
            case SqlDbType.Real:
                return SqlDataType.Real;
            case SqlDbType.UniqueIdentifier:
                return SqlDataType.UniqueIdentifier;
            case SqlDbType.SmallDateTime:
                return SqlDataType.SmallDateTime;
            case SqlDbType.SmallInt:
                return SqlDataType.SmallInt;
            case SqlDbType.SmallMoney:
                return SqlDataType.SmallMoney;
            case SqlDbType.Text:
                return SqlDataType.Text;
            case SqlDbType.Timestamp:
                return SqlDataType.Timestamp;
            case SqlDbType.TinyInt:
                return SqlDataType.TinyInt;
            case SqlDbType.VarBinary:
                return SqlDataType.VarBinary;
            case SqlDbType.VarChar:
                return SqlDataType.VarChar;
            case SqlDbType.Variant:
                return SqlDataType.Variant;
            case SqlDbType.Xml:
                return SqlDataType.Xml;
            case SqlDbType.Udt:
                return SqlDataType.UserDefinedDataType;
            case SqlDbType.Structured:
                return SqlDataType.UserDefinedTableType;
            case SqlDbType.Date:
                return SqlDataType.Date;
            case SqlDbType.Time:
                return SqlDataType.Time;
            case SqlDbType.DateTime2:
                return SqlDataType.DateTime2;
            case SqlDbType.DateTimeOffset:
                return SqlDataType.DateTimeOffset;
            default:
                throw new ArgumentOutOfRangeException(nameof(SqlDbType), sqlDbType,
                    $"The value of {nameof(SqlDbType)} is not supported.");
        }
    }
}