using System.Data;
using JetBrains.Annotations;
using Microsoft.SqlServer.Management.Smo;

namespace BigO.Data.SqlServer.Smo;

/// <summary>
///     Provides utilities for conversions between <see cref="SqlDbType" /> and <see cref="SqlDataType" />
/// </summary>
[PublicAPI]
public static class SqlTypesConversionExtensions
{
    /// <summary>
    ///     Converts a <see cref="SqlDbType" /> to a <see cref="SqlDataType" />.
    /// </summary>
    /// <param name="sqlDbType">The <see cref="SqlDbType" /> to convert.</param>
    /// <returns>The corresponding <see cref="SqlDataType" />.</returns>
    /// <exception cref="NotSupportedException">Thrown if the <see cref="SqlDbType" /> is not supported.</exception>
    public static SqlDataType ToSqlDataType(this SqlDbType sqlDbType)
    {
        return sqlDbType switch
        {
            SqlDbType.BigInt => SqlDataType.BigInt,
            SqlDbType.Binary => SqlDataType.Binary,
            SqlDbType.Bit => SqlDataType.Bit,
            SqlDbType.Char => SqlDataType.Char,
            SqlDbType.Date => SqlDataType.Date,
            SqlDbType.DateTime => SqlDataType.DateTime,
            SqlDbType.DateTime2 => SqlDataType.DateTime2,
            SqlDbType.DateTimeOffset => SqlDataType.DateTimeOffset,
            SqlDbType.Decimal => SqlDataType.Decimal,
            SqlDbType.Float => SqlDataType.Float,
            SqlDbType.Image => SqlDataType.Image,
            SqlDbType.Int => SqlDataType.Int,
            SqlDbType.Money => SqlDataType.Money,
            SqlDbType.NChar => SqlDataType.NChar,
            SqlDbType.NText => SqlDataType.NText,
            SqlDbType.NVarChar => SqlDataType.NVarChar,
            SqlDbType.Real => SqlDataType.Real,
            SqlDbType.SmallDateTime => SqlDataType.SmallDateTime,
            SqlDbType.SmallInt => SqlDataType.SmallInt,
            SqlDbType.SmallMoney => SqlDataType.SmallMoney,
            SqlDbType.Structured => SqlDataType.UserDefinedTableType,
            SqlDbType.Text => SqlDataType.Text,
            SqlDbType.Time => SqlDataType.Time,
            SqlDbType.Timestamp => SqlDataType.Timestamp,
            SqlDbType.TinyInt => SqlDataType.TinyInt,
            SqlDbType.Udt => SqlDataType.UserDefinedType,
            SqlDbType.UniqueIdentifier => SqlDataType.UniqueIdentifier,
            SqlDbType.VarBinary => SqlDataType.VarBinary,
            SqlDbType.VarChar => SqlDataType.VarChar,
            SqlDbType.Variant => SqlDataType.Variant,
            SqlDbType.Xml => SqlDataType.Xml,
            _ => throw new NotSupportedException("The type is not supported.")
        };
    }

    /// <summary>
    ///     Converts a <see cref="SqlDataType" /> to a <see cref="SqlDbType" />.
    /// </summary>
    /// <param name="sqlDataType">The <see cref="SqlDataType" /> to convert.</param>
    /// <returns>The corresponding <see cref="SqlDbType" />.</returns>
    /// <exception cref="NotSupportedException">Thrown if the <see cref="SqlDataType" /> is not supported.</exception>
    public static SqlDbType ToSqlDbType(this SqlDataType sqlDataType)
    {
        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        return sqlDataType switch
        {
            SqlDataType.BigInt => SqlDbType.BigInt,
            SqlDataType.Binary => SqlDbType.Binary,
            SqlDataType.Bit => SqlDbType.Bit,
            SqlDataType.Char => SqlDbType.Char,
            SqlDataType.Date => SqlDbType.Date,
            SqlDataType.DateTime => SqlDbType.DateTime,
            SqlDataType.DateTime2 => SqlDbType.DateTime2,
            SqlDataType.DateTimeOffset => SqlDbType.DateTimeOffset,
            SqlDataType.Decimal => SqlDbType.Decimal,
            SqlDataType.Float => SqlDbType.Float,
            SqlDataType.Geography => SqlDbType.Udt,
            SqlDataType.Geometry => SqlDbType.Udt,
            SqlDataType.HierarchyId => SqlDbType.Udt,
            SqlDataType.Image => SqlDbType.Image,
            SqlDataType.Int => SqlDbType.Int,
            SqlDataType.Money => SqlDbType.Money,
            SqlDataType.NChar => SqlDbType.NChar,
            SqlDataType.NText => SqlDbType.NText,
            SqlDataType.Numeric => SqlDbType.Decimal,
            SqlDataType.NVarChar => SqlDbType.NVarChar,
            SqlDataType.NVarCharMax => SqlDbType.NVarChar,
            SqlDataType.Real => SqlDbType.Real,
            SqlDataType.SmallDateTime => SqlDbType.SmallDateTime,
            SqlDataType.SmallInt => SqlDbType.SmallInt,
            SqlDataType.SmallMoney => SqlDbType.SmallMoney,
            SqlDataType.Text => SqlDbType.Text,
            SqlDataType.Time => SqlDbType.Time,
            SqlDataType.Timestamp => SqlDbType.Timestamp,
            SqlDataType.TinyInt => SqlDbType.TinyInt,
            SqlDataType.UniqueIdentifier => SqlDbType.UniqueIdentifier,
            SqlDataType.UserDefinedDataType => SqlDbType.Structured,
            SqlDataType.UserDefinedTableType => SqlDbType.Structured,
            SqlDataType.UserDefinedType => SqlDbType.Structured,
            SqlDataType.VarBinary => SqlDbType.VarBinary,
            SqlDataType.VarBinaryMax => SqlDbType.VarBinary,
            SqlDataType.VarChar => SqlDbType.VarChar,
            SqlDataType.VarCharMax => SqlDbType.VarChar,
            SqlDataType.Variant => SqlDbType.Variant,
            SqlDataType.Xml => SqlDbType.Xml,
            _ => throw new NotSupportedException("The type is not supported.")
        };
    }
}