// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.FieldBinding
{
    public interface INC<T> : INotifyChange<T>
    {
    }

    public interface INCString : INC<string>
    {
    }

    public interface INCList<TValue> : INotifyChangeList<TValue>
    {
    }

    public interface INCDictionary<TKey, TValue> : INotifyChangeDictionary<TKey, TValue>
    {
    }
}