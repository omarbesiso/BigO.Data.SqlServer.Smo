using BigO.Core.Validation;
using Humanizer;
using JetBrains.Annotations;
using Microsoft.SqlServer.Management.Smo;
using Index = Microsoft.SqlServer.Management.Smo.Index;

// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

// ReSharper disable InvertIf
// ReSharper disable LoopCanBeConvertedToQuery

namespace BigO.Data.SqlServer.Smo;

[PublicAPI]
public static class SmoTableExtensions
{
    private const string MSDescriptionExtendedPropertyName = "MS_Description";

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

    /// <summary>
    ///     Retrieves a column with a given name from a table.
    /// </summary>
    /// <param name="table">The <see cref="Table" /> to search for the column in.</param>
    /// <param name="columnName">The name of the column to retrieve.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="table" /> or <paramref name="columnName" /> is
    ///     <c>null</c> or whitespace.
    /// </exception>
    /// <returns>The <see cref="Column" /> with the specified name, or <c>null</c> if no such column is found.</returns>
    /// <remarks>
    ///     The search is case-insensitive.
    ///     The method is checking the parameters passed in if they are null or empty strings.
    /// </remarks>
    public static Column? GetColumn(this Table table, string columnName)
    {
        Guard.NotNull(table);
        Guard.NotNullOrWhiteSpace(columnName);

        var columns = table.Columns.Cast<Column>();
        var column = columns.FirstOrDefault(c => c.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));

        return column;
    }

    /// <summary>
    ///     Retrieves an index with a given name from a table.
    /// </summary>
    /// <param name="table">The <see cref="Table" /> to search for the index in.</param>
    /// <param name="indexName">The name of the index to retrieve.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="table" /> or <paramref name="indexName" /> is
    ///     <c>null</c> or whitespace.
    /// </exception>
    /// <returns>The <see cref="Index" /> with the specified name, or <c>null</c> if no such index is found.</returns>
    /// <remarks>
    ///     The search is case-insensitive.
    ///     The method is checking the parameters passed in if they are null or empty strings.
    /// </remarks>
    public static Index? GetIndex(this Table table, string indexName)
    {
        Guard.NotNull(table);
        Guard.NotNullOrWhiteSpace(indexName);

        var indexes = table.Indexes.Cast<Index>();
        var index = indexes.FirstOrDefault(i => i.Name.Equals(indexName, StringComparison.OrdinalIgnoreCase));

        return index;
    }

    /// <summary>
    ///     Adds a non-clustered index to a table on a specified set of columns
    /// </summary>
    /// <param name="table">The <see cref="Table" /> to add the index to.</param>
    /// <param name="columnNames">The names of the columns to add the index to.</param>
    /// <param name="indexName">
    ///     The name of the index. If not specified it will be generated automatically as "IX_" + tableName
    ///     + "_" + columnName(s) (underscored-separated).
    /// </param>
    /// <param name="indexComment">The comment to add to the index.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="table" /> or <paramref name="columnNames" /> is
    ///     <c>null</c> or empty.
    /// </exception>
    /// <remarks>
    ///     This method creates a new non-clustered index, sets the indexed columns and add it to the table.
    ///     If an index comment is provided, it will also add an extended property for the index.
    /// </remarks>
    public static void AddNonClusteredIndex(this Table table, IEnumerable<string> columnNames, string? indexName = null,
        string? indexComment = null)
    {
        Guard.NotNull(table);
        var internalColumnNames = columnNames as string[] ?? columnNames.ToArray();
        Guard.NotNullOrEmpty(internalColumnNames);

        var tableName = table.Name;
        var idxName = string.IsNullOrWhiteSpace(indexName)
            ? $"IX_{tableName}_{string.Join('_', internalColumnNames)}"
            : indexName;

        var nonClusteredIndex = new Index(table, idxName)
        {
            IndexKeyType = IndexKeyType.None,
            IndexType = IndexType.NonClusteredIndex
        };

        if (!string.IsNullOrWhiteSpace(indexComment))
        {
            var indexDescription = new ExtendedProperty(nonClusteredIndex, MSDescriptionExtendedPropertyName,
                indexComment);

            nonClusteredIndex.ExtendedProperties.Add(indexDescription);
        }

        foreach (var columnName in internalColumnNames)
        {
            nonClusteredIndex.IndexedColumns.Add(new IndexedColumn(nonClusteredIndex, columnName));
        }

        table.Indexes.Add(nonClusteredIndex);
    }

    /// <summary>
    ///     Adds a new column to a table
    /// </summary>
    /// <param name="table">The <see cref="Table" /> to add the column to.</param>
    /// <param name="columnName">The name of the column to be added.</param>
    /// <param name="dataType">The <see cref="DataType" /> of the column to be added.</param>
    /// <param name="isNullable">A flag indicating if the column is nullable, defaults to true.</param>
    /// <param name="description">The description of the column.</param>
    /// <param name="defaultValue">The default value of the column.</param>
    /// <param name="persistChange">if set to <c>true</c> persists the table changes to the database.</param>
    /// <returns>The newly created <see cref="Column" /> object.</returns>
    /// <exception cref="System.InvalidOperationException">A column with the same name exists in the table.</exception>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="table" /> or <paramref name="columnName" /> is
    ///     <c>null</c> or whitespace.
    /// </exception>
    /// <remarks>
    ///     The method will ensure that the column doesn't exist before adding it to the table.
    ///     If the description is provided, it will also add it to the column.
    ///     If the default value is provided, it will also set it as the default constraint of the column.
    /// </remarks>
    public static Column AddColumn(this Table table, string columnName, DataType dataType, bool isNullable = true,
        string? description = null, string? defaultValue = null, bool persistChange = true)
    {
        Guard.NotNull(table);
        Guard.NotNullOrWhiteSpace(columnName);

        table.Refresh();

        var column = table.GetColumn(columnName);

        if (column != null)
        {
            throw new InvalidOperationException("A column with the same name exists in the table.");
        }

        column = new Column(table, columnName, dataType)
        {
            Nullable = isNullable
        };

        table.Columns.Add(column);

        if (!string.IsNullOrWhiteSpace(description))
        {
            column.AddDescription(description);
        }

        if (!string.IsNullOrWhiteSpace(defaultValue))
        {
            column.SetDefaultConstraint(defaultValue);
        }

        if (persistChange)
        {
            table.Alter();
            table.Refresh();
        }

        return column;
    }

    /// <summary>
    ///     Checks if a table has a foreign key with a specific name
    /// </summary>
    /// <param name="table">The <see cref="Table" /> to check if it has the foreign key</param>
    /// <param name="foreignKeyName">The name of the foreign key</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="table" /> or <paramref name="foreignKeyName" /> is
    ///     <c>null</c> or whitespace.
    /// </exception>
    /// <returns>True if the table has the foreign key, otherwise false</returns>
    /// <remarks>
    ///     This method will check if any foreign key in the table has the name foreignKeyName(case-insensitive).
    /// </remarks>
    public static bool HasForeignKey(this Table table, string foreignKeyName)
    {
        Guard.NotNull(table);
        Guard.NotNullOrWhiteSpace(foreignKeyName);

        return table.ForeignKeys.Cast<ForeignKey>()
            .Any(fk => fk.Name.Equals(foreignKeyName, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    ///     Add a foreign key to a table
    /// </summary>
    /// <param name="table">The <see cref="Table" /> to add the foreign key to.</param>
    /// <param name="columnName">The name of the column on which the foreign key is created.</param>
    /// <param name="referencedColumn">The name of the column of the referenced table.</param>
    /// <param name="referencedTable">The name of the referenced table.</param>
    /// <param name="referencedTableSchema">The schema of the referenced table</param>
    /// <param name="foreignKeyName">The name of the foreign key.</param>
    /// <param name="foreignKeyDescription">The description of the foreign key.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when any of the parameters <paramref name="table" />,
    ///     <paramref name="columnName" />, <paramref name="referencedColumn" />, <paramref name="referencedTable" />, or
    ///     <paramref name="referencedTableSchema" /> is <c>null</c> or whitespace.
    /// </exception>
    /// <exception cref="InvalidOperationException">Thrown when a foreign key with the same name already exists in the table.</exception>
    /// <returns>The newly created <see cref="ForeignKey" /> object.</returns>
    /// <remarks>
    ///     The method will ensure that the foreign key doesn't exist before adding it to the table.
    ///     If the description is provided, it will also add it to the foreign key.
    /// </remarks>
    public static ForeignKey AddForeignKey(this Table table, string columnName, string referencedColumn,
        string referencedTable, string referencedTableSchema, string? foreignKeyName = null,
        string? foreignKeyDescription = null)
    {
        Guard.NotNull(table);
        Guard.NotNullOrWhiteSpace(columnName);
        Guard.NotNullOrWhiteSpace(referencedColumn);
        Guard.NotNullOrWhiteSpace(referencedTable);
        Guard.NotNullOrWhiteSpace(referencedTableSchema);

        var tableName = table.Name;
        var fKeyName = string.IsNullOrWhiteSpace(foreignKeyName)
            ? $"FK_{table.Schema}_{tableName}_{columnName}"
            : foreignKeyName;

        if (table.HasForeignKey(fKeyName))
        {
            throw new InvalidOperationException("A foreign key with the same name exists in the table.");
        }

        var foreignKey = new ForeignKey(table, foreignKeyName);
        var foreignKeyColumn = new ForeignKeyColumn(foreignKey, columnName, referencedColumn);
        foreignKey.Columns.Add(foreignKeyColumn);
        foreignKey.ReferencedTable = referencedTable;
        foreignKey.ReferencedTableSchema = referencedTableSchema;

        table.ForeignKeys.Add(foreignKey);

        if (!string.IsNullOrWhiteSpace(foreignKeyDescription))
        {
            foreignKey.ExtendedProperties.Add(new ExtendedProperty(foreignKey, MSDescriptionExtendedPropertyName,
                foreignKeyDescription));
        }

        table.Alter();

        return foreignKey;
    }

    /// <summary>
    ///     Activates system versioning for the <paramref name="table" />. This method creates a history table and adds a valid
    ///     from and valid to column to the <paramref name="table" />.
    /// </summary>
    /// <param name="table">The table to activate system versioning on.</param>
    /// <param name="historySchema">
    ///     The schema of the history table. This parameter is required and cannot be <c>null</c> or
    ///     whitespace.
    /// </param>
    /// <param name="historyTableName">
    ///     The name of the history table. If <c>null</c> then the name will be automatically
    ///     generated
    /// </param>
    /// <returns>The updated <paramref name="table" /></returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when a foreign key with the same name exists in the
    ///     <paramref name="table" />
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="table" />, <paramref name="historySchema" /> is
    ///     <c>null</c> or whitespace.
    /// </exception>
    /// <remarks>
    ///     This method adds a column named "ValidFrom" of type <c>datetime2</c>, "ValidTo" of type <c>datetime2</c>, and
    ///     activates the system versioning on the <paramref name="table" />
    ///     It also creates a history table based on the <paramref name="table" /> with the specified
    ///     <paramref name="historySchema" /> and <paramref name="historyTableName" />
    /// </remarks>
    public static Table ActivateSystemVersioning(this Table table, string historySchema = "History",
        string? historyTableName = null)
    {
        Guard.NotNull(table);
        Guard.NotNullOrWhiteSpace(historySchema);

        table.Refresh();

        var validFrom = table.AddColumn("ValidFrom", DataType.DateTime2(7), false,
            "The system recorded date on which the record was created or last updated.", "sysutcdatetime()");
        var validTo = table.AddColumn("ValidTo", DataType.DateTime2(7), false,
            "The system recorded date on which the validity of the record expires.",
            "CONVERT([datetime2],'9999-12-31 23:59:59')");

        var historyTable = table.BuildHistoryTable(historySchema, historyTableName);

        table.AddPeriodForSystemTime(validFrom.Name, validTo.Name, true);

        table.Alter();

        validFrom.IsHidden = true;
        validTo.IsHidden = true;

        table.HistoryTableSchema = historyTable.Schema;
        table.HistoryTableName = historyTable.Name;
        table.IsSystemVersioned = true;

        table.Alter();
        table.Refresh();

        return table;
    }

    private static Table BuildHistoryTable(this Table table, string historySchema = "History",
        string? historyTableName = null)
    {
        Guard.NotNull(table);
        Guard.NotNullOrWhiteSpace(historySchema);

        var hTableName = string.IsNullOrWhiteSpace(historyTableName) ? table.Name + "History" : historyTableName;

        var historyTable = table.Parent.GetTable(historySchema, hTableName);

        if (historyTable != null)
        {
            throw new InvalidOperationException($"A table with the same name '{hTableName}' exists in the database.");
        }

        historyTable = new Table(table.Parent, hTableName, historySchema);
        var tableDescription = new ExtendedProperty(historyTable, MSDescriptionExtendedPropertyName,
            $"Historical records for the '{table.Name}' table.");
        historyTable.ExtendedProperties.Add(tableDescription);

        foreach (var column in table.Columns.Cast<Column>().ToList())
        {
            var historyColumn = historyTable.AddColumn(column.Name, column.DataType, column.Nullable,
                column.Description(), persistChange: false);
            historyTable.Columns.Add(historyColumn);
        }

        var validFrom = new Column(historyTable, "ValidFrom", DataType.DateTime2(7))
        {
            Nullable = false
        };
        validFrom.AddDescription("The system recorded date on which the original record was created or updated.");
        historyTable.Columns.Add(validFrom);

        var validTo = new Column(historyTable, "ValidTo", DataType.DateTime2(7))
        {
            Nullable = false
        };
        validTo.AddDescription("The system recorded date on which the validity of the original record expired.");
        historyTable.Columns.Add(validTo);
        historyTable.Create();

        historyTable.AddNonClusteredIndex(new[]
        {
            "ValidFrom", "ValidTo", $"IX_{historySchema}_{hTableName}_ID_PERIOD_COLUMNS",
            "Performance index for historical records."
        });

        historyTable.Alter();

        return historyTable;
    }
}