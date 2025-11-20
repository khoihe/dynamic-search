namespace DynamicSearch.Test.Unit;

public class EqualsOperationBuilderTests
{
    private readonly EqualsOperationBuilder _builder;

    public EqualsOperationBuilderTests()
    {
        _builder = new EqualsOperationBuilder();
    }

    [Fact]
    public void Build_StringValue_GeneratesCorrectExpression()
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
        Xunit.Assert.Contains("=", result.Query);
        Xunit.Assert.Contains("name", result.Query);
        Xunit.Assert.Contains("test", result.Values);
    }

    [Fact]
    public void Build_NumericValue_GeneratesCorrectExpression()
    {
        // Arrange
        var filter = new QueryFilter
        {
            QueryKey = "age",
            QueryValue = "25",
            QueryType = "numeric"
        };

        // Act
        var result = _builder.Build(filter);

        // Assert
        Xunit.Assert.Contains("=", result.Query);
        Xunit.Assert.Equal(25.0, result.Values[0]);
    }

    [Fact]
    public void Build_BooleanValue_GeneratesCorrectExpression()
    {
        // Arrange
        var filter = new QueryFilter
        {
            QueryKey = "is_active",
            QueryValue = "true",
            QueryType = "boolean"
        };

        // Act
        var result = _builder.Build(filter);

        // Assert
        Xunit.Assert.True((bool)result.Values[0]);
    }

    [Fact]
    public void Build_NullValue_HandlesGracefully()
    {
        // Arrange
        var filter = new QueryFilter
        {
            QueryKey = "description",
            QueryValue = null,
            QueryType = "text"
        };

        // Act
        var result = _builder.Build(filter);

        // Assert
        Xunit.Assert.NotNull(result);
    }
}
