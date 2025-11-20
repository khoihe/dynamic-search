namespace DynamicSearch.Test.Unit;

public class NumericArrayParserTests
{
    private readonly NumericArrayParser _parser;

    public NumericArrayParserTests()
    {
        _parser = new NumericArrayParser();
    }

    [Fact]
    public void Parse_SingleValue_ReturnsArrayWithOneElement()
    {
        // Arrange
        var input = "[123.45]";

        // Act
        var result = _parser.Parse(input);

        // Assert
        Xunit.Assert.Single(result);
        Xunit.Assert.Equal(123.45, result[0]);
    }

    [Fact]
    public void Parse_MultipleValues_ReturnsArrayWithAllElements()
    {
        // Arrange
        var input = "[1,2.5,3.75,4]";

        // Act
        var result = _parser.Parse(input);

        // Assert
        Xunit.Assert.Equal(4, result.Length);
        Xunit.Assert.Equal(1.0, result[0]);
        Xunit.Assert.Equal(2.5, result[1]);
        Xunit.Assert.Equal(3.75, result[2]);
        Xunit.Assert.Equal(4.0, result[3]);
    }

    [Fact]
    public void Parse_NegativeValues_ParsesCorrectly()
    {
        // Arrange
        var input = "[-1,-2.5,-3.75]";

        // Act
        var result = _parser.Parse(input);

        // Assert
        Xunit.Assert.Equal(-1.0, result[0]);
        Xunit.Assert.Equal(-2.5, result[1]);
        Xunit.Assert.Equal(-3.75, result[2]);
    }

    [Fact]
    public void Parse_EmptyArray_ThrowsFormatException()
    {
        // Arrange
        var input = "[]";

        // Act & Assert - Empty array becomes "" after trim, which cannot be parsed
        Xunit.Assert.Throws<FormatException>(() => _parser.Parse(input));
    }

    [Theory]
    [InlineData("[abc]")]
    [InlineData("[1,abc,3]")]
    [InlineData("[1.2.3]")]
    public void Parse_InvalidValues_ThrowsFormatException(string input)
    {
        // Act & Assert
        Xunit.Assert.Throws<FormatException>(() => _parser.Parse(input));
    }

    [Fact]
    public void Parse_WithoutBrackets_ParsesCorrectly()
    {
        // Arrange
        var input = "1,2,3";

        // Act
        var result = _parser.Parse(input);

        // Assert
        Xunit.Assert.Equal(3, result.Length);
    }
}
