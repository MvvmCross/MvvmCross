// <copyright file="MvxTouchViewPresenter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
using Cirrious.MvvmCross.Interfaces.Views;
using System;


using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Mac.Interfaces;
using Cirrious.MvvmCross.Views;
using MonoMac.AppKit;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Mac.Views.Presenters
{
    public class MvxMacViewPresenter 
        : MvxBaseViewPresenter
    {
        private readonly NSApplicationDelegate _applicationDelegate;
		private readonly NSWindow _window;

		protected NSWindow Window{
			get{
				return _window;
			}
		}
        
        public MvxMacViewPresenter (NSApplicationDelegate applicationDelegate, NSWindow window)
        {
            _applicationDelegate = applicationDelegate;
			_window = window;
        } 

		protected virtual void PlaceView(MvxShowViewModelRequest request, NSViewController viewController)
		{
			Window.ContentView.AddSubview(viewController.View);
		}

		protected virtual IMvxMacView GetView(MvxShowViewModelRequest request)
		{
			var creator = Mvx.Resolve<IMvxMacViewCreator>();
			return creator.CreateView(request);
		}

        public override void Show(MvxShowViewModelRequest request)
        {
			try
			{
				var view = GetView(request);

				var viewController = view as NSViewController;
				if (viewController == null)
					throw new MvxException("Passed in IMvxTouchView is not a NSViewController");

				PlaceView(request, viewController);
			}
			catch (Exception exception)
			{
				MvxTrace.Trace("Error seen during navigation request to {0} - error {1}", request.ViewModelType.Name,
				               exception.ToLongString());
			}
        }
    }	
}
