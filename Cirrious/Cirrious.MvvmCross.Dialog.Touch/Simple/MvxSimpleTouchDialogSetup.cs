// MvxSimpleTouchDialogSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using System.Linq;

namespace Cirrious.MvvmCross.Dialog.Touch.Simple
{
    public class MvxSimpleTouchDialogSetup
        : MvxTouchDialogSetup
    {
        private readonly List<Type> _converterTypes;

        public MvxSimpleTouchDialogSetup(params Type[] converterTypes)
            : base(null, (IMvxTouchViewPresenter)null)
        {
            _converterTypes = converterTypes.ToList();
        }

        protected override List<Type> ValueConverterHolders
        {
            get { return _converterTypes; }
        }

        protected override IMvxApplication CreateApp()
        {
            var app = new MvxEmptyApp();
            return app;
        }

        protected override void InitializeViewLookup()
        {
            // do nothing
        }

        public static void Initialise(params Type[] converterTypes)
        {
            var setup = new MvxSimpleTouchDialogSetup(converterTypes);
            setup.Initialize();
        }
    }
}