# Dynamic Search
[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://github.com/codespaces/new?hide_repo_select=true&ref=develop&repo=822938906&machine=standardLinux32gb&devcontainer_path=.devcontainer%2Fdevcontainer.json&location=EastUs)

[![NuGet](https://img.shields.io/nuget/v/DynamicSearch.EfCore)](https://www.nuget.org/packages/DynamicSearch.EfCore)

> Dynamic Search is a library that built from Linq.Dynamic.Core, main features including paging, sorting, and filtering. Supported multiple relational databases (Postgres, Microsoft SQL Server, MySQL,...)

| How?                                                                     |
| ------------------------------------------------------------------------ |
| [How to construct Search Object from front-end](./docs/search-object.md) |
| [How to setup in ASP.NET Core from back-end](./docs/setup.md)            |
| [How to run HTTP Integration Test](./docs/testing.md)                    |

## Features
### Search with paging
```json
{
    "pageIndex": 0,     // Page index starts from 0
    "pageSize": 100,    // Page size default is 20
}
```

### Search with sorting
```json
{
    "pageIndex": 0,     // Page index starts from 0
    "pageSize": 100,    // Page size default is 20
    "sorts": "id=desc,name=asc",
    "fields": ["id", "name", "type"]
}
```

### Search with single filtering
```json
{
    "pageIndex": 0,
    "pageSize": 100,
    "sorts": "id=desc,name=asc",
    "fields": ["id", "name", "type"],
    "filter": {
        "queryKey": "<query_key>",
        "queryType": "<query_type>",
        "operation": "<operation>",
        "queryValue": "<query_value>"
    }
}
```

### Search with multiple filtering (and/or)
```json
{
    "pageIndex": 0,
    "pageSize": 100,
    "sorts": "id=desc,name=asc",
    "fields": ["id", "name", "type"],
    "filter": {
        "and": [ // or
            {
                "queryKey": "<query_key>",
                "queryType": "<query_type>",
                "operation": "<operation>",
                "queryValue": "<query_value>"
            },
            {
                "queryKey": "<query_key>",
                "queryType": "<query_type>",
                "operation": "<operation>",
                "queryValue": "<query_value>"
            }
        ]
    }
}
```

### Search with relationship one-one
```json
{
    "pageIndex": 0,
    "pageSize": 100,
    "sorts": "id=desc,name=asc",
    "fields": ["id", "name", "type"],
    "filter": {
        "queryKey": "<reference_entity>.<query_key>",
        "queryType": "<query_type>",
        "operation": "<operation>",
        "queryValue": "<query_value>"
    }
}
```

### Search with relationship one-many
```json
{
    "pageIndex": 0,
    "pageSize": 100,
    "sorts": "id=desc,name=asc",
    "fields": ["id", "name", "type"],
    "filter": {
        "queryKey": "<reference_entities>.Any(e => e.<query_key>.ToString() == \"<query_value>\")",
        "queryType": "boolean",
        "operation": "eq",
        "queryValue": true
    }
}
```