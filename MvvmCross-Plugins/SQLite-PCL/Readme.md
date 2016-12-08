## SQLite-PCL

Using the SQLite Plugin for MvvmCross is quite simple. The plugin injects the IMvxSqliteConnectionFactory into the IoC container. First you have to inject the factory in your class through the contructor or through property injection.

```c#
private readonly IMvxSqliteConnectionFactory _sqliteConnectionFactory;

public Repository(IMvxSqliteConnectionFactory sqliteConnectionFactory)
{
    _sqliteConnectionFactory = sqliteConnectionFactory;
}
```

### API

The API of IMvxSqliteConnectionFactory is very easy to understand and to use.

```c#
public interface IMvxSqliteConnectionFactory
{
    SQLiteConnection GetConnection(string databaseName, bool prefixPlatformPath = true);
    SQLiteConnection GetConnection(SqLiteConfig config, bool prefixPlatformPath = true);
    SQLiteAsyncConnection GetAsyncConnection(string databaseName, bool prefixPlatformPath = true);
    SQLiteAsyncConnection GetAsyncConnection(SqLiteConfig config, bool prefixPlatformPath = true);
}
```
#### SQLite Connection

Using the name of the database only:
```c#
var databaseName = "myTestDatabase.sqlite";
var connection = _sqliteConnectionFactory.GetConnection(databaseName);
```

Using the SqliteConfig Object:
```c#
var config = new SqLiteConfig("myTestDatabase.sqlite", true);
                              
var connection = _sqliteConnectionFactory.GetConnection(config);
```

#### Async SQLite Connection

* Using the name of the database only:
```c#
var databaseName = "myTestDatabase.sqlite";
var connection = _sqliteConnectionFactory.GetAsyncConnection(databaseName);
```

Using the SqliteConfig Object:
```c#
var config = new SqLiteConfig("myTestDatabase.sqlite", true);            
var connection = _sqliteConnectionFactory.GetAsyncConnection(config);
```
