// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using AppKit;
using MvvmCross.Exceptions;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Mac.Views
{
    public class MvxMacViewsContainer
        : MvxViewsContainer, IMvxMacViewsContainer
    {
        public MvxViewModelRequest CurrentRequest { get; private set; }

        public virtual IMvxMacView CreateView(MvxViewModelRequest request)
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

        public virtual IMvxMacView CreateViewOfType(Type viewType, MvxViewModelRequest request)
        {
            var storyboardAttribute = viewType.GetCustomAttribute<MvxFromStoryboardAttribute>();
            if (storyboardAttribute != null)
            {
                var storyboardName = storyboardAttribute.StoryboardName ?? viewType.Name;
                try
                {
                    var storyboard = NSStoryboard.FromName(storyboardName, null);
                    var viewController = storyboard.InstantiateControllerWithIdentifier(viewType.Name);
                    return (IMvxMacView)viewController;
                }
                catch (Exception ex)
                {
                    throw new MvxException("Loading view of type {0} from storyboard {1} failed: {2}", viewType.Name, storyboardName, ex.Message);
                }
            }

            var view = Activator.CreateInstance(viewType) as IMvxMacView;
            if (view == null)
                throw new MvxException("View not loaded for " + viewType);
            return view;
        }

        public virtual IMvxMacView CreateView(IMvxViewModel viewModel)
        {
            var request = new MvxViewModelInstanceRequest(viewModel);
            var view = CreateView(request);
            return view;
        }
    }
}
