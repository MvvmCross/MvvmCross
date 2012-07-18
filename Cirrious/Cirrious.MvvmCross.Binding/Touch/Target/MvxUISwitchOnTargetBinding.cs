#region Copyright
// <copyright file="MvxUISwitchOnTargetBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Reflection;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public class MvxUISwitchOnTargetBinding : MvxPropertyInfoTargetBinding<UISwitch>
    {
        public MvxUISwitchOnTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var view = View;
            if (view == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - Switch is null in MvxUISwitchOnTargetBinding");
            }
            else
            {
                view.ValueChanged += HandleValueChanged;
            }
        }

        void HandleValueChanged(object sender, System.EventArgs e)
        {
            FireValueChanged(View.On);
        }

        public override MvxBindingMode DefaultMode
        {
            get
            {
                return MvxBindingMode.TwoWay;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var view = View;
                if (view != null)
                {
                    view.ValueChanged -= HandleValueChanged;
                }
            }
        }
    }
}