// NC.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace MvvmCross.Plugins.FieldBinding
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

    public class NCList<TValue>
        : NotifyChangeList<TValue>
        , INCList<TValue>
    {
        public NCList()
        {
        }

        public NCList(IList<TValue> value)
            : base(value)
        {
        }

        public NCList(IList<TValue> value, Action<IList<TValue>> valueChanged)
            : base(value, valueChanged)
        {
        }
    }

    public class NCDictionary<TKey, TValue>
        : NotifyChangeDictionary<TKey, TValue>
        , INCDictionary<TKey, TValue>
    {
        public NCDictionary()
        {
        }

        public NCDictionary(IDictionary<TKey, TValue> value)
            : base(value)
        {
        }

        public NCDictionary(IDictionary<TKey, TValue> value, Action<IDictionary<TKey, TValue>> valueChanged)
            : base(value, valueChanged)
        {
        }
    }
}