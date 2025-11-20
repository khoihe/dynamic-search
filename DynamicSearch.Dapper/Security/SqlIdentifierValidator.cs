namespace DynamicSearch.Dapper.Security;

/// <summary>
/// Validates and sanitizes SQL identifiers to prevent SQL injection
/// </summary>
public static class SqlIdentifierValidator
{
    private static readonly HashSet<string> ValidSortOrders = new(StringComparer.OrdinalIgnoreCase)
    {
        "asc",
        "desc"
    };

    /// <summary>
    /// Validates that a column name contains only safe characters
    /// Allows: letters, numbers, underscores, dots (for joins)
    /// </summary>
    public static bool IsValidIdentifier(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            return false;

        // Allow alphanumeric, underscore, and dot for nested properties
        // Max length 128 to prevent abuse
        if (identifier.Length > 128)
            return false;

        foreach (char c in identifier)
        {
            if (!char.IsLetterOrDigit(c) && c != '_' && c != '.')
                return false;
        }

        // Don't allow starting with a dot or ending with a dot
        if (identifier.StartsWith('.') || identifier.EndsWith('.'))
            return false;

        // Don't allow consecutive dots
        if (identifier.Contains(".."))
            return false;

        return true;
    }

    /// <summary>
    /// Validates sort order (asc/desc only)
    /// </summary>
    public static bool IsValidSortOrder(string sortOrder)
    {
        return ValidSortOrders.Contains(sortOrder);
    }

    /// <summary>
    /// Safely quotes a SQL identifier (column/table name)
    /// Validates before quoting to prevent injection
    /// </summary>
    public static string QuoteIdentifier(string identifier)
    {
        if (!IsValidIdentifier(identifier))
        {
            throw new ArgumentException(
                $"Invalid SQL identifier: '{identifier}'. Only alphanumeric characters, underscores, and dots are allowed.",
                nameof(identifier));
        }

        // Use PostgreSQL double-quote escaping
        // Double quotes protect against SQL keywords and allow case-sensitive names
        return $"\"{identifier.Replace("\"", "\"\"")}\"";
    }

    /// <summary>
    /// Validates and returns safe sort order
    /// </summary>
    public static string ValidateSortOrder(string sortOrder)
    {
        if (!IsValidSortOrder(sortOrder))
        {
            throw new ArgumentException(
                $"Invalid sort order: '{sortOrder}'. Only 'asc' or 'desc' are allowed.",
                nameof(sortOrder));
        }

        return sortOrder.ToUpperInvariant();
    }
}
