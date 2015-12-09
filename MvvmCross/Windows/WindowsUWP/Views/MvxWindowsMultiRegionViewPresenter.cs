﻿// MvxWindowsMultiRegionViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsUWP.Views
{
    using System;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;

    public class MvxWindowsMultiRegionViewPresenter
        : MvxWindowsViewPresenter
    {
        private readonly IMvxWindowsFrame _rootFrame;

        public MvxWindowsMultiRegionViewPresenter(IMvxWindowsFrame rootFrame)
            : base(rootFrame)
        {
            this._rootFrame = rootFrame;
        }

        public override void Show(MvxViewModelRequest request)
        {
            var viewType = GetViewType(request);

            if (viewType.HasRegionAttribute())
            {
                var converter = Mvx.Resolve<IMvxNavigationSerializer>();
                var requestText = converter.Serializer.SerializeObject(request);

                var containerView = FindChild<Frame>(this._rootFrame.UnderlyingControl, viewType.GetRegionName());

                if (containerView == null)
                    throw new MvxException($"Region '{viewType.GetRegionName()}' not found in view '{viewType}'");

                containerView.Navigate(viewType, requestText);
            }
            else
            {
                base.Show(request);
            }
        }

        private static Type GetViewType(MvxViewModelRequest request)
        {
            var viewFinder = Mvx.Resolve<IMvxViewsContainer>();
            return viewFinder.GetViewType(request.ViewModelType);
        }

        // Implementation from: http://stackoverflow.com/a/1759923/80186
        internal static T FindChild<T>(DependencyObject reference, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid.
            if (reference == null) return null;

            var foundChild = default(T);

            var childrenCount = VisualTreeHelper.GetChildrenCount(reference);
            for (var index = 0; index < childrenCount; index++)
            {
                var child = VisualTreeHelper.GetChild(reference, index);

                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}