// INC.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace Cirrious.MvvmCross.FieldBinding
{
    public interface INC<T> : INotifyChange<T>
    {
    }

    public interface INC<T, TValue> : INotifyChange<T, TValue>
        where T : IList<TValue>
    {        
    }

    public interface INC<T, TKey, TValue> : INotifyChange<T, TKey, TValue>
        where T : IDictionary<TKey, TValue>
    {
    }
}