// BaseClasses.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Cirrious.MvvmCross.Plugins.Sqlite
{
    [Obsolete("Please consider using Cirrious.MvvmCross.Community.Plugins.Sqlite instead")]
    public interface ISQLiteConnectionFactory
    {
        ISQLiteConnection Create(string address);
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Name { get; set; }

        public TableAttribute(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public ColumnAttribute(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AutoIncrementAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IndexedAttribute : Attribute
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public virtual bool Unique { get; set; }

        public IndexedAttribute()
        {
        }

        public IndexedAttribute(string name, int order)
        {
            Name = name;
            Order = order;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueAttribute : IndexedAttribute
    {
        public override bool Unique
        {
            get { return true; }
            set
            {
                /* throw?  */
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLengthAttribute : Attribute
    {
        public int Value { get; private set; }

        public MaxLengthAttribute(int length)
        {
            Value = length;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CollationAttribute : Attribute
    {
        public string Value { get; private set; }

        public CollationAttribute(string collation)
        {
            Value = collation;
        }
    }

    [Obsolete("Please consider using Cirrious.MvvmCross.Community.Plugins.Sqlite instead")]
    public interface ISQLiteConnection : IDisposable
    {
        string DatabasePath { get; }

        bool TimeExecution { get; set; }

        bool Trace { get; set; }

        int CreateTable<T>();

        int DropTable<T>();

        ITableMapping GetMapping(Type type);

        ISQLiteCommand CreateCommand(string cmdText, params object[] ps);

        int Execute(string query, params object[] args);

        T ExecuteScalar<T>(string query, params object[] args);

        List<T> Query<T>(string query, params object[] args) where T : new();

        IEnumerable<T> DeferredQuery<T>(string query, params object[] args) where T : new();

        List<object> Query(ITableMapping map, string query, params object[] args);

        IEnumerable<object> DeferredQuery(ITableMapping map, string query, params object[] args);

        ITableQuery<T> Table<T>() where T : new();

        T Get<T>(object pk) where T : new();

        T Find<T>(object pk) where T : new();

        object Find(object pk, ITableMapping map);

        bool IsInTransaction { get; }

        void BeginTransaction();

        void Rollback();

        void Commit();

        void RunInTransaction(Action action);

        int InsertAll(System.Collections.IEnumerable objects, bool beginTransaction = true);

        int Insert(object obj);

        int Insert(object obj, Type objType);

        int Insert(object obj, string extra);

        int Insert(object obj, string extra, Type objType);

        int InsertOrReplace(object obj);

        int InsertOrReplace(object obj, Type objType);

        int Update(object obj);

        int Update(object obj, Type objType);

        int UpdateAll(System.Collections.IEnumerable objects, bool beginTransaction = true);

        int Delete(object objectToDelete);

        int Delete<T>(object primaryKey);

        int DeleteAll<T>();

        void Close();
    }

    public interface ITableMapping
    {
        string TableName { get; }
    }

    public interface ISQLiteCommand
    {
    }

    public interface ITableQuery<T> : IEnumerable<T> where T : new()
    {
        ITableQuery<T> Where(Expression<Func<T, bool>> predExpr);
        ITableQuery<T> Take(int n);
        ITableQuery<T> Skip(int n);
        T ElementAt(int index);
        ITableQuery<T> OrderBy<U>(Expression<Func<T, U>> orderExpr);
        ITableQuery<T> OrderByDescending<U>(Expression<Func<T, U>> orderExpr);

        ITableQuery<TResult> Join<TInner, TKey, TResult>(
            ITableQuery<TInner> inner,
            Expression<Func<T, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<T, TInner, TResult>> resultSelector)
            where TInner : new()
            where TResult : new();

        ITableQuery<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
            where TResult : new();

        int Count();
        T First();
        T FirstOrDefault();
    }
}