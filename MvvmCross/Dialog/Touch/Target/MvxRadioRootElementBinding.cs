// MvxRadioRootElementBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Dialog.Touch.Target
{
    using System;
    using System.Reflection;

    using CrossUI.Touch.Dialog.Elements;

    using MvvmCross.Binding;
    using MvvmCross.Binding.Bindings.Target;
    using MvvmCross.Platform.Platform;

    public class MvxRadioRootElementBinding : MvxPropertyInfoTargetBinding<RootElement>
    {
        public MvxRadioRootElementBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var rootElement = this.View;
            if (rootElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - rootElement is null in MvxRadioRootElementBinding");
            }
            else
            {
                rootElement.RadioSelectedChanged += this.RootElementOnRadioSelectedChanged;
            }
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        private void RootElementOnRadioSelectedChanged(object sender, EventArgs eventArgs)
        {
            this.FireValueChanged(this.View.RadioSelected);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var rootElement = this.View;
                if (rootElement != null)
                {
                    rootElement.RadioSelectedChanged -= this.RootElementOnRadioSelectedChanged;
                }
            }
        }
    }
}