#region Copyright

// <copyright file="MvxBaseAndroidSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Android.Content;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Android.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Android.Platform
{
    public abstract class MvxBaseAndroidSetup
        : MvxBaseSetup
          , IMvxServiceProducer<IMvxAndroidViewModelRequestTranslator>
          , IMvxServiceProducer<IMvxAndroidSubViewServices>
          , IMvxServiceProducer<IMvxAndroidActivityTracker>
    {
        private readonly Context _applicationContext;

        protected MvxBaseAndroidSetup(Context applicationContext)
        {
            _applicationContext = applicationContext;
        }

        protected override MvxViewsContainer CreateViewsContainer()
        {
            var container = new MvxAndroidViewsContainer(_applicationContext);
            this.RegisterServiceInstance<IMvxAndroidViewModelRequestTranslator>(container);
            this.RegisterServiceInstance<IMvxAndroidSubViewServices>(container);
            this.RegisterServiceInstance<IMvxAndroidActivityTracker>(container);
            return container;
        }
    }
}