// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Binding.Extensions;

namespace MvvmCross.Plugin.FieldBinding
{
    [RequiresUnreferencedCode("This class uses reflection to bind to fields which may not be preserved by trimming.")]
    public class MvxLeafNotifyChangeFieldSourceBinding
        : MvxNotifyChangeFieldSourceBinding
    {
        public MvxLeafNotifyChangeFieldSourceBinding(object source, INotifyChange notifyChange)
            : base(source, notifyChange)
        {
        }

        protected override void NotifyChangeOnChanged(object? sender, EventArgs eventArgs)
        {
            FireChanged();
        }

        public override void SetValue(object value)
        {
            var fieldType = NotifyChange.ValueType;
            var safeValue = fieldType!.MakeSafeValue(value);

            // if safeValue matches the existing value, then don't call set
            if (EqualsCurrentValue(safeValue!))
                return;

            NotifyChange.Value = safeValue;
        }

        public override Type SourceType => NotifyChange.ValueType!;

        public override object GetValue()
        {
            return NotifyChange.Value!;
        }
    }
}
