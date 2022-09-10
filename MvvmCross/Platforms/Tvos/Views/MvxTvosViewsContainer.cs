// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Exceptions;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Views
{
    public class MvxTvosViewsContainer
        : MvxViewsContainer
        , IMvxTvosViewsContainer
    {
        public MvxViewModelRequest CurrentRequest { get; private set; }

        public virtual IMvxTvosView CreateView(MvxViewModelRequest request)
        {
            try
            {
                CurrentRequest = request;
                var viewType = GetViewType(request.ViewModelType);
                if (viewType == null)
                    throw new MvxException("View Type not found for " + request.ViewModelType);

                var view = CreateViewOfType(viewType, request);
                view.Request = request;
                return view;
            }
            finally
            {
                CurrentRequest = null;
            }
        }

        public virtual IMvxTvosView CreateViewOfType(Type viewType, MvxViewModelRequest request)
        {
            var storyboardAttribute = viewType.GetCustomAttribute<MvxFromStoryboardAttribute>();
            if (storyboardAttribute != null)
            {
                var storyboardName = storyboardAttribute.StoryboardName ?? viewType.Name;
                try
                {
                    var storyboard = UIStoryboard.FromName(storyboardName, null);
                    var viewController = storyboard.InstantiateViewController(viewType.Name);
                    return (IMvxTvosView)viewController;
                }
                catch (Exception ex)
                {
                    throw new MvxException("Loading view of type {0} from storyboard {1} failed: {2}", viewType.Name, storyboardName, ex.Message);
                }
            }

            var view = Activator.CreateInstance(viewType) as IMvxTvosView;
            if (view == null)
                throw new MvxException("View not loaded for " + viewType);
            return view;
        }

        public virtual IMvxTvosView CreateView(IMvxViewModel viewModel)
        {
            var request = new MvxViewModelInstanceRequest(viewModel);
            var view = CreateView(request);
            return view;
        }


    }
}
