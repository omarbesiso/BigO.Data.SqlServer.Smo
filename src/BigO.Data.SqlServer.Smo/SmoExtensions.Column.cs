using System.Data;
using BigO.Core.Extensions;
using BigO.Core.Validation;
using BigO.Data.SqlServer.Extensions;
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

    /// <summary>
    ///     Determines whether the provided <see cref="Column" /> has a description associated with it.
    /// </summary>
    /// <param name="smoColumn">The <see cref="Column" /> to check for a description.</param>
    /// <returns>True if the <see cref="Column" /> has a description, false otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="smoColumn" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method checks the <see cref="Column" />'s extended properties for a property with a name of "MS_Description"
    ///     and returns true if found.
    /// </remarks>
    public static bool HasDescription(this Column smoColumn)
    {
        if (smoColumn.ExtendedProperties.Count == 0)
        {
            return false;
        }

        var description = smoColumn.ExtendedProperties.Cast<ExtendedProperty>()
            .FirstOrDefault(e => e.IsMSDescription());

        return description != null;
    }

    /// <summary>
    ///     Returns the .NET data type string representation of the column.
    /// </summary>
    /// <param name="smoColumn">The <see cref="Column" /> object to retrieve the data type from.</param>
    /// <returns>A string representation of the column's data type.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="smoColumn" /> is <c>null</c>.
    /// </exception>
    public static string NetDataTypeString(this Column smoColumn)
    {
        var output = smoColumn.NetDataType().GetNameOrAlias();

        return output;
    }

    /// <summary>
    ///     Extension method that retrieves the .NET data type of a <see cref="Column" /> object.
    /// </summary>
    /// <param name="smoColumn">
    ///     The <see cref="Column" /> object to retrieve the .NET data type from.
    ///     <paramref name="smoColumn" /> should not be <c>null</c>.
    /// </param>
    /// <returns>
    ///     A <see cref="Type" /> representing the .NET data type of the <paramref name="smoColumn" /> object.
    /// </returns>
    /// <exception cref="ArgumentNullException">If <paramref name="smoColumn" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method uses the <c>Nullable</c> property of the <paramref name="smoColumn" /> to determine if the column is
    ///     nullable.
    ///     If it is, it uses the <c>ToSqlDbType().GetDotNetType(true)</c> method to get the .NET data type.
    ///     If it is not, it uses the <c>ToSqlDbType().GetDotNetType()</c> method to get the .NET data type.
    /// </remarks>
    public static Type NetDataType(this Column smoColumn)
    {
        var output = smoColumn.Nullable
            ? smoColumn.ToSqlDbType().GetDotNetType(true)
            : smoColumn.ToSqlDbType().GetDotNetType();

        return output;
    }

    /// <summary>
    ///     Extension method that checks if a <see cref="Column" /> object's data type is a string.
    /// </summary>
    /// <param name="smoColumn">
    ///     The <see cref="Column" /> object to check the data type of. <paramref name="smoColumn" />
    ///     should not be <c>null</c>.
    /// </param>
    /// <returns>
    ///     A <c>bool</c> value indicating whether the <paramref name="smoColumn" /> object's data type is a string.
    /// </returns>
    /// <exception cref="ArgumentNullException">If <paramref name="smoColumn" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method checks the <c>DataType.IsStringType</c> property of the <paramref name="smoColumn" /> object to
    ///     determine if it's data type is a string or not.
    /// </remarks>
    public static bool IsString(this Column smoColumn)
    {
        var output = smoColumn.DataType.IsStringType;

        return output;
    }

    /// <summary>
    ///     Determines whether the <see cref="Column" /> is of a numeric data type.
    /// </summary>
    /// <param name="smoColumn">The <see cref="Column" /> to check.</param>
    /// <returns>A boolean value indicating whether the <see cref="Column" /> is of a numeric data type.</returns>
    /// <remarks>
    ///     This method uses the <see cref="DataType.IsNumericType" /> property to determine whether the <see cref="Column" />
    ///     is of a numeric data type.
    /// </remarks>
    public static bool IsNumericType(this Column smoColumn)
    {
        var output = smoColumn.DataType.IsNumericType;
        return output;
    }

    /// <summary>
    ///     Retrieves the numeric precision of a <see cref="Column" /> object.
    /// </summary>
    /// <param name="smoColumn">The <see cref="Column" /> object whose numeric precision is to be retrieved.</param>
    /// <returns>The numeric precision of the <paramref name="smoColumn" /> object.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="smoColumn" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method uses the DataType property of the <paramref name="smoColumn" /> object to retrieve its numeric
    ///     precision.
    /// </remarks>
    public static int NumericPrecision(this Column smoColumn)
    {
        var output = smoColumn.DataType.NumericPrecision;
        return output;
    }

    /// <summary>
    ///     Retrieves the numeric scale of a <see cref="Column" /> object.
    /// </summary>
    /// <param name="smoColumn">The <see cref="Column" /> object whose numeric scale is to be retrieved.</param>
    /// <returns>The numeric scale of the <paramref name="smoColumn" /> object.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="smoColumn" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method uses the DataType property of the <paramref name="smoColumn" /> object to retrieve its numeric scale.
    /// </remarks>
    public static int NumericScale(this Column smoColumn)
    {
        var output = smoColumn.DataType.NumericScale;
        return output;
    }
}