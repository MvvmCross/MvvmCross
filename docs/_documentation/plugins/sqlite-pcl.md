---
layout: documentation
title: SQLite-PCL
category: Plugins
---
Using the SQLite-Plugin for MvvmCross is quite simple. The plugin injects the IMvxSqliteConnectionFactory into the IoC container. First you have to inject the factory in your class through the contructor or through property injection.

```cs
private readonly IMvxSqliteConnectionFactory _sqliteConnectionFactory;

public Repository(IMvxSqliteConnectionFactory sqliteConnectionFactory)
{
    _sqliteConnectionFactory = sqliteConnectionFactory;
}
```

### API

The API of IMvxSqliteConnectionFactory is very easy to understand and to use.

```cs
public interface IMvxSqliteConnectionFactory
{
    SQLiteConnectionWithLock GetConnectionWithLock(string databaseName);
    SQLiteAsyncConnection GetAsyncConnection(string databaseName);
    SQLiteConnection GetConnection(string databaseName);
    SQLiteConnectionWithLock GetConnectionWithLock(SqLiteConfig config);
    SQLiteAsyncConnection GetAsyncConnection(SqLiteConfig config);
    SQLiteConnection GetConnection(SqLiteConfig config);
}
```

#### SQLite Connection

Using the name of the database only:

```cs
var databaseName = "myTestDatabase.sqlite";
var connection = _sqliteConnectionFactory.GetConnection(databaseName);
```

Using the SqliteConfig Object:

```cs
var config = new SqLiteConfig("myTestDatabase.sqlite", 
                              true, 
                              new CustomBlobSerializer(), 
                              new CustomContractResolver());
                              
var connection = _sqliteConnectionFactory.GetConnection(config);
```

#### SQLite Connection with lock

Using the name of the database only:

```cs
var databaseName = "myTestDatabase.sqlite";
var connection = _sqliteConnectionFactory.GetConnectionWithLock(databaseName);
```

Using the SqliteConfig Object:

```cs
var config = new SqLiteConfig("myTestDatabase.sqlite", 
                              true, 
                              new CustomBlobSerializer(), 
                              new CustomContractResolver());
                              
var connection = _sqliteConnectionFactory.GetConnectionWithLock(config);
```

#### Async SQLite Connection

* Using the name of the database only:

```cs
var databaseName = "myTestDatabase.sqlite";
var connection = _sqliteConnectionFactory.GetAsyncConnection(databaseName);
```

Using the SqliteConfig Object:

```cs
var config = new SqLiteConfig("myTestDatabase.sqlite", 
                              true, 
                              new CustomBlobSerializer(), 
                              new CustomContractResolver());
                              
var connection = _sqliteConnectionFactory.GetAsyncConnection(config);
```
