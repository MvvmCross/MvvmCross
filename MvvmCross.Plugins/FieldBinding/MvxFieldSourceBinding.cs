// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Binding.Bindings.Source;

namespace MvvmCross.Plugin.FieldBinding
{
    public abstract class MvxFieldSourceBinding
        : MvxSourceBinding
    {
        private readonly FieldInfo _fieldInfo;

        protected FieldInfo FieldInfo => _fieldInfo;

        protected MvxFieldSourceBinding(object source, FieldInfo fieldInfo)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (fieldInfo == null)
                throw new ArgumentNullException(nameof(fieldInfo));
            _fieldInfo = fieldInfo;
        }
    }
}
