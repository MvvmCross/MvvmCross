// MvxBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces.BindingContext;
using MonoMac.AppKit;

namespace Cirrious.MvvmCross.Binding.Touch.BindingContext
{
    public class MvxBindingContext
        : MvxBaseBindingContext<NSView>
          , IMvxBindingContext
    {
        public Action CallOnNextDataContextChange { get; private set; }

        private IMvxBinder GetBinder()
        {
            return Mvx.Resolve<IMvxBinder>();
        }

		public MvxBindingContext () 
			: base(null)
		{
		}

        public MvxBindingContext(NSView mainView, string firstBindingText)
            : base(null)
        {
            CallOnNextDataContextChange =
                () =>
                    {
                        var bindings = GetBinder().Bind(DataContext, mainView, firstBindingText).ToList();
                        bindings.ForEach(RegisterBinding);
                    };
        }

        public MvxBindingContext(NSView mainView, IEnumerable<MvxBindingDescription> firstBindingDescriptions)
            : base(null)
        {
            CallOnNextDataContextChange =
                () =>
                    {
                        var bindings = GetBinder().Bind(DataContext, mainView, firstBindingDescriptions).ToList();
                        bindings.ForEach(RegisterBinding);
                    };
        }

        protected override void OnDataContextChange()
        {
            if (CallOnNextDataContextChange != null)
            {
                CallOnNextDataContextChange();
                return;
            }

            base.OnDataContextChange();
        }
    }
}