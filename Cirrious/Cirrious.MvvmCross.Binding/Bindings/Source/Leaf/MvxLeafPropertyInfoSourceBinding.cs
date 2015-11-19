// MvxLeafPropertyInfoSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using System;
using System.Reflection;

namespace Cirrious.MvvmCross.Binding.Bindings.Source.Leaf
{
    public abstract class MvxLeafPropertyInfoSourceBinding : MvxPropertyInfoSourceBinding
    {
        protected MvxLeafPropertyInfoSourceBinding(object source, PropertyInfo propertyInfo)
            : base(source, propertyInfo)
        {
        }

        public override Type SourceType => PropertyInfo?.PropertyType;

        protected override void OnBoundPropertyChanged()
        {
            FireChanged();
        }

        public override object GetValue()
        {
            if (PropertyInfo == null)
            {
                return MvxBindingConstant.UnsetValue;
            }

            if (!PropertyInfo.CanRead)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "GetValue ignored in binding - target property is writeonly");
                return MvxBindingConstant.UnsetValue;
            }

            try
            {
                return PropertyInfo.GetValue(Source, PropertyIndexParameters());
            }
            catch (TargetInvocationException)
            {
                // for dictionary lookups we quite often expect this during binding
                // for list-based lookups we quite often expect this during binding
                return MvxBindingConstant.UnsetValue;
            }
        }

        protected abstract object[] PropertyIndexParameters();

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

                // if safeValue matches the existing value, then don't call set
                if (EqualsCurrentValue(safeValue))
                    return;

                PropertyInfo.SetValue(Source, safeValue, PropertyIndexParameters());
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "SetValue failed with exception - " + exception.ToLongString());
            }
        }
    }
}