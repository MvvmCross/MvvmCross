// NC.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.FieldBinding
{
    public class NC<T> : NotifyChange<T>, INC<T>
    {
        public NC()
        {
        }

        public NC(T value)
            : base(value)
        {
        }

        public NC(T value, Action<T> valueChanged)
            : base(value, valueChanged)
        {
        }
    }

    public class NC<T, TValue>
        : NotifyChange<T, TValue>
        , INC<T, TValue>
        where T : IList<TValue>
    {
        public NC()
        {
        }

        public NC(T value)
            : base(value)
        {
        }

        public NC(T value, Action<T> valueChanged)
            : base(value, valueChanged)
        {
        }
    }

    public class NC<T, TKey, TValue>
        : NotifyChange<T, TKey, TValue>
        , INC<T, TKey, TValue>
        where T : IDictionary<TKey, TValue>
    {
        public NC()
        {
        }

        public NC(T value)
            : base(value)
        {
        }

        public NC(T value, Action<T> valueChanged)
            : base(value, valueChanged)
        {
        }
    }
}