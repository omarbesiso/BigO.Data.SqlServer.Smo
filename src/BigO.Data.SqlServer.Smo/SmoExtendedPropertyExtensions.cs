using Microsoft.SqlServer.Management.Smo;

namespace BigO.Data.SqlServer.Smo;

public static class SmoExtendedPropertyExtensions
{
    /// <summary>
    ///     Determines whether the extended property's name is "MS_Description".
    /// </summary>
    /// <param name="extendedProperty">The extended property to check.</param>
    /// <returns><c>true</c> if the extended property's name is "MS_Description"; otherwise, <c>false</c>.</returns>
    /// <remarks>
    ///     This method performs a case-insensitive comparison of the extended property's name with the string
    ///     "MS_Description".
    ///     If the name is equal to this string, the method returns <c>true</c>. Otherwise, it returns <c>false</c>.
    /// </remarks>
    public static bool IsMSDescription(this ExtendedProperty extendedProperty)
    {
        return extendedProperty.Name.Equals("MS_Description", StringComparison.OrdinalIgnoreCase);
    }
}