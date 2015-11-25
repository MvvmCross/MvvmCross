// INotifyChange.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace MvvmCross.Plugins.FieldBinding
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

    public interface INotifyChangeList<TValue>
        : INotifyChange<IList<TValue>>
    {
        TValue this[int key] { get; set; }
    }

    public interface INotifyChangeDictionary<TKey, TValue>
        : INotifyChange<IDictionary<TKey, TValue>>
    {
        TValue this[TKey key] { get; set; }
    }
}