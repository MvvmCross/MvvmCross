﻿// MvxEntryElementValueBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using CrossUI.Touch.Dialog.Elements;

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
            get { return MvxBindingMode.TwoWay; }
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