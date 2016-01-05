# Sql Extensions
Lightweight library for querying data into objects

> NOTE: No stable version available for the time being.

## How to Use

### SqlConnector

The `SqlConnector` class accepts a function that generates a `DbConnection` (the connection generator).
Calling any method in `SqlConnector` will invoke the passed in DbConnection generator and close it once done.

For example,
```csharp
var connector = new SqlConnector(() => new MySqlConnection(connectionString));
```

### Standard Functions

Standard functions are extension methods or part of the `SqlConnector`.

#### Synchronous

* `.UsingConnection(actionOrFunctionDelegate)`
* `.UsingTransaction(actionOrFunctionDelegate, optionalIsolationLevel)`
* `.CommitTransaction(actionOrFunctionDelegate, optionalIsolationLevel)`
* .`UsingCommand(queryString, actionOrFunctionDelegate, optionalParameters)`
* `.QueryList(queryString, functionThatReturnsAList, optionalParameters)`
* `.QuerySingle(queryString, functionThatReturnsAnObject, optionalParameters)`
* .`NonQuery(sql, optionalParameters)`

#### Asynchronous

* `.UsingConnectionAsync(actionOrFunctionDelegate)`
* `.UsingTransactionAsync(actionOrFunctionDelegate, optionalIsolationLevel)`
* `.CommitTransactionAsync(actionOrFunctionDelegate, optionalIsolationLevel)`
* .`UsingCommandAsync(queryString, actionOrFunctionDelegate, optionalParameters)`
* `.QueryListAsync(queryString, functionThatReturnsAList, optionalParameters)`
* `.QuerySingleAsync(queryString, functionThatReturnsAnObject, optionalParameters)`
* .`NonQueryAsync(sql, optionalParameters)`

### Mapping Query Data to Objects

#### For QuerySingle

* `Mapper.StringSingle` - `Dictionary<string, string>`
* `Mapper.ObjectSingle`- `Dictionary<string, object>`
* `Mapper.DynamicSingle` - dynamic `ExpandoObject`
* `ObjectMapper<TObject>.Map`
* `ObjectMapper<TObject>.MapAsync`

#### For QueryList

* `Mapper.String`
* `Mapper.StringAsync`
* `Mapper.Object`
* `Mapper.ObjectAsync`
* `Mapper.Dynamic`
* `Mapper.DynamicAsync`
* `ObjectMapper<TObject>.MapAll`
* `ObjectMapper<TObject>.MapAllAsync`

#### Mapping to an Object

`ObjectMapper<TObject>` maps the result of query row into an object by invoking the public instance setter fields.
Mapping to an object will perform type conversion as well, within reason.
For example if a string is returned from the database, but the TObject property for that column is a DateTime, then `DateTime.Parse` will be called on the returned string.

### Passing in Parameters

Note - only IN parameters supported.

For parameters to queries, the following parameter types are supported,
* `object` - anonymous object or an object with public getters for query parameters
* `params Tuple<string, TValue>[]` - tuples as params
* `IEnumerable<Tuple<string, TValue>>` - a collection of tuples
* `IEnumerable<KeyValuePair<string, TValue>>` - a Dictionary

### Examples

```csharp
// Generate a MySQL connector
var connector = new SqlConnector(() => new MySqlConnection(connectionString.GetConnectionString(true)));

// Retrieve a list of rows
// where each row is a string key to string value map
// with a single parameter of OfficeCode = "1"
IReadOnlyList<IReadOnlyDictionary<string, string>> offices
    = connector.QueryList("SELECT * FROM `classicmodels`.`offices` WHERE `OfficeCode` = @OfficeCode", Mapper.String, new { OfficeCode = "1" });

// Query a single row
// and map the columns into an Office object
Offices office = connector.QuerySingle("SELECT * FROM classicmodels.offices LIMIT 1", ObjectMapper<Offices>.Map);
```
