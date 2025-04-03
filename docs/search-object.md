# Search Object
| Property  | Description                                |
| --------- | ------------------------------------------ |
| pageIndex | Index of the page requested, starts from 0 |
| pageSize  | Size of the page requested, default is 20  |
| sorts     | Columns need to be sorted                  |
| fields    | Fields need to be taken                    |
| filter    | The dynamic filter object                  |

## Filter Object
### queryKey (table column)
- The key of an object in the response when calling a search API without the filter object

### queryValue (table column's value)
- The value of the key need to be searched

### queryType (table column's type)
| Value             | Description                 |
| ----------------- | --------------------------- |
| text              | Text data type              |
| number            | Number data type            |
| boolean           | Boolean data type           |
| guid              | Guid data type              |
| nullable_guid     | Nullable Guid data type     |
| date              | Date data type              |
| nullable_date     | Nullable Date data type     |
| datetime          | Datetime data type          |
| nullable_datetime | Nullable datetime data type |

### operation
| Value     | Description            |
| --------- | ---------------------- |
| eq        | Equals                 |
| neq       | Not equals             |
| in        | In                     |
| nin       | Not in                 |
| lt        | Less than              |
| lte       | Less than or equals    |
| gt        | Greater than           |
| gte       | Greater than or equals |
| contains  | Contains               |
| ncontains | Not contains           |
| ago       | Ago                    |
| between   | Between                |
| nbetween  | Not beetween           |
| sw        | Starts with            |
| nsw       | Not starts with        |
| ew        | Ends with              |
| new       | Not ends with          |

## Sample ([more sample](../DynamicSearch.Test/Integrations/DeviceTests.cs))
### Response
- Assuming the response from a search without the filter object
```json
{
    "pageIndex": 0,
    "pageSize": 100,
    "totalPage": 1,
    "totalCount": 5,
    "durationInMilisecond": 11,
    "data": [
        {
            "id": "device5",
            "name": "Device 5",
            "createdUtc": "0001-01-01T00:00:00",
            "updatedUtc": "0001-01-01T00:00:00",
            "type": {
                "id": "COMMAND",
                "name": "Command"
            }
        },
        {
            "id": "device4",
            "name": "Device 4",
            "createdUtc": "0001-01-01T00:00:00",
            "updatedUtc": "0001-01-01T00:00:00",
            "type": {
                "id": "ALIAS",
                "name": "Alias"
            }
        },
        {
            "id": "device3",
            "name": "Device 3",
            "createdUtc": "0001-01-01T00:00:00",
            "updatedUtc": "0001-01-01T00:00:00",
            "type": {
                "id": "RUNTIME",
                "name": "Runtime"
            }
        },
        {
            "id": "device2",
            "name": "Device 2",
            "createdUtc": "0001-01-01T00:00:00",
            "updatedUtc": "0001-01-01T00:00:00",
            "type": {
                "id": "DYNAMIC",
                "name": "Dynamic"
            }
        },
        {
            "id": "device1",
            "name": "Device 1",
            "createdUtc": "0001-01-01T00:00:00",
            "updatedUtc": "0001-01-01T00:00:00",
            "type": {
                "id": "STATIC",
                "name": "Static"
            }
        }
    ]
}
```

### Request
- Filter only items have name "Device 1"
```json
{
    "pageIndex": 0,
    "pageSize": 100,
    "filter": {
        "queryKey": "name",
        "queryType": "text",
        "operation": "eq",
        "queryValue": "Device 1"
    }
}
```

- Filter only items have name ends with "1" and type ends with "c" (relationship one-one)
```json
{
    "pageIndex": 0,
    "pageSize": 100,
    "filter": {
        "and": [
            {
                "queryKey": "name",
                "queryType": "text",
                "operation": "ew",
                "queryValue": "1"
            },
            {
                "queryKey": "type.name",
                "queryType": "text",
                "operation": "ew",
                "queryValue": "c"
            }
        ]
    }
}
```

- Filter only items that are visible & one of its template "Device Template" (relationship one-many)
```json
{
    "pageIndex": 0,
    "pageSize": 100,
    "filter": {
        "and": [
            {
                "queryKey": "isVisible",
                "queryType": "boolean",
                "operation": "eq",
                "queryValue": true
            },
            {
                "queryKey": "Templates.Any(e => e.Name.ToString() == \"Device Template\")",
                "queryType": "boolean",
                "operation": "eq",
                "queryValue": true
            }
        ]
    }
}
```