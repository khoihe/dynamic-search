namespace DynamicSearch.Test.Unit;

public class StringParserTests
{
    private readonly StringParser _parser;

    public StringParserTests()
    {
        _parser = new StringParser();
    }

    [Theory]
    [InlineData("hello", "hello")]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("test with spaces", "test with spaces")]
    [InlineData("123", "123")]
    [InlineData("special!@#$%", "special!@#$%")]
    public void Parse_AnyString_ReturnsExactValue(string input, string expected)
    {
        // Act
        var result = _parser.Parse(input);

        // Assert
        Xunit.Assert.Equal(expected, result);
    }

    [Fact]
    public void Parse_NullString_ReturnsNull()
    {
        // Act
        var result = _parser.Parse(null);

        // Assert
        Xunit.Assert.Null(result);
    }

    [Fact]
    public void Parse_Unicode_PreservesUnicode()
    {
        // Arrange
        var input = "Hello 世界 🌍";

        // Act
        var result = _parser.Parse(input);

        // Assert
        Xunit.Assert.Equal(input, result);
    }
}
