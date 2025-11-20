namespace DynamicSearch.Test.Unit;

public class ContainsOperationBuilderTests
{
    private readonly ContainsOperationBuilder _builder;

    public ContainsOperationBuilderTests()
    {
        _builder = new ContainsOperationBuilder();
    }

    [Fact]
    public void Build_StringValue_GeneratesLikeExpression()
    {
        // Arrange
        var filter = new QueryFilter
        {
            QueryKey = "name",
            QueryValue = "test",
            QueryType = "text"
        };

        // Act
        var result = _builder.Build(filter);

        // Assert
        Xunit.Assert.Contains("like", result.Query.ToLower());
        Xunit.Assert.Contains("name", result.Query);
    }

    [Fact]
    public void Build_StringValue_AddsWildcards()
    {
        // Arrange
        var filter = new QueryFilter
        {
            QueryKey = "name",
            QueryValue = "test",
            QueryType = "text"
        };

        // Act
        var result = _builder.Build(filter);

        // Assert
        var value = result.Values[0].ToString();
        Xunit.Assert.StartsWith("%", value);
        Xunit.Assert.EndsWith("%", value);
        Xunit.Assert.Contains("test", value);
    }

    [Fact]
    public void Build_EmptyString_StillGeneratesValidExpression()
    {
        // Arrange
        var filter = new QueryFilter
        {
            QueryKey = "name",
            QueryValue = "",
            QueryType = "text"
        };

        // Act
        var result = _builder.Build(filter);

        // Assert
        Xunit.Assert.NotNull(result);
        Xunit.Assert.Contains("like", result.Query.ToLower());
    }
}
