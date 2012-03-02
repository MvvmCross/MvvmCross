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

namespace Cirrious.MvvmCross.Touch.Platform
{
    public abstract class MvxBaseTouchSetup
        : MvxBaseSetup
        , IMvxServiceProducer<IMvxTouchNavigator>
        , IMvxServiceProducer<IMvxTouchViewCreator>
    {
        private readonly IMvxTouchViewPresenter _presenter;
		private readonly MvxApplicationDelegate _applicationDelegate;
		
        protected MvxBaseTouchSetup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
        {
			_presenter = presenter;
			_applicationDelegate = applicationDelegate;
        }

        protected sealed override MvxViewsContainer CreateViewsContainer()
        {
            var container = CreateViewsContainer(_presenter);
            this.RegisterServiceInstance<IMvxTouchNavigator>(container);
            this.RegisterServiceInstance<IMvxTouchViewCreator>(container);            
            return container;
        }

        protected virtual MvxTouchViewsContainer CreateViewsContainer(IMvxTouchViewPresenter presenter)
        {
            var container = new MvxTouchViewsContainer(presenter);
            return container;
        }
		
		protected override void InitializeAdditionalPlatformServices ()
		{
			MvxTouchServiceProvider.Instance.SetupAdditionalPlatformTypes(_applicationDelegate, _presenter);
        }
    }
}