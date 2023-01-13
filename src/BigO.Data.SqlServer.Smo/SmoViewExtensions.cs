using BigO.Core.Validation;
using Humanizer;
using JetBrains.Annotations;
using Microsoft.SqlServer.Management.Smo;

namespace BigO.Data.SqlServer.Smo;

[PublicAPI]
public static class SmoViewExtensions
{
    /// <summary>
    ///     Returns the plural form of the view's name.
    /// </summary>
    /// <param name="view">The <see cref="View" /> whose name to pluralize.</param>
    /// <returns>The plural form of the view's name.</returns>
    /// <remarks>
    ///     This method uses the <c>Humanizer</c> library to pluralize the view's name. If the view object is
    ///     <c>null</c>, an exception will be thrown.
    /// </remarks>
    public static string PluralName(this View view)
    {
        return view.Name.Pluralize();
    }

    /// <summary>
    ///     Returns the full name of the view in the format "schema.viewName".
    /// </summary>
    /// <param name="view">The <see cref="View" /> whose full name to retrieve.</param>
    /// <returns>The full name of the view in the format "schema.viewName".</returns>
    public static string FullName(this View view)
    {
        return $"{view.Schema}.{view.Name}";
    }

    /// <summary>
    ///     Returns the fully-qualified name of the view in the format "[schema].[viewName]".
    /// </summary>
    /// <param name="view">The <see cref="View" /> whose fully-qualified name to retrieve.</param>
    /// <returns>The fully-qualified name of the view in the format "[schema].[viewName]".</returns>
    public static string FullQualifiedName(this View view)
    {
        return $"[{view.Schema}].[{view.Name}]";
    }

    /// <summary>
    ///     Returns the description of the view, if one exists.
    /// </summary>
    /// <param name="view">The <see cref="View" /> whose description to retrieve.</param>
    /// <returns>The description of the view, if one exists. Otherwise, returns <c>null</c>.</returns>
    /// <remarks>
    ///     This method iterates through the view's extended properties to find a property with the name "MS_Description".
    ///     If such a property is found, its value is returned as the view's description. If the view object is
    ///     <c>null</c>, an exception will be thrown.
    /// </remarks>
    public static string? Description(this View view)
    {
        string? description = null;

        foreach (ExtendedProperty extendedProperty in view.ExtendedProperties)
        {
            if (extendedProperty.IsMSDescription())
            {
                description = extendedProperty.Value.ToString();
                break;
            }
        }

        return description;
    }

    /// <summary>
    ///     Determines whether the specified view has a description.
    /// </summary>
    /// <param name="view">The view to check for a description.</param>
    /// <returns>
    ///     <c>true</c> if the specified view has a description; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="view" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method checks the ExtendedProperties collection of the specified SMO view for an ExtendedProperty object with
    ///     the property IsMSDescription set to true. If such an object is found, the method returns true, indicating that the
    ///     view has a description. If no such object is found, the method returns false.
    /// </remarks>
    public static bool HasDescription(this View view)
    {
        if (view.ExtendedProperties.Count == 0)
        {
            return false;
        }

        var description = view.ExtendedProperties.Cast<ExtendedProperty>()
            .FirstOrDefault(e => e.IsMSDescription());

        return description != null;
    }

    /// <summary>
    ///     Retrieves a column with a given name from a view.
    /// </summary>
    /// <param name="view">The <see cref="View" /> to search for the column in.</param>
    /// <param name="columnName">The name of the column to retrieve.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="view" /> or <paramref name="columnName" /> is
    ///     <c>null</c> or whitespace.
    /// </exception>
    /// <returns>The <see cref="Column" /> with the specified name, or <c>null</c> if no such column is found.</returns>
    /// <remarks>
    ///     The search is case-insensitive.
    ///     The method is checking the parameters passed in if they are null or empty strings.
    /// </remarks>
    public static Column? GetColumn(this View view, string columnName)
    {
        Guard.NotNull(view);
        Guard.NotNullOrWhiteSpace(columnName);

        var columns = view.Columns.Cast<Column>();
        var column = columns.FirstOrDefault(c => c.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));

        return column;
    }
}