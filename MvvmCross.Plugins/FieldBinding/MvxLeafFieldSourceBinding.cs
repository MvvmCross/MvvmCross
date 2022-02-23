// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Binding.Extensions;

namespace MvvmCross.Plugin.FieldBinding
{
    [Preserve(AllMembers = true)]
    public class MvxLeafFieldSourceBinding
        : MvxFieldSourceBinding
    {
        public MvxLeafFieldSourceBinding(object source, FieldInfo fieldInfo)
            : base(source, fieldInfo)
        {
        }

        public override void SetValue(object value)
        {
            var fieldType = FieldInfo.FieldType;
            var safeValue = fieldType.MakeSafeValue(value);

            // if safeValue matches the existing value, then don't call set
            if (EqualsCurrentValue(safeValue))
                return;

            FieldInfo.SetValue(Source, safeValue);
        }

        public override Type SourceType => FieldInfo.FieldType;

        public override object GetValue()
        {
            return FieldInfo.GetValue(Source);
        }
    }
}
