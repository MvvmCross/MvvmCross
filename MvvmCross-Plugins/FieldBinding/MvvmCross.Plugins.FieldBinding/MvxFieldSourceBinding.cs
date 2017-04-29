// MvxFieldSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using MvvmCross.Binding.Bindings.Source;

namespace MvvmCross.Plugins.FieldBinding
{
    public abstract class MvxFieldSourceBinding
        : MvxSourceBinding
    {
        protected MvxFieldSourceBinding(object source, FieldInfo fieldInfo)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (fieldInfo == null)
                throw new ArgumentNullException(nameof(fieldInfo));
            FieldInfo = fieldInfo;
        }

        protected FieldInfo FieldInfo { get; }
    }
}