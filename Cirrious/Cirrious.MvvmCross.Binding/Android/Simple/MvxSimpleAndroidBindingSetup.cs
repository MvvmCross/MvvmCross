#region Copyright
// <copyright file="MvxSimpleAndroidBindingSetup.cs" company="Cirrious">
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
using Android.Content;
using Cirrious.MvvmCross.Application;

namespace Cirrious.MvvmCross.Binding.Android.Simple
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
            get
            {
                return _converterTypes;
            }
        }

        protected override MvxApplication CreateApp()
        {
            return new MvxSimpleEmptyAndroidApp();
        }
    }
}