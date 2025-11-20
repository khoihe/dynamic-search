namespace DynamicSearch.Test.Integration;

/// <summary>
/// Integration tests for Dapper dynamic search functionality
/// These tests verify SQL query generation, parameter binding, and SQL injection prevention
/// </summary>
[Collection("Composite")]
public class DapperDeviceTests : IClassFixture<CompositeFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly CompositeFixture _compositeFixture;
    private readonly IQueryService _queryService;
    private readonly string _baseQuery = "select * from devices";

    public DapperDeviceTests(ITestOutputHelper output, CompositeFixture compositeFixture)
    {
        _output = output;
        _compositeFixture = compositeFixture;

        // Setup Dapper services
        var services = new ServiceCollection();
        services.AddDapperDynamicSearch();
        var serviceProvider = services.BuildServiceProvider();
        _queryService = serviceProvider.GetRequiredService<IQueryService>();
    }

    #region Basic Query Tests

    [Fact]
    public void CompileQuery_EmptyCriteria_ReturnsBaseQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("select * from devices", query);
        Xunit.Assert.Contains("limit 10 offset 0", query);
    }

    [Fact]
    public void CompileQuery_NoPaging_OmitsLimitOffset()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria, paging: false);

        // Assert
        Xunit.Assert.DoesNotContain("limit", query);
        Xunit.Assert.DoesNotContain("offset", query);
    }

    #endregion

    #region Filter Operation Tests

    [Fact]
    public void CompileQuery_EqualsFilter_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "name" },
                { "queryType", "text" },
                { "operation", "eq" },
                { "queryValue", "Device 1" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("where", query);
        Xunit.Assert.Contains("\"name\"", query);
        Xunit.Assert.Contains("@0", query);
        var dict = (IDictionary<string, object>)value;
        Xunit.Assert.Equal("Device 1", dict["0"]);
    }

    [Fact]
    public void CompileQuery_NotEqualsFilter_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "name" },
                { "queryType", "text" },
                { "operation", "neq" },
                { "queryValue", "Device 1" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("!=", query);
        Xunit.Assert.Contains("\"name\"", query);
    }

    [Fact]
    public void CompileQuery_InFilter_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "name" },
                { "queryType", "text" },
                { "operation", "in" },
                { "queryValue", "[Device 1,Device 2]" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("in", query.ToLower());
        Xunit.Assert.Contains("\"name\"", query);
    }

    [Fact]
    public void CompileQuery_NotInFilter_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "name" },
                { "queryType", "text" },
                { "operation", "nin" },
                { "queryValue", "[Device 1,Device 2]" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("not in", query.ToLower());
        Xunit.Assert.Contains("\"name\"", query);
    }

    [Fact]
    public void CompileQuery_LessThanFilter_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "created_utc" },
                { "queryType", "datetime" },
                { "operation", "lt" },
                { "queryValue", "2024-01-03T00:00:00" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("<", query);
        Xunit.Assert.Contains("\"created_utc\"", query);
    }

    [Fact]
    public void CompileQuery_LessThanOrEqualsFilter_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "created_utc" },
                { "queryType", "datetime" },
                { "operation", "lte" },
                { "queryValue", "2024-01-03T00:00:00" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("<=", query);
    }

    [Fact]
    public void CompileQuery_GreaterThanFilter_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "created_utc" },
                { "queryType", "datetime" },
                { "operation", "gt" },
                { "queryValue", "2024-01-03T00:00:00" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains(">", query);
        Xunit.Assert.DoesNotContain("=", query.Split('>')[1].Substring(0, 1));
    }

    [Fact]
    public void CompileQuery_GreaterThanOrEqualsFilter_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "created_utc" },
                { "queryType", "datetime" },
                { "operation", "gte" },
                { "queryValue", "2024-01-03T00:00:00" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains(">=", query);
    }

    [Fact]
    public void CompileQuery_ContainsFilter_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "name" },
                { "queryType", "text" },
                { "operation", "contains" },
                { "queryValue", "test" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("like", query.ToLower());
        Xunit.Assert.Contains("\"name\"", query);
    }

    [Fact]
    public void CompileQuery_NotContainsFilter_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "name" },
                { "queryType", "text" },
                { "operation", "ncontains" },
                { "queryValue", "test" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("not like", query.ToLower());
    }

    [Fact]
    public void CompileQuery_StartsWithFilter_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "name" },
                { "queryType", "text" },
                { "operation", "sw" },
                { "queryValue", "Dev" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("like", query.ToLower());
        Xunit.Assert.Contains("\"name\"", query);
    }

    [Fact]
    public void CompileQuery_EndsWithFilter_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "name" },
                { "queryType", "text" },
                { "operation", "ew" },
                { "queryValue", "1" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("like", query.ToLower());
    }

    [Fact]
    public void CompileQuery_BetweenFilter_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "created_utc" },
                { "queryType", "datetime" },
                { "operation", "between" },
                { "queryValue", "[2024-01-01T00:00:00,2024-01-02T00:00:00]" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("between", query.ToLower());
        Xunit.Assert.Contains("and", query.ToLower());
    }

    [Fact]
    public void CompileQuery_NotBetweenFilter_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "created_utc" },
                { "queryType", "datetime" },
                { "operation", "nbetween" },
                { "queryValue", "[2024-01-01T00:00:00,2024-01-02T00:00:00]" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("not between", query.ToLower());
    }

    #endregion

    #region Pagination and Sorting Tests

    [Fact]
    public void CompileQuery_SimplePagination_GeneratesCorrectLimitOffset()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 2,
            PageSize = 20
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("limit 20 offset 40", query);
    }

    [Fact]
    public void CompileQuery_PageZero_GeneratesZeroOffset()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("offset 0", query);
    }

    [Fact]
    public void CompileQuery_SingleColumnSortAscending_GeneratesCorrectOrderBy()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Sorts = "name=asc"
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("order by", query.ToLower());
        Xunit.Assert.Contains("\"name\"", query);
        Xunit.Assert.Contains("ASC", query);
    }

    [Fact]
    public void CompileQuery_SingleColumnSortDescending_GeneratesCorrectOrderBy()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Sorts = "name=desc"
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("order by", query.ToLower());
        Xunit.Assert.Contains("DESC", query);
    }

    [Fact]
    public void CompileQuery_MultiColumnSort_GeneratesCorrectOrderBy()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Sorts = "name=asc,created_utc=desc"
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("order by", query.ToLower());
        Xunit.Assert.Contains("\"name\"", query);
        Xunit.Assert.Contains("\"created_utc\"", query);
        Xunit.Assert.Contains("ASC", query);
        Xunit.Assert.Contains("DESC", query);
    }

    [Fact]
    public void CompileQuery_InvalidSortOrder_ThrowsArgumentException()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Sorts = "name=invalid"
        };

        // Act & Assert
        var exception = Xunit.Assert.Throws<ArgumentException>(() =>
            _queryService.CompileQuery(_baseQuery, criteria));
        Xunit.Assert.Contains("Invalid sort order", exception.Message);
    }

    [Fact]
    public void CompileQuery_SqlInjectionInSort_ThrowsArgumentException()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Sorts = "name; DROP TABLE users--=asc"
        };

        // Act & Assert
        var exception = Xunit.Assert.Throws<ArgumentException>(() =>
            _queryService.CompileQuery(_baseQuery, criteria));
        Xunit.Assert.Contains("Invalid SQL identifier", exception.Message);
    }

    #endregion

    #region Complex Filter Tests

    [Fact]
    public void CompileQuery_MultipleAndFilters_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "and", new JArray
                    {
                        new JObject
                        {
                            { "queryKey", "name" },
                            { "queryType", "text" },
                            { "operation", "contains" },
                            { "queryValue", "Device" }
                        },
                        new JObject
                        {
                            { "queryKey", "created_utc" },
                            { "queryType", "datetime" },
                            { "operation", "gt" },
                            { "queryValue", "2024-01-01T00:00:00" }
                        }
                    }
                }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("and", query.ToLower());
        Xunit.Assert.Contains("\"name\"", query);
        Xunit.Assert.Contains("\"created_utc\"", query);
    }

    [Fact]
    public void CompileQuery_MultipleOrFilters_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "or", new JArray
                    {
                        new JObject
                        {
                            { "queryKey", "name" },
                            { "queryType", "text" },
                            { "operation", "eq" },
                            { "queryValue", "Device 1" }
                        },
                        new JObject
                        {
                            { "queryKey", "name" },
                            { "queryType", "text" },
                            { "operation", "eq" },
                            { "queryValue", "Device 2" }
                        }
                    }
                }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("or", query.ToLower());
    }

    [Fact]
    public void CompileQuery_InvalidLogicalOperator_ThrowsArgumentException()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "xor", new JArray
                    {
                        new JObject
                        {
                            { "queryKey", "name" },
                            { "queryType", "text" },
                            { "operation", "eq" },
                            { "queryValue", "Device 1" }
                        }
                    }
                }
            }
        };

        // Act & Assert
        var exception = Xunit.Assert.Throws<ArgumentException>(() =>
            _queryService.CompileQuery(_baseQuery, criteria));
        Xunit.Assert.Contains("Invalid logical operator", exception.Message);
    }

    #endregion

    #region Security Tests

    [Fact]
    public void CompileQuery_SqlInjectionInQueryKey_ThrowsArgumentException()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "name; DROP TABLE users--" },
                { "queryType", "text" },
                { "operation", "eq" },
                { "queryValue", "test" }
            }
        };

        // Act & Assert
        var exception = Xunit.Assert.Throws<ArgumentException>(() =>
            _queryService.CompileQuery(_baseQuery, criteria));
        Xunit.Assert.Contains("Invalid SQL identifier", exception.Message);
    }

    [Fact]
    public void CompileQuery_SqlCommentInQueryKey_ThrowsArgumentException()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "name--comment" },
                { "queryType", "text" },
                { "operation", "eq" },
                { "queryValue", "test" }
            }
        };

        // Act & Assert
        var exception = Xunit.Assert.Throws<ArgumentException>(() =>
            _queryService.CompileQuery(_baseQuery, criteria));
        Xunit.Assert.Contains("Invalid SQL identifier", exception.Message);
    }

    [Fact]
    public void CompileQuery_ValidNestedProperty_GeneratesCorrectQuery()
    {
        // Arrange
        var criteria = new QueryCriteria
        {
            PageIndex = 0,
            PageSize = 10,
            Filter = new JObject
            {
                { "queryKey", "type.name" },
                { "queryType", "text" },
                { "operation", "eq" },
                { "queryValue", "test" }
            }
        };

        // Act
        var (query, value) = _queryService.CompileQuery(_baseQuery, criteria);

        // Assert
        Xunit.Assert.Contains("\"type.name\"", query);
        Xunit.Assert.DoesNotThrow(() => _queryService.CompileQuery(_baseQuery, criteria));
    }

    #endregion
}
