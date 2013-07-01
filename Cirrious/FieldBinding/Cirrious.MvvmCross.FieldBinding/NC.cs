// NC.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

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
}