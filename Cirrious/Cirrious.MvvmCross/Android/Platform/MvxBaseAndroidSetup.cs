#region Copyright
// <copyright file="MvxBaseAndroidSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        #region IMvxAndroidGlobals Members

        public virtual string ExecutableNamespace { get { return GetType().Namespace; } }
        public virtual Assembly ExecutableAssembly { get { return GetType().Assembly; } }
        public Context ApplicationContext { get { return _applicationContext; } }

        #endregion

        protected override void InitializeAdditionalPlatformServices()
        {
            MvxAndroidServiceProvider.Instance.RegisterPlatformContextTypes(_applicationContext);
            this.RegisterServiceInstance<IMvxAndroidGlobals>(this);
        }

        protected override MvxViewsContainer CreateViewsContainer()
        {
            var container = CreateViewsContainer(_applicationContext);
            this.RegisterServiceInstance<IMvxAndroidViewModelRequestTranslator>(container);
            return container;
        }

        protected virtual MvxAndroidViewsContainer CreateViewsContainer(Context applicationContext)
        {
            return new MvxAndroidViewsContainer(applicationContext);
        }

        protected override IDictionary<System.Type, System.Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(ExecutableAssembly, typeof(IMvxAndroidView));
        }
    }
}