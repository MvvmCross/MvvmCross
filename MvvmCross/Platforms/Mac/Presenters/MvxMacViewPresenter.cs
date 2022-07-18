// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;
using Foundation;
using Microsoft.Extensions.Logging;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Mac.Presenters.Attributes;
using MvvmCross.Platforms.Mac.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Mac.Presenters
{
    public class MvxMacViewPresenter
        : MvxAttributeViewPresenter, IMvxMacViewPresenter, IMvxAttributeViewPresenter
    {
        private readonly INSApplicationDelegate _applicationDelegate;

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            MvxLogHost.Default?.Log(LogLevel.Trace, "PresentationAttribute not found for {ViewTypeName}. Assuming new window presentation", viewType.Name);
            return new MvxWindowPresentationAttribute();
        }

        public override MvxBasePresentationAttribute GetOverridePresentationAttribute(MvxViewModelRequest request, Type viewType)
        {
            if (viewType?.GetInterface(nameof(IMvxOverridePresentationAttribute)) != null)
            {
                var viewInstance = this.CreateViewControllerFor(viewType, null) as NSViewController;
                using (viewInstance)
                {
                    var presentationAttribute = (viewInstance as IMvxOverridePresentationAttribute)?.PresentationAttribute(request);

                    if (presentationAttribute == null)
                    {
                        MvxLogHost.Default?.Log(LogLevel.Warning, "Override PresentationAttribute null. Falling back to existing attribute.");
                    }
                    else
                    {
                        if (presentationAttribute.ViewType == null)
                            presentationAttribute.ViewType = viewType;

                        if (presentationAttribute.ViewModelType == null)
                            presentationAttribute.ViewModelType = request.ViewModelType;

                        return presentationAttribute;
                    }
                }
            }

            return null;
        }

        protected virtual INSApplicationDelegate ApplicationDelegate => _applicationDelegate;

        protected virtual List<NSWindow> Windows { get; } = new List<NSWindow>();

        protected virtual NSWindow MainWindow => NSApplication.SharedApplication.MainWindow;

        public MvxMacViewPresenter(INSApplicationDelegate applicationDelegate)
        {
            _applicationDelegate = applicationDelegate;
            NSWindow.Notifications.ObserveWillClose(OnWindowWillCloseNotification);
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Register<MvxWindowPresentationAttribute>(
                    (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowWindowViewController(viewController, (MvxWindowPresentationAttribute)attribute, request);
                    },
                    (viewModel, attribute) => Close(viewModel));

            AttributeTypesToActionsDictionary.Register<MvxContentPresentationAttribute>(
                    (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowContentViewController(viewController, (MvxContentPresentationAttribute)attribute, request);
                    },
                    (viewModel, attribute) => Close(viewModel));

            AttributeTypesToActionsDictionary.Register<MvxModalPresentationAttribute>(
                    (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowModalViewController(viewController, (MvxModalPresentationAttribute)attribute, request);
                    },
                    (viewModel, attribute) => Close(viewModel));

            AttributeTypesToActionsDictionary.Register<MvxSheetPresentationAttribute>(
                    (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowSheetViewController(viewController, (MvxSheetPresentationAttribute)attribute, request);
                    },
                    (viewModel, attribute) => Close(viewModel));

            AttributeTypesToActionsDictionary.Register<MvxTabPresentationAttribute>(
                    (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowTabViewController(viewController, (MvxTabPresentationAttribute)attribute, request);
                    },
                    (viewModel, attribute) => Close(viewModel));
        }

        protected virtual Task<bool> ShowWindowViewController(
            NSViewController viewController,
            MvxWindowPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            NSWindow window = null;
            MvxWindowController windowController = null;

            if (!string.IsNullOrEmpty(attribute.WindowControllerName))
            {
                windowController = CreateWindowController(attribute);
                window = windowController.Window;
            }

            if (window == null)
            {
                window = CreateWindow(attribute);

                if (windowController == null)
                {
                    windowController = CreateWindowController(window);
                    windowController.ShouldCascadeWindows = attribute.ShouldCascadeWindows;
                }
                windowController.Window = window;
            }
            else
            {
                UpdateWindow(attribute, window);
            }

            if (!Windows.Contains(window))
                Windows.Add(window);

            window.Identifier = attribute.Identifier ?? viewController.GetType().Name;

            if (!string.IsNullOrEmpty(viewController.Title))
                window.Title = viewController.Title;

            window.ContentView = viewController.View;
            window.ContentViewController = viewController;
            windowController.ShowWindow(null);
            return Task.FromResult(true);
        }

        protected virtual void UpdateWindow(MvxWindowPresentationAttribute attribute, NSWindow window)
        {
            var positionX = (float)window.Frame.X;
            var positionY = (float)window.Frame.Y;
            var width = (float)window.Frame.Width;
            var height = (float)window.Frame.Height;

            var newFrame = new CGRect(positionX, positionY, width, height);
            window.SetFrame(newFrame, false);

            window.StyleMask = attribute.WindowStyle;
            window.BackingType = attribute.BufferingType;
            window.TitleVisibility = attribute.TitleVisibility;
        }

        protected virtual NSWindow CreateWindow(MvxWindowPresentationAttribute attribute)
        {
            NSWindow window;
            var positionX = attribute.PositionX;
            var positionY = attribute.PositionY;
            var width = attribute.Width;
            var height = attribute.Height;

            window = new NSWindow(
                new CGRect(positionX, positionY, width, height),
                attribute.WindowStyle,
                attribute.BufferingType,
                false,
                NSScreen.MainScreen)
            {
                TitleVisibility = attribute.TitleVisibility,
            };
            return window;
        }

        protected virtual MvxWindowController CreateWindowController(MvxWindowPresentationAttribute attribute)
        {
            MvxWindowController windowController;
            if (!string.IsNullOrEmpty(attribute.StoryboardName))
            {
                // Instantiate from storyboard
                var storyboard = NSStoryboard.FromName(attribute.StoryboardName, null);
                windowController = (MvxWindowController)storyboard.InstantiateControllerWithIdentifier(attribute.WindowControllerName);
            }
            else
            {
                // Instantiate using Reflection - failure is possible if blank constructor is missing
                windowController = (MvxWindowController)Activator.CreateInstance(Type.GetType(attribute.WindowControllerName));
            }
            windowController.ShouldCascadeWindows = attribute.ShouldCascadeWindows;
            return windowController;
        }

        protected virtual MvxWindowController CreateWindowController(NSWindow window)
        {
            return new MvxWindowController(window);
        }

        protected virtual Task<bool> ShowContentViewController(
            NSViewController viewController,
            MvxContentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

            if (!string.IsNullOrEmpty(viewController.Title))
                window.Title = viewController.Title;

            window.ContentView = viewController.View;
            window.ContentViewController = viewController;
            return Task.FromResult(true);
        }

        protected virtual Task<bool> ShowModalViewController(
            NSViewController viewController,
            MvxModalPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

            window.ContentViewController.PresentViewControllerAsModalWindow(viewController);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> ShowSheetViewController(
            NSViewController viewController,
            MvxSheetPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

            window.ContentViewController.PresentViewControllerAsSheet(viewController);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> ShowTabViewController(
            NSViewController viewController,
            MvxTabPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

            var tabViewController = window.ContentViewController as IMvxTabViewController;
            if (tabViewController == null)
                throw new MvxException($"Trying to display a tab but there is no TabViewController to host it! View type: {viewController.GetType()}");

            tabViewController.ShowTabView(viewController, attribute.TabTitle);
            return Task.FromResult(true);
        }

        protected virtual NSWindow FindPresentingWindow(string identifier, NSViewController viewController)
        {
            NSWindow window = null;

            if (!string.IsNullOrEmpty(identifier))
                window = Windows.FirstOrDefault(w => w.Identifier == identifier);

            if (window == null)
                window = MainWindow ?? Windows.LastOrDefault();

            if (window == null)
                throw new MvxException($"Could not find a window with identifier '{identifier}' to display view '{viewController.GetType()}'");

            return window;
        }

        public override Task<bool> Close(IMvxViewModel viewModel)
        {
            for (int i = Windows.Count - 1; i >= 0; i--)
            {
                var window = Windows[i];

                // closing controller is a tab
                var tabViewController = window.ContentViewController as IMvxTabViewController;
                if (tabViewController != null && tabViewController.CloseTabView(viewModel))
                {
                    return Task.FromResult(true);
                }

                var controller = window.ContentViewController as MvxViewController;

                // if closing controller is a sheet or modal, it must have a presenting parent
                var presentedController = controller.PresentedViewControllers?.FirstOrDefault(c => ((MvxViewController)c).ViewModel == viewModel);
                if (presentedController != null)
                {
                    controller.DismissViewController(presentedController);
                    return Task.FromResult(true);
                }

                // closing controller is content in a regular window
                if (controller != null && controller.ViewModel == viewModel)
                {
                    Windows.Remove(window);
                    window.Close();
                    return Task.FromResult(true);
                }
            }

            throw new MvxException($"Could not find and close a view for '{viewModel.GetType()}'");
        }

        protected void OnWindowWillCloseNotification(object sender, NSNotificationEventArgs e)
        {
            var window = e.Notification.Object as NSWindow;
            if (Windows.Contains(window))
                Windows.Remove(window);
        }
    }
}
