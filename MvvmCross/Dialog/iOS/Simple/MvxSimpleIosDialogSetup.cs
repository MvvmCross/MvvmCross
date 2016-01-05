// MvxSimpleIosDialogSetup.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Dialog.iOS.Simple
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.iOS.Views.Presenters;

    public class MvxSimpleIosDialogSetup
        : MvxIosDialogSetup
    {
        private readonly List<Type> _converterTypes;

        public MvxSimpleIosDialogSetup(params Type[] converterTypes)
            : base(null, (IMvxIosViewPresenter)null)
        {
            this._converterTypes = converterTypes.ToList();
        }

        protected override List<Type> ValueConverterHolders => this._converterTypes;

        protected override IMvxApplication CreateApp()
        {
            var app = new MvxEmptyApp();
            return app;
        }

        protected override void InitializeViewLookup()
        {
            // do nothing
        }

        public static void Initialize(params Type[] converterTypes)
        {
            var setup = new MvxSimpleIosDialogSetup(converterTypes);
            ((MvxSetup)setup).Initialize();
        }
    }
}