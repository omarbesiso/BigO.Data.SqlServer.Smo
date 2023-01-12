using BigO.Core.Validation;
using JetBrains.Annotations;
using Microsoft.SqlServer.Management.Smo;
using Index = Microsoft.SqlServer.Management.Smo.Index;

namespace BigO.Data.SqlServer.Smo;

[PublicAPI]
public static class SmoIndexExtensions
{
    /// <summary>
    ///     Determines whether the specified index is a primary key index.
    /// </summary>
    /// <param name="index">The index to check.</param>
    /// <returns>
    ///     <c>true</c> if the specified index is a primary key index; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="index" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method checks the IndexKeyType property of the specified index against the value IndexKeyType.DriPrimaryKey
    ///     using a pattern matching. If the IndexKeyType property is equal to IndexKeyType.DriPrimaryKey, the method returns
    ///     true, indicating that the index is a primary key index. Otherwise, the method returns false.
    /// </remarks>
    public static bool IsPrimaryKeyIndex(this Index index)
    {
        return index is { IndexKeyType: IndexKeyType.DriPrimaryKey };
    }

    /// <summary>
    ///     Returns the indexed columns of the specified index.
    /// </summary>
    /// <param name="index">The index to retrieve indexed columns from.</param>
    /// <returns>
    ///     A <see cref="List{IndexedColumn}" /> containing the indexed columns of the specified index.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="index" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method uses LINQ to cast the IndexedColumns property of the specified index to a List of IndexedColumn objects
    ///     and return the list.
    /// </remarks>
    public static List<IndexedColumn> GetIndexedColumns(this Index index)
    {
        Guard.NotNull(index);
        return index.IndexedColumns.Cast<IndexedColumn>().ToList();
    }

    /// <summary>
    ///     Returns the names of the indexed columns of the specified index.
    /// </summary>
    /// <param name="index">The index to retrieve indexed column names from.</param>
    /// <returns>
    ///     A list of strings containing the names of the indexed columns of the specified index.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="index" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method calls the GetIndexedColumns extension method to retrieve a list of indexed columns for the specified
    ///     index. It then uses LINQ to select the Name property of each column and return a list of indexed column names.
    /// </remarks>
    public static List<string> GetIndexedColumnNames(this Index index)
    {
        Guard.NotNull(index);
        return index.GetIndexedColumns().Select(e => e.Name).ToList();
    }
}