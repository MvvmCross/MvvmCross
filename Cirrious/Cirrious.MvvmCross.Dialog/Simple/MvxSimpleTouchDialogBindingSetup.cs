#region Copyright
// <copyright file="MvxSimpleTouchDialogBindingSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

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
            get
            {
                return _converterTypes;
            }
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