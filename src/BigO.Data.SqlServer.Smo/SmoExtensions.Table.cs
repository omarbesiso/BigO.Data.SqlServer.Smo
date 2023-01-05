using Humanizer;
using Microsoft.SqlServer.Management.Smo;

// ReSharper disable InvertIf
// ReSharper disable LoopCanBeConvertedToQuery

namespace BigO.Data.SqlServer.Smo;

public static partial class SmoExtensions
{
    /// <summary>
    ///     Returns the plural form of the table's name.
    /// </summary>
    /// <param name="table">The <see cref="Table" /> whose name to pluralize.</param>
    /// <returns>The plural form of the table's name.</returns>
    /// <remarks>
    ///     This method uses the <c>Humanizer</c> library to pluralize the table's name. If the table object is
    ///     <c>null</c>, an exception will be thrown.
    /// </remarks>
    public static string PluralName(this Table table)
    {
        return table.Name.Pluralize();
    }

    /// <summary>
    ///     Returns the full name of the table in the format "schema.tableName".
    /// </summary>
    /// <param name="table">The <see cref="Table" /> whose full name to retrieve.</param>
    /// <returns>The full name of the table in the format "schema.tableName".</returns>
    public static string FullName(this Table table)
    {
        return $"{table.Schema}.{table.Name}";
    }

    /// <summary>
    ///     Returns the fully-qualified name of the table in the format "[schema].[tableName]".
    /// </summary>
    /// <param name="table">The <see cref="Table" /> whose fully-qualified name to retrieve.</param>
    /// <returns>The fully-qualified name of the table in the format "[schema].[tableName]".</returns>
    public static string FullQualifiedName(this Table table)
    {
        return $"[{table.Schema}].[{table.Name}]";
    }

    /// <summary>
    ///     Returns the description of the table, if one exists.
    /// </summary>
    /// <param name="table">The <see cref="Table" /> whose description to retrieve.</param>
    /// <returns>The description of the table, if one exists. Otherwise, returns <c>null</c>.</returns>
    /// <remarks>
    ///     This method iterates through the table's extended properties to find a property with the name "MS_Description".
    ///     If such a property is found, its value is returned as the table's description. If the table object is
    ///     <c>null</c>, an exception will be thrown.
    /// </remarks>
    public static string? Description(this Table table)
    {
        string? description = null;

        foreach (ExtendedProperty extendedProperty in table.ExtendedProperties)
        {
            if (extendedProperty.IsMSDescription())
            {
                description = extendedProperty.Value.ToString();
                break;
            }
        }

        return description;
    }
}