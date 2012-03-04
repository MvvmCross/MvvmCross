#region Copyright
// <copyright file="MvxEntryElementValueBinding.cs" company="Cirrious">
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
using Cirrious.MvvmCross.Dialog.Touch.Dialog;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Dialog.Touch.Target
{
    public class MvxEntryElementValueBinding : MvxPropertyInfoTargetBinding<EntryElement>
    {
        public MvxEntryElementValueBinding(object target, PropertyInfo targetPropertyInfo) 
            : base(target, targetPropertyInfo)
        {
            var entryElement = View;
            if (entryElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - entryElement is null in MvxEntryElementValueBinding");
            }
            else
            {
                entryElement.Changed += EntryElementOnChanged;
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get
            {
                return MvxBindingMode.TwoWay;
            }
        }

        private void EntryElementOnChanged(object sender, EventArgs eventArgs)
        {
            FireValueChanged(View.Value);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var entryElement = View;
                if (entryElement != null)
                {
                    entryElement.Changed -= EntryElementOnChanged;
                }
            }
        }
    }
}
