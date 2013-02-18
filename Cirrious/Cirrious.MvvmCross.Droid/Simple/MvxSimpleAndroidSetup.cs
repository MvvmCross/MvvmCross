// MvxSimpleAndroidSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Android.Content;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Droid.Platform;

namespace Cirrious.MvvmCross.Binding.Droid.Simple
{
    public abstract class MvxSimpleAndroidSetup : MvxAndroidSetup
    {
        private readonly IEnumerable<Type> _converterTypes;

        protected MvxSimpleAndroidSetup(Context applicationContext, params Type[] converterTypes)
            : base(applicationContext)
        {
            _converterTypes = converterTypes;
        }

        protected override IEnumerable<Type> ValueConverterHolders
        {
            get { return _converterTypes; }
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return new Dictionary<Type, Type>();
        }

        protected override MvxApplication CreateApp()
        {
            return new MvxSimpleEmptyAndroidApp();
        }

        protected override void InitializeNavigationRequestSerializer()
        {
            // do nothing in simple apps - nothing to initialise
        }

        protected override MvvmCross.Interfaces.ViewModels.IMvxNavigationRequestSerializer CreateNavigationRequestSerializer()
        {
            throw new NotImplementedException("Not used in Simple apps - no navigation needed");
        }
    }
}
