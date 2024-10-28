// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.Extensions;
using MvvmCross.Converters;
using MvvmCross.Exceptions;

namespace MvvmCross.Binding.Bindings.Source.Leaf
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
                MvxBindingLog.Instance?.LogError(
                    "GetValue ignored in binding - target property {PropertyTypeName}.{PropertyName} is writeonly",
                    PropertyInfo.DeclaringType?.Name, PropertyName);
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
                MvxBindingLog.Instance?.LogWarning("SetValue ignored in binding - source property {PropertyName} is missing", PropertyName);
                return;
            }

            if (!PropertyInfo.CanWrite)
            {
                MvxBindingLog.Instance?.LogWarning(
                    "SetValue ignored in binding - target property {PropertyTypeName}.{PropertyName} is readonly",
                    PropertyInfo.DeclaringType?.Name, PropertyName);
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
                MvxBindingLog.Instance?.LogError(exception, "SetValue failed with exception. Property Name: {PropertyName}, Value: {Value}", PropertyName, value);
            }
        }
    }
}
