using System.Data;
using BigO.Core.Validation;
using Microsoft.SqlServer.Management.Smo;

namespace BigO.Data.SqlServer.Smo;

public static partial class SmoExtensions
{
    /// <summary>
    ///     Returns the ordinal position of the column within its parent table.
    /// </summary>
    /// <param name="column">The <see cref="Column" /> whose ordinal position to retrieve.</param>
    /// <returns>The ordinal position of the column within its parent table.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="column" /> is <c>null</c>.</exception>
    /// <exception cref="IndexOutOfRangeException">Thrown if the column's ordinal position cannot be found.</exception>
    /// <remarks>
    ///     This method first checks if the <paramref name="column" /> is <c>null</c> and throws an
    ///     <see cref="ArgumentNullException" /> if it is. It then checks if the parent collection of the column is a
    ///     <see cref="ColumnCollection" /> and returns -1 if it is not. If the parent collection is a
    ///     <see cref="ColumnCollection" />, the method iterates through the collection and compares each column to the input
    ///     <paramref name="column" />. If a match is found, the ordinal position is returned. If the input column
    ///     is not found in the parent table's column collection, an <see cref="IndexOutOfRangeException" /> is thrown.
    /// </remarks>
    public static int Ordinal(this Column column)
    {
        Guard.NotNull(column);

        if (column.ParentCollection is not ColumnCollection columns)
        {
            return -1;
        }

        var columnCount = columns.Count;
        for (var i = 0; i < columnCount; i++)
        {
            if (columns[i] == column)
            {
                return i;
            }
        }

        throw new IndexOutOfRangeException("The column's ordinal cannot be found.");
    }

    /// <summary>
    ///     Returns the maximum length of the column's data.
    /// </summary>
    /// <param name="column">The <see cref="Column" /> whose maximum data length to retrieve.</param>
    /// <returns>The maximum length of the column's data.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="column" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method first checks if the <paramref name="column" /> is <c>null</c> and throws an
    ///     <see cref="ArgumentNullException" /> if it is. If the column object is not <c>null</c>, it returns the
    ///     maximum length of the data that the column can hold, as specified by the <see cref="Column.DataType" /> property.
    /// </remarks>
    public static int Length(this Column column)
    {
        Guard.NotNull(column);
        return column.DataType.MaximumLength;
    }

    /// <summary>
    ///     Returns the description of the column if it exists.
    /// </summary>
    /// <param name="column">The <see cref="Column" /> whose description to retrieve.</param>
    /// <returns>The description of the column, or <c>null</c> if no description exists.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="column" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method first checks if the <paramref name="column" /> is <c>null</c> and throws an
    ///     <see cref="ArgumentNullException" /> if it is. It then iterates through the column's extended properties
    ///     and checks if any of them are the Microsoft SQL Server-defined description property using the
    ///     <see cref="SmoExtensions.IsMSDescription" /> method. If a description is found, it is returned. If no
    ///     description is found, <c>null</c> is returned.
    /// </remarks>
    public static string? Description(this Column column)
    {
        Guard.NotNull(column);
        string? description = null;

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (ExtendedProperty extendedProperty in column.ExtendedProperties)
        {
            // ReSharper disable once InvertIf
            if (extendedProperty.IsMSDescription())
            {
                description = extendedProperty.Value.ToString();
                break;
            }
        }

        return description;
    }

    /// <summary>
    ///     Gets the numeric scale of the column.
    /// </summary>
    /// <param name="column">The column to get the numeric scale for.</param>
    /// <returns>The numeric scale of the column.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="column" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     The numeric scale is the number of digits to the right of the decimal point in a number.
    /// </remarks>
    public static int Scale(this Column column)
    {
        Guard.NotNull(column);
        return column.DataType.NumericScale;
    }

    /// <summary>
    ///     Returns a boolean value indicating whether the <paramref name="column" /> has a string data type.
    /// </summary>
    /// <param name="column">The column to check the data type of.</param>
    /// <returns>True if the column has a string data type, false otherwise.</returns>
    /// <remarks>
    ///     A string data type is defined as any of the following: <see cref="SqlDataType.Char" />,
    ///     <see cref="SqlDataType.NChar" />, <see cref="SqlDataType.NText" />, <see cref="SqlDataType.NVarChar" />,
    ///     <see cref="SqlDataType.NVarCharMax" />, <see cref="SqlDataType.Text" />, <see cref="SqlDataType.VarChar" />,
    ///     <see cref="SqlDataType.VarCharMax" />.
    /// </remarks>
    public static bool HasAStringType(this Column column)
    {
        Guard.NotNull(column);
        return column.DataType.IsStringType;
    }

    /// <summary>
    ///     Converts the <see cref="SqlDataType" /> of the <paramref name="column" /> to a <see cref="SqlDbType" />.
    /// </summary>
    /// <param name="column">The <see cref="Column" /> to convert the <see cref="SqlDataType" /> of.</param>
    /// <returns>The <see cref="SqlDbType" /> of the <paramref name="column" />.</returns>
    /// <exception cref="NotSupportedException">
    ///     Thrown when the <see cref="SqlDataType" /> of the <paramref name="column" /> is
    ///     not supported.
    /// </exception>
    /// <remarks>
    ///     This method calls the <see cref="SqlTypesConversionExtensions.ToSqlDbType" /> method on the
    ///     <see cref="SqlDataType" /> of the <paramref name="column" />.
    /// </remarks>
    public static SqlDbType ToSqlDbType(this Column column)
    {
        return column.DataType.SqlDataType.ToSqlDbType();
    }
}