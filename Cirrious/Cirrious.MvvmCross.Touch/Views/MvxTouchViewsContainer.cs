// MvxTouchViewsContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

#region using

#endregion

namespace Cirrious.MvvmCross.Touch.Views
{
    public class MvxTouchViewsContainer
        : MvxViewsContainer
          , IMvxTouchViewCreator
          , IMvxCurrentRequest
    {
        public MvxViewModelRequest CurrentRequest { get; private set; }

        #region IMvxTouchViewCreator Members

        public virtual IMvxTouchView CreateView(MvxViewModelRequest request)
        {
            try
            {
                CurrentRequest = request;
                var viewType = GetViewType(request.ViewModelType);
                if (viewType == null)
                    throw new MvxException("View Type not found for " + request.ViewModelType);

                var view = Activator.CreateInstance(viewType) as IMvxTouchView;
                if (view == null)
                    throw new MvxException("View not loaded for " + viewType);
                view.Request = request;
                return view;
            }
            finally
            {
                CurrentRequest = null;
            }
        }

        public virtual IMvxTouchView CreateView(IMvxViewModel viewModel)
        {
            var request = new MvxViewModelInstaceRequest(viewModel);
            var view = CreateView(request);
            return view;
        }

        #endregion
    }
}