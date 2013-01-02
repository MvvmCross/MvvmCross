// MvxSimpleAndroidBindingSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Android.Content;
using Cirrious.MvvmCross.Application;

namespace Cirrious.MvvmCross.Binding.Droid.Simple
{
    public abstract class MvxSimpleAndroidBindingSetup : MvxBaseAndroidBindingSetup
    {
        private readonly IEnumerable<Type> _converterTypes;

        protected MvxSimpleAndroidBindingSetup(Context applicationContext, params Type[] converterTypes)
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
    }
}