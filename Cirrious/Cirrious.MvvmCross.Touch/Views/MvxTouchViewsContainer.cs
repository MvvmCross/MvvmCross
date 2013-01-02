// MvxTouchViewsContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using System;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Views;

#endregion

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxTouchViewsContainer
        : MvxViewsContainer
          , IMvxTouchViewCreator
    {
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

        public virtual IMvxTouchView CreateView(IMvxViewModel viewModel)
        {
            var viewModelType = viewModel.GetType();
            var request = MvxShowViewModelRequest.GetDefaultRequest(viewModelType);
            var view = CreateView(request);
            var viewModelProperty = view.GetType().GetProperty("ViewModel");
            if (viewModelProperty == null)
                throw new MvxException("ViewModel Property missing for " + view.GetType());

            if (!viewModelProperty.CanWrite)
                throw new MvxException("ViewModel Property readonly for " + view.GetType());

            viewModelProperty.SetValue(view, viewModel, null);
            return view;
        }

        #endregion
    }
}