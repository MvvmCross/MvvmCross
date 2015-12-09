// MvxLeafPropertyInfoSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Source.Leaf
{
    using System;
    using System.Reflection;

    using MvvmCross.Binding.ExtensionMethods;
    using MvvmCross.Platform.Converters;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    public abstract class MvxLeafPropertyInfoSourceBinding : MvxPropertyInfoSourceBinding
    {
        protected MvxLeafPropertyInfoSourceBinding(object source, PropertyInfo propertyInfo)
            : base(source, propertyInfo)
        {
        }

        public override Type SourceType => this.PropertyInfo?.PropertyType;

        protected override void OnBoundPropertyChanged()
        {
            this.FireChanged();
        }

        public override object GetValue()
        {
            if (this.PropertyInfo == null)
            {
                return MvxBindingConstant.UnsetValue;
            }

            if (!this.PropertyInfo.CanRead)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "GetValue ignored in binding - target property is writeonly");
                return MvxBindingConstant.UnsetValue;
            }

            try
            {
                return this.PropertyInfo.GetValue(this.Source, this.PropertyIndexParameters());
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
            if (this.PropertyInfo == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "SetValue ignored in binding - source property {0} is missing", this.PropertyName);
                return;
            }

            if (!this.PropertyInfo.CanWrite)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "SetValue ignored in binding - target property is readonly");
                return;
            }

            try
            {
                var propertyType = this.PropertyInfo.PropertyType;
                var safeValue = propertyType.MakeSafeValue(value);

                // if safeValue matches the existing value, then don't call set
                if (this.EqualsCurrentValue(safeValue))
                    return;

                this.PropertyInfo.SetValue(this.Source, safeValue, this.PropertyIndexParameters());
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "SetValue failed with exception - " + exception.ToLongString());
            }
        }
    }
}