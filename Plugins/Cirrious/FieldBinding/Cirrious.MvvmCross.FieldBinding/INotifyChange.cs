// INotifyChange.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.FieldBinding
{
    public interface INotifyChange
    {
        event EventHandler Changed;
        object Value { get; set; }
        Type ValueType { get; }
    }

    public interface INotifyChange<T> : INotifyChange
    {
        new T Value { get; set; }
    }

    public interface INotifyChange<T, TValue>
        : INotifyChange<T>
        where T : IList<TValue>
    {
        TValue this[int key] { get; set; }
    }

    public interface INotifyChange<T, TKey, TValue>
        : INotifyChange<T>
        where T : IDictionary<TKey,TValue>
    {
        TValue this[TKey key] { get; set; }
    }
}