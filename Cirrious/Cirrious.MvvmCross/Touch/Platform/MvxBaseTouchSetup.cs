#region Copyright
// <copyright file="MvxBaseTouchSetup.cs" company="Cirrious">
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
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Images;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Platform.Images;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Platform.Images;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Platform
{
    public abstract class MvxBaseTouchSetup
        : MvxBaseSetup
        , IMvxServiceProducer<IMvxTouchNavigator>
        , IMvxServiceProducer<IMvxTouchViewCreator>
        , IMvxServiceProducer<IMvxImageCache<UIImage>>
        , IMvxServiceProducer<IMvxLocalFileImageLoader<UIImage>>
        , IMvxServiceProducer<IMvxHttpFileDownloader>
    {
        private readonly MvxApplicationDelegate _applicationDelegate;
        private readonly IMvxTouchViewPresenter _presenter;

        protected MvxBaseTouchSetup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
        {
			_presenter = presenter;
			_applicationDelegate = applicationDelegate;
        }

        protected sealed override MvxViewsContainer CreateViewsContainer()
        {
            var container = new MvxTouchViewsContainer();
            RegisterTouchViewCreator(container);            
            return container;
        }

        protected void RegisterTouchViewCreator(MvxTouchViewsContainer container)
        {
            this.RegisterServiceInstance<IMvxTouchViewCreator>(container);
        }

        protected override MvvmCross.Interfaces.Views.IMvxViewDispatcherProvider CreateViewDispatcherProvider()
        {
            return new MvxTouchViewDispatcherProvider(_presenter);
        }
	
		protected override void InitializeAdditionalPlatformServices ()
		{
			MvxTouchServiceProvider.Instance.SetupAdditionalPlatformTypes(_applicationDelegate, _presenter);
		    InitialiseUIImageProviders();
		}

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().Assembly, typeof(IMvxTouchView));
        }

        protected virtual void InitialiseUIImageProviders()
        {
            this.RegisterServiceInstance<IMvxHttpFileDownloader>(new MvxHttpFileDownloader());

#warning Huge Magic numbers here
            var fileDownloadCache = new MvxFileDownloadCache("Pictures.MvvmCross","../Library/Caches/Pictures.MvvmCross/", 500, TimeSpan.FromDays(3.0));
            var fileCache = new MvxImageCache<UIImage>(fileDownloadCache, 30, 4000000);
            this.RegisterServiceInstance<IMvxImageCache<UIImage>>(fileCache);

            this.RegisterServiceInstance<IMvxLocalFileImageLoader<UIImage>>(new MvxTouchLocalFileImageLoader());
        }
    }
}