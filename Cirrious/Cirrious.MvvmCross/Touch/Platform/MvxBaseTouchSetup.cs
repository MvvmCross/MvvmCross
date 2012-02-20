#region Copyright

// <copyright file="MvxBaseTouchSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion


using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Touch.Services;

namespace Cirrious.MvvmCross.Touch.Platform
{
    public abstract class MvxBaseTouchSetup
        : MvxBaseSetup
          , IMvxServiceProducer<IMvxTouchNavigator>
    {
        private readonly IMvxTouchViewPresenter _presenter;
		private readonly MvxApplicationDelegate _applicationDelegate;
		
        protected MvxBaseTouchSetup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
        {
			_presenter = presenter;
			_applicationDelegate = applicationDelegate;
        }

        protected override MvxViewsContainer CreateViewsContainer()
        {
            var container = new MvxTouchViewsContainer(_presenter);
            this.RegisterServiceInstance<IMvxTouchNavigator>(container);
            return container;
        }
		
		protected override void InitializeAdditionalPlatformServices ()
		{
			MvxTouchServiceProvider.Instance.SetupAdditionalPlatformTypes(_applicationDelegate, _presenter);
        }
    }
}