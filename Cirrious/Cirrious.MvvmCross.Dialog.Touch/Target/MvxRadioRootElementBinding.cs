// MvxRadioRootElementBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using CrossUI.Touch.Dialog.Elements;

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

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

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