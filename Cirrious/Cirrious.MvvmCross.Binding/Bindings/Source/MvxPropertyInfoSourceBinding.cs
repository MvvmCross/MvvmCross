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
using Cirrious.MvvmCross.ExtensionMethods;

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
                MvxBindingTrace.Trace(MvxBindingTraceLevel.Error,"SetValue ignored in binding - target property is writeonly");
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
                MvxBindingTrace.Trace(MvxBindingTraceLevel.Warning,"SetValue ignored in binding - target property missing");
                return;
            }

            if (!PropertyInfo.CanWrite)
            {
                MvxBindingTrace.Trace(MvxBindingTraceLevel.Warning, "SetValue ignored in binding - target property is readonly");
                return;
            }

            try
            {
                PropertyInfo.SetValue(Source, value, null);
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxBindingTrace.Trace(MvxBindingTraceLevel.Error, "SetValue failed with exception - " + exception.ToLongString());
            }
        }
    }
}