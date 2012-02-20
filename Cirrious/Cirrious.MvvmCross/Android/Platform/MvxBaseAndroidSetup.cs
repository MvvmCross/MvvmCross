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

using System.Reflection;
using Android.Content;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Android.Services;
using Cirrious.MvvmCross.Android.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Android.Platform
{
    public abstract class MvxBaseAndroidSetup
        : MvxBaseSetup
        , IMvxAndroidGlobals
        , IMvxServiceProducer<IMvxAndroidViewModelRequestTranslator>
        , IMvxServiceProducer<IMvxAndroidContextSource>
        , IMvxServiceProducer<IMvxAndroidGlobals>
    {
        private readonly Context _applicationContext;

        protected MvxBaseAndroidSetup(Context applicationContext)
        {
            _applicationContext = applicationContext;            
        }

        protected override void InitializeAdditionalPlatformServices()
        {
            MvxAndroidServiceProvider.Instance.RegisterPlatformContextTypes(_applicationContext);
            this.RegisterServiceInstance<IMvxAndroidGlobals>(this);
        }

        protected override MvxViewsContainer CreateViewsContainer()
        {
            var container = new MvxAndroidViewsContainer(_applicationContext);
            this.RegisterServiceInstance<IMvxAndroidViewModelRequestTranslator>(container);
            return container;
        }

        public abstract string ExecutableNamespace { get; }
        public abstract Assembly ExecutableAssembly { get; }
        public Context ApplicationContext { get { return _applicationContext; } }
    }
}