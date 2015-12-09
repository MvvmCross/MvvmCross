// MvxSimpleAndroidSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cirrious.MvvmCross.Droid.Simple
{
    public abstract class MvxSimpleAndroidSetup : MvxAndroidSetup
    {
        private readonly IEnumerable<Type> _converterTypes;

        protected MvxSimpleAndroidSetup(Context applicationContext, params Type[] converterTypes)
            : base(applicationContext)
        {
            _converterTypes = converterTypes;
        }

        protected override IEnumerable<Type> ValueConverterHolders => _converterTypes.ToList();

        protected override void InitializeViewLookup()
        {
            // do nothing
        }

        protected override IMvxApplication CreateApp()
        {
            return new MvxSimpleEmptyAndroidApp();
        }

        protected override void InitializeNavigationSerializer()
        {
            // do nothing in simple apps - nothing to initialize
        }

        protected override IMvxNavigationSerializer
            CreateNavigationSerializer()
        {
            throw new NotImplementedException("Not used in Simple apps - no navigation needed");
        }
    }
}