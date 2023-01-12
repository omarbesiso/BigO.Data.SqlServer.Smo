using JetBrains.Annotations;
using Microsoft.SqlServer.Management.Smo;

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
}