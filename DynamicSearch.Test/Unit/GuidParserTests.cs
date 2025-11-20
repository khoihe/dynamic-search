namespace DynamicSearch.Test.Unit;

public class GuidParserTests
{
    private readonly GuidParser _parser;

    public GuidParserTests()
    {
        _parser = new GuidParser();
    }

    [Fact]
    public void Parse_ValidGuid_ReturnsCorrectGuid()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var input = guid.ToString();

        // Act
        var result = _parser.Parse(input);

        // Assert
        Xunit.Assert.Equal(guid, result);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("12345678-1234-1234-1234-123456789012")]
    [InlineData("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF")]
    public void Parse_WellKnownGuids_ParsesCorrectly(string input)
    {
        // Act
        var result = _parser.Parse(input);

        // Assert
        Xunit.Assert.Equal(Guid.Parse(input), result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-a-guid")]
    [InlineData("12345678")]
    [InlineData("12345678-1234-1234-1234")]
    public void Parse_InvalidGuid_ThrowsFormatException(string input)
    {
        // Act & Assert
        Xunit.Assert.Throws<FormatException>(() => _parser.Parse(input));
    }

    [Fact]
    public void Parse_DifferentFormats_AllParse()
    {
        // Arrange
        var guid = new Guid("12345678-1234-1234-1234-123456789012");

        // Act & Assert
        Xunit.Assert.Equal(guid, _parser.Parse("12345678-1234-1234-1234-123456789012"));
        Xunit.Assert.Equal(guid, _parser.Parse("{12345678-1234-1234-1234-123456789012}"));
        Xunit.Assert.Equal(guid, _parser.Parse("(12345678-1234-1234-1234-123456789012)"));
    }
}
