// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace MvvmCross.Plugin.FieldBinding
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
