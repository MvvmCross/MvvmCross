#region Copyright

// <copyright file="MvxTouchViewsContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

#region using

using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;
using System;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Platform.Diagnostics;

#endregion

namespace Cirrious.MvvmCross.Touch.Views
{	
    public class MvxTouchViewsContainer
        : MvxViewsContainer
		, IMvxTouchNavigator
    {
        private readonly IMvxTouchViewPresenter _presenter;
        
        public MvxTouchViewsContainer(IMvxTouchViewPresenter presenter)
        {
			_presenter = presenter;
        }

        #region IMvxViewDispatcherProvider Members

        public override IMvxViewDispatcher Dispatcher
        {
            get { return new MvxTouchViewDispatcher(); }
        }

        #endregion

		#region Implementation of IMvxTouchNavigator

        public void NavigateTo(MvxShowViewModelRequest request)
        {
			MvxTrace.TaggedTrace("TouchNavigation", "Navigate requested");
			
            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
                throw new MvxException("View Type not found for " + request.ViewModelType);

            var view = Activator.CreateInstance(viewType, request) as IMvxTouchView;
			if (view == null)
                throw new MvxException("View not loaded for " + viewType);

            if (request.ClearTop)
                _presenter.ClearBackStack();
			_presenter.ShowView(view);
        }
		
		public void GoBack()
		{
			MvxTrace.TaggedTrace("TouchNavigation", "Navigate back requested");
			_presenter.GoBack();
		}
		
        #endregion
    }
}