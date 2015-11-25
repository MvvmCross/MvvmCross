// MvxLeafNotifyChangeFieldSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.ExtensionMethods;
using System;

namespace MvvmCross.Plugins.FieldBinding
{
    public class MvxLeafNotifyChangeFieldSourceBinding
        : MvxNotifyChangeFieldSourceBinding
    {
        public MvxLeafNotifyChangeFieldSourceBinding(object source, INotifyChange notifyChange)
            : base(source, notifyChange)
        {
        }

        protected override void NotifyChangeOnChanged(object sender, EventArgs eventArgs)
        {
            base.FireChanged();
        }

        public override void SetValue(object value)
        {
            var fieldType = NotifyChange.ValueType;
            var safeValue = fieldType.MakeSafeValue(value);

            // if safeValue matches the existing value, then don't call set
            if (EqualsCurrentValue(safeValue))
                return;

            NotifyChange.Value = safeValue;
        }

        public override Type SourceType
        {
            get { return NotifyChange.ValueType; }
        }

        public override object GetValue()
        {
            return NotifyChange.Value;
        }
    }
}