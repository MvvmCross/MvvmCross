#region Copyright
// <copyright file="MvxPropertyInfoSourceBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Threading;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
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
                MvxBindingTrace.Trace(MvxTraceLevel.Error,"SetValue ignored in binding - target property is writeonly");
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
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,"SetValue ignored in binding - target property missing");
                return;
            }

            if (!PropertyInfo.CanWrite)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "SetValue ignored in binding - target property is readonly");
                return;
            }

            try
            {
                var safeValue = PropertyInfo.PropertyType.MakeSafeValue(value);
                PropertyInfo.SetValue(Source, safeValue, null);
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "SetValue failed with exception - " + exception.ToLongString());
            }
        }
    }
}