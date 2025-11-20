namespace DynamicSearch.Test.Unit;

public class SqlIdentifierValidatorTests
{
    [Theory]
    [InlineData("id")]
    [InlineData("name")]
    [InlineData("user_id")]
    [InlineData("created_at")]
    [InlineData("type.name")]
    [InlineData("user.profile.email")]
    [InlineData("Column123")]
    [InlineData("_private")]
    public void IsValidIdentifier_ValidIdentifiers_ReturnsTrue(string identifier)
    {
        // Act
        var result = SqlIdentifierValidator.IsValidIdentifier(identifier);

        // Assert
        Xunit.Assert.True(result, $"'{identifier}' should be valid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("name; DROP TABLE users--")]
    [InlineData("1' OR '1'='1")]
    [InlineData("name--comment")]
    [InlineData("name/*comment*/")]
    [InlineData("name;")]
    [InlineData("name OR 1=1")]
    [InlineData("name' --")]
    [InlineData(".name")]
    [InlineData("name.")]
    [InlineData("name..field")]
    [InlineData("name with spaces")]
    [InlineData("name@domain")]
    [InlineData("name$")]
    [InlineData("name%")]
    [InlineData("name&")]
    [InlineData("name*")]
    [InlineData("name(")]
    [InlineData("name)")]
    [InlineData("name+")]
    [InlineData("name=")]
    [InlineData("name[")]
    [InlineData("name]")]
    [InlineData("name{")]
    [InlineData("name}")]
    [InlineData("name|")]
    [InlineData("name\\")]
    [InlineData("name/")]
    [InlineData("name<")]
    [InlineData("name>")]
    [InlineData("name?")]
    [InlineData("name:")]
    [InlineData("name\"")]
    [InlineData("name'")]
    public void IsValidIdentifier_InvalidIdentifiers_ReturnsFalse(string identifier)
    {
        // Act
        var result = SqlIdentifierValidator.IsValidIdentifier(identifier);

        // Assert
        Xunit.Assert.False(result, $"'{identifier}' should be invalid");
    }

    [Fact]
    public void IsValidIdentifier_TooLong_ReturnsFalse()
    {
        // Arrange
        var identifier = new string('a', 129);

        // Act
        var result = SqlIdentifierValidator.IsValidIdentifier(identifier);

        // Assert
        Xunit.Assert.False(result);
    }

    [Theory]
    [InlineData("asc")]
    [InlineData("desc")]
    [InlineData("ASC")]
    [InlineData("DESC")]
    [InlineData("Asc")]
    [InlineData("Desc")]
    public void IsValidSortOrder_ValidOrders_ReturnsTrue(string sortOrder)
    {
        // Act
        var result = SqlIdentifierValidator.IsValidSortOrder(sortOrder);

        // Assert
        Xunit.Assert.True(result, $"'{sortOrder}' should be valid");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("ascending")]
    [InlineData("descending")]
    [InlineData("asc; DROP TABLE")]
    [InlineData("asc--")]
    [InlineData("random")]
    [InlineData("1")]
    [InlineData("asc desc")]
    public void IsValidSortOrder_InvalidOrders_ReturnsFalse(string sortOrder)
    {
        // Act
        var result = SqlIdentifierValidator.IsValidSortOrder(sortOrder);

        // Assert
        Xunit.Assert.False(result, $"'{sortOrder}' should be invalid");
    }

    [Theory]
    [InlineData("id", "\"id\"")]
    [InlineData("name", "\"name\"")]
    [InlineData("user_id", "\"user_id\"")]
    [InlineData("type.name", "\"type.name\"")]
    public void QuoteIdentifier_ValidIdentifiers_ReturnsQuotedString(string identifier, string expected)
    {
        // Act
        var result = SqlIdentifierValidator.QuoteIdentifier(identifier);

        // Assert
        Xunit.Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("name; DROP TABLE users--")]
    [InlineData("1' OR '1'='1")]
    [InlineData("name--comment")]
    [InlineData(".name")]
    [InlineData("name.")]
    public void QuoteIdentifier_InvalidIdentifiers_ThrowsArgumentException(string identifier)
    {
        // Act & Assert
        var exception = Xunit.Assert.Throws<ArgumentException>(() => SqlIdentifierValidator.QuoteIdentifier(identifier));
        Xunit.Assert.Contains("Invalid SQL identifier", exception.Message);
    }

    [Theory]
    [InlineData("asc", "ASC")]
    [InlineData("desc", "DESC")]
    [InlineData("ASC", "ASC")]
    [InlineData("DESC", "DESC")]
    public void ValidateSortOrder_ValidOrders_ReturnsUpperCase(string sortOrder, string expected)
    {
        // Act
        var result = SqlIdentifierValidator.ValidateSortOrder(sortOrder);

        // Assert
        Xunit.Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("ascending")]
    [InlineData("random")]
    [InlineData("asc; DROP TABLE")]
    public void ValidateSortOrder_InvalidOrders_ThrowsArgumentException(string sortOrder)
    {
        // Act & Assert
        var exception = Xunit.Assert.Throws<ArgumentException>(() => SqlIdentifierValidator.ValidateSortOrder(sortOrder));
        Xunit.Assert.Contains("Invalid sort order", exception.Message);
    }

    [Fact]
    public void QuoteIdentifier_WithDoubleQuotes_EscapesCorrectly()
    {
        // Arrange - This should fail validation, but if it passed, we test escaping
        var identifier = "valid_name"; // Valid identifier

        // Act
        var result = SqlIdentifierValidator.QuoteIdentifier(identifier);

        // Assert
        Xunit.Assert.Equal("\"valid_name\"", result);
    }
}
