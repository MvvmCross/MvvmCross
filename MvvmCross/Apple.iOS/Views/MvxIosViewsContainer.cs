// MvxIosViewsContainer.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.iOS.Views
{
    using System;
    using System.Reflection;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform.Exceptions;

    using UIKit;

    public class MvxIosViewsContainer
        : MvxViewsContainer
        , IMvxIosViewsContainer
    {
        public MvxViewModelRequest CurrentRequest { get; private set; }

        public virtual IMvxIosView CreateView(MvxViewModelRequest request)
        {
            try
            {
                this.CurrentRequest = request;
                var viewType = this.GetViewType(request.ViewModelType);
                if (viewType == null)
                    throw new MvxException("View Type not found for " + request.ViewModelType);

                var view = this.CreateViewOfType(viewType, request);
                view.Request = request;
                return view;
            }
            finally
            {
                this.CurrentRequest = null;
            }
        }

        protected virtual IMvxIosView CreateViewOfType(Type viewType, MvxViewModelRequest request)
        {
#warning SHARED-APPLE: Switched back to PCL profile 7, missing member 'GetCustomAttribute'
            //var storyboardAttribute = viewType.GetCustomAttribute<MvxFromStoryboardAttribute>();

            var storyboardAttribute = viewType.GetTypeInfo().GetCustomAttribute<MvxFromStoryboardAttribute>();
            if (storyboardAttribute != null)
            {
                var storyboardName = storyboardAttribute.StoryboardName ?? viewType.Name;
                try
                {
                    var storyboard = UIStoryboard.FromName(storyboardName, null);
                    var viewController = storyboard.InstantiateViewController(viewType.Name);
                    return (IMvxIosView)viewController;
                }
                catch (Exception ex)
                {
                    throw new MvxException("Loading view of type {0} from storyboard {1} failed: {2}", viewType.Name, storyboardName, ex.Message);
                }
            }

            var view = Activator.CreateInstance(viewType) as IMvxIosView;
            if (view == null)
                throw new MvxException("View not loaded for " + viewType);
            return view;
        }

        public virtual IMvxIosView CreateView(IMvxViewModel viewModel)
        {
            var request = new MvxViewModelInstanceRequest(viewModel);
            var view = this.CreateView(request);
            return view;
        }
    }
}