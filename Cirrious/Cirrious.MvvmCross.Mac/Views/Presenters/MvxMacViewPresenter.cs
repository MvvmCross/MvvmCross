#region Copyright
// <copyright file="MvxTouchViewPresenter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Views;
using MonoMac.AppKit;

namespace Cirrious.MvvmCross.Touch.Views.Presenters
{
    public class MvxMacViewPresenter 
        : MvxBaseMacViewPresenter
        , IMvxServiceConsumer<IMvxMacViewCreator>
    {
        private readonly NSApplicationDelegate _applicationDelegate;
        
        public MvxMacViewPresenter (NSApplicationDelegate applicationDelegate)
        {
            _applicationDelegate = applicationDelegate;
        } 

        public override void Show(MvxShowViewModelRequest request)
        {
            var view = CreateView(request);

            if (request.ClearTop)
                ClearBackStack();

            Show(view);
        }

        private IMvxMacView CreateView(MvxShowViewModelRequest request)
        {
            return this.GetService<IMvxMacViewCreator>().CreateView(request);
        }

        public virtual void Show (IMvxMacView view)
        {			
			var viewController = view as NSWindowController;
            if (viewController == null)
                throw new MvxException("Passed in IMvxTouchView is not a UIViewController");
        
			viewController.Window.MakeKeyAndOrderFront(_applicationDelegate);
        }
        
        public override void ClearBackStack()
        {
			// ? TODO
        }
    }	
}
