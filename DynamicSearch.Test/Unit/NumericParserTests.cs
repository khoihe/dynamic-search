namespace DynamicSearch.Test.Unit;

public class NumericParserTests
{
    private readonly NumericParser _parser;

    public NumericParserTests()
    {
        _parser = new NumericParser();
    }

    [Theory]
    [InlineData("0", 0.0)]
    [InlineData("1", 1.0)]
    [InlineData("-1", -1.0)]
    [InlineData("123.45", 123.45)]
    [InlineData("-123.45", -123.45)]
    [InlineData("0.5", 0.5)]
    [InlineData("1000000", 1000000.0)]
    [InlineData("1.23456789", 1.23456789)]
    public void Parse_ValidNumericStrings_ReturnsCorrectValue(string input, double expected)
    {
        // Act
        var result = _parser.Parse(input);

        // Assert
        Xunit.Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("abc")]
    [InlineData("12.34.56")]
    [InlineData("1,000")]
    public void Parse_InvalidNumericStrings_ThrowsFormatException(string input)
    {
        // Act & Assert
        Xunit.Assert.Throws<FormatException>(() => _parser.Parse(input));
    }

    [Fact]
    public void Parse_NullString_ThrowsArgumentNullException()
    {
        // Act & Assert
        Xunit.Assert.Throws<ArgumentNullException>(() => _parser.Parse(null));
    }

    [Theory]
    [InlineData("1.7976931348623157E+308")] // Max double
    [InlineData("-1.7976931348623157E+308")] // Min double
    public void Parse_EdgeCaseValues_ParsesCorrectly(string input)
    {
        // Act
        var result = _parser.Parse(input);

        // Assert
        Xunit.Assert.True(double.IsFinite(result));
    }
}
