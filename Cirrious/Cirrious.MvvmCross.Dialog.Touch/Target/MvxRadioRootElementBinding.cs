#region Copyright
// <copyright file="MvxRadioRootElementBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Dialog.Touch.Target
{
    public class MvxRadioRootElementBinding : MvxPropertyInfoTargetBinding<RootElement>
    {
        public MvxRadioRootElementBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var rootElement = View;
            if (rootElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - rootElement is null in MvxRadioRootElementBinding");
            }
            else
            {
                rootElement.RadioSelectedChanged += RootElementOnRadioSelectedChanged;
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get
            {
                return MvxBindingMode.TwoWay;
            }
        }

        private void RootElementOnRadioSelectedChanged(object sender, EventArgs eventArgs)
        {
            FireValueChanged(View.RadioSelected);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var rootElement = View;
                if (rootElement != null)
                {
                    rootElement.RadioSelectedChanged -= RootElementOnRadioSelectedChanged;
                }
            }
        }
    }
}