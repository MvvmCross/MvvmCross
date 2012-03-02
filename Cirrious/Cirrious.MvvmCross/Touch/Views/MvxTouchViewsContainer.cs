#region Copyright
// <copyright file="MvxTouchViewsContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using System;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Views;

#endregion

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxTouchViewsContainer
        : MvxViewsContainer
		, IMvxTouchNavigator
        , IMvxTouchViewCreator
    {
        private readonly IMvxTouchViewPresenter _presenter;
        
        public MvxTouchViewsContainer(IMvxTouchViewPresenter presenter)
        {
			_presenter = presenter;
        }

        public override IMvxViewDispatcher Dispatcher
        {
            get { return new MvxTouchViewDispatcher(); }
        }

        #region Implementation of IMvxTouchNavigator

        #region IMvxTouchNavigator Members

        public virtual void NavigateTo(MvxShowViewModelRequest request)
        {
            MvxTrace.TaggedTrace("TouchNavigation", "Navigate requested");
			
            var view = CreateView(request);

            if (request.ClearTop)
                _presenter.ClearBackStack();
            _presenter.ShowView(view);
        }

        public virtual void GoBack()
        {
            MvxTrace.TaggedTrace("TouchNavigation", "Navigate back requested");
            _presenter.GoBack();
        }

        #endregion

        #region IMvxTouchViewCreator Members

        public virtual IMvxTouchView CreateView(MvxShowViewModelRequest request)
        {
            var viewType = GetViewType(request.ViewModelType);
            if (viewType == null)
                throw new MvxException("View Type not found for " + request.ViewModelType);

            var view = Activator.CreateInstance(viewType, request) as IMvxTouchView;
            if (view == null)
                throw new MvxException("View not loaded for " + viewType);
            return view;
        }

        #endregion

        #endregion
    }
}