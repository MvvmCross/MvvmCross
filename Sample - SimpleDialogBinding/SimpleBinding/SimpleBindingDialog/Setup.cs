using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Touch.Platform;

namespace SimpleBindingDialog
{
    public class NullApp
        : MvxApplication
    {
    }
    public class Setup
        : MvxTouchDialogBindingSetup
    {
        public Setup()
            : base(null, null)
        {
        }

        #region Overrides of MvxBaseSetup

        protected override MvxApplication CreateApp()
        {
            var app = new NullApp();
            return app;
        }

        protected override IEnumerable<Type> ValueConverterHolders
        {
            get { return new[] { typeof(Converters) }; }
        }

        #endregion
    }
}
