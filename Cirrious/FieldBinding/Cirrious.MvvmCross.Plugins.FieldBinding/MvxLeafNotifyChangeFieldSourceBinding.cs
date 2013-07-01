// MvxLeafNotifyChangeFieldSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding.Bindings.Source;
using Cirrious.MvvmCross.FieldBinding;

namespace Cirrious.MvvmCross.Plugins.FieldBinding
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
            base.FireChanged(new MvxSourcePropertyBindingEventArgs(true, NotifyChange.Value));
        }

        public override void SetValue(object value)
        {
            NotifyChange.Value = value;
        }

        public override Type SourceType
        {
            get { return NotifyChange.ValueType; }
        }

        public override bool TryGetValue(out object value)
        {
            value = NotifyChange.Value;
            return true;
        }
    }
}