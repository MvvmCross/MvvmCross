// INC.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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