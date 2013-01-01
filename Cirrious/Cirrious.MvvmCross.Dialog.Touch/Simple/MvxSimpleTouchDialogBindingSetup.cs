// MvxSimpleTouchDialogBindingSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Application;

namespace Cirrious.MvvmCross.Dialog.Touch.Simple
{
    public class MvxSimpleTouchDialogBindingSetup
        : MvxTouchDialogBindingSetup
    {
        private readonly IEnumerable<Type> _converterTypes;

        public MvxSimpleTouchDialogBindingSetup(params Type[] converterTypes)
            : base(null, null)
        {
            _converterTypes = converterTypes;
        }

        #region Overrides of MvxBaseSetup

        protected override IEnumerable<Type> ValueConverterHolders
        {
            get { return _converterTypes; }
        }

        protected override MvxApplication CreateApp()
        {
            var app = new MvxEmptyApp();
            return app;
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return new Dictionary<Type, Type>();
        }

        #endregion

        public static void Initialise(params Type[] converterTypes)
        {
            var setup = new MvxSimpleTouchDialogBindingSetup(converterTypes);
            setup.Initialize();
        }
    }
}