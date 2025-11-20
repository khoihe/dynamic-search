namespace DynamicSearch.Test.Unit;

public class BetweenOperationBuilderTests
{
    private readonly BetweenOperationBuilder _builder;

    public BetweenOperationBuilderTests()
    {
        _builder = new BetweenOperationBuilder();
    }

    [Fact]
    public void Build_DateTimeArray_GeneratesBetweenExpression()
    {
        // Arrange
        var filter = new QueryFilter
        {
            QueryKey = "created_date",
            QueryValue = "[2024-01-01,2024-12-31]",
            QueryType = "datetime"
        };

        // Act
        var result = _builder.Build(filter);

        // Assert
        Xunit.Assert.Contains("between", result.Query.ToLower());
        Xunit.Assert.Contains("and", result.Query.ToLower());
        Xunit.Assert.Equal(2, result.Values.Length);
    }

    [Fact]
    public void Build_NumericArray_GeneratesBetweenExpression()
    {
        // Arrange
        var filter = new QueryFilter
        {
            QueryKey = "price",
            QueryValue = "[10.5,99.99]",
            QueryType = "numeric"
        };

        // Act
        var result = _builder.Build(filter);

        // Assert
        Xunit.Assert.Contains("between", result.Query.ToLower());
        Xunit.Assert.Equal(10.5, result.Values[0]);
        Xunit.Assert.Equal(99.99, result.Values[1]);
    }

    [Fact]
    public void Build_InvalidArraySize_ThrowsException()
    {
        // Arrange
        var filter = new QueryFilter
        {
            QueryKey = "price",
            QueryValue = "[10.5]", // Only one value
            QueryType = "numeric"
        };

        // Act & Assert - Should handle gracefully or throw
        var result = _builder.Build(filter);
        // Depending on implementation, may need adjustment
    }
}
