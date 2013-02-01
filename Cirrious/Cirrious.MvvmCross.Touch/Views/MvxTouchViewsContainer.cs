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
            var request = new MvxViewModelInstanceShowViewModelRequest(viewModel);
            var view = CreateView(request);
            return view;
        }

        #endregion
    }
}