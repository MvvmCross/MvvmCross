// MvxPropertyInfoSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Bindings.Source
{
    public class MvxPropertyInfoSourceBinding : MvxBasePropertyInfoSourceBinding
    {
        public MvxPropertyInfoSourceBinding(object source, string propertyName)
            : base(source, propertyName)
        {
        }

        public override Type SourceType
        {
            get { return (PropertyInfo == null) ? null : PropertyInfo.PropertyType; }
        }

        protected override void OnBoundPropertyChanged()
        {
            FireChanged(new MvxSourcePropertyBindingEventArgs(this));
        }

        public override bool TryGetValue(out object value)
        {
            if (PropertyInfo == null)
            {
                value = null;
                return false;
            }

            if (!PropertyInfo.CanRead)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "SetValue ignored in binding - target property is writeonly");
                value = null;
                return false;
            }

            value = PropertyInfo.GetValue(Source, null);
            return true;
        }

        public override void SetValue(object value)
        {
            if (PropertyInfo == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "SetValue ignored in binding - source property {0} is missing", PropertyName);
                return;
            }

            if (!PropertyInfo.CanWrite)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "SetValue ignored in binding - target property is readonly");
                return;
            }

            try
            {
                var propertyType = PropertyInfo.PropertyType;
                var safeValue = propertyType.MakeSafeValue(value);
                PropertyInfo.SetValue(Source, safeValue, null);
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "SetValue failed with exception - " + exception.ToLongString());
            }
        }
    }
}