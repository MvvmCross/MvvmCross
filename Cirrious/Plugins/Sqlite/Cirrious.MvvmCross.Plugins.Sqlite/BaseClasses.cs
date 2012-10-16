#warning TODO - add explanation of copyrights - MS-Pl vs MIT vs custom...

//
// Copyright (c) 2009-2011 Krueger Systems, Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

namespace Cirrious.MvvmCross.Plugins.Sqlite
{
    public interface ISQLiteConnectionFactory
    {
        ISQLiteConnection Create(string address);
    }

    public class PrimaryKeyAttribute : Attribute
    {
    }

    public class AutoIncrementAttribute : Attribute
    {
    }

    public class IndexedAttribute : Attribute
    {
    }

    public class IgnoreAttribute : Attribute
    {
    }

    public class MaxLengthAttribute : Attribute
    {
        public int Value { get; private set; }


        public MaxLengthAttribute(int length)
        {
            Value = length;
        }
    }

    public class CollationAttribute : Attribute
    {
        public string Value { get; private set; }

        public CollationAttribute(string collation)
        {
            Value = collation;
        }
    }

    public interface ISQLiteConnection : IDisposable
    {
        string DatabasePath { get; }

        bool TimeExecution { get; set; }

        bool Trace { get; set; }

        int CreateTable<T>();

        ISQLiteCommand CreateCommand(string cmdText, params object[] ps);

        int Execute(string query, params object[] args);

        List<T> Query<T>(string query, params object[] args) where T : new();

        IEnumerable<T> DeferredQuery<T>(string query, params object[] args) where T : new();

        List<object> Query(ITableMapping map, string query, params object[] args);

        IEnumerable<object> DeferredQuery(ITableMapping map, string query, params object[] args);

        ITableQuery<T> Table<T>() where T : new();

        T Get<T>(object pk) where T : new();

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

        int Update(object obj);

        int Update(object obj, Type objType);

        int Delete(object objectToDelete);
        int Delete<T>(object primaryKey);
        
        void Close();
    }

    public interface ITableMapping { }

    public interface ISQLiteCommand { }

    public interface ITableQuery<T> : IEnumerable<T> where T : new()
    {        
    }
}
