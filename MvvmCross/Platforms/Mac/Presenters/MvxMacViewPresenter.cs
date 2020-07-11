// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using AppKit;
using CoreGraphics;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Mac.Presenters.Attributes;
using MvvmCross.Platforms.Mac.Views;
using MvvmCross.ViewModels;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using System.Threading.Tasks;

namespace MvvmCross.Platforms.Mac.Presenters
{
    public class MvxMacViewPresenter
        : MvxAttributeViewPresenter, IMvxMacViewPresenter, IMvxAttributeViewPresenter
    {
        private readonly INSApplicationDelegate _applicationDelegate;

        public override ValueTask<MvxBasePresentationAttribute?> CreatePresentationAttribute(Type? viewModelType, Type? viewType)
        {
            MvxLog.Instance.Trace($"PresentationAttribute not found for {viewType?.Name}. Assuming new window presentation");
            return new ValueTask<MvxBasePresentationAttribute?>(new MvxWindowPresentationAttribute());
        }

        public override ValueTask<MvxBasePresentationAttribute?> GetOverridePresentationAttribute(MvxViewModelRequest request, Type? viewType)
        {
            if (viewType?.GetInterface(nameof(IMvxOverridePresentationAttribute)) != null)
            {
                var viewInstance = this.CreateViewControllerFor(viewType, null) as NSViewController;
                using (viewInstance)
                {
                    MvxBasePresentationAttribute? presentationAttribute = (viewInstance as IMvxOverridePresentationAttribute)?.PresentationAttribute(request);

                    if (presentationAttribute == null)
                    {
                        MvxLog.Instance.Warn("Override PresentationAttribute null. Falling back to existing attribute.");
                    }
                    else
                    {
                        if (presentationAttribute.ViewType == null)
                            presentationAttribute.ViewType = viewType;

                        if (presentationAttribute.ViewModelType == null)
                            presentationAttribute.ViewModelType = request.ViewModelType;

                        return new ValueTask<MvxBasePresentationAttribute?>(presentationAttribute);
                    }
                }
            }

            return new ValueTask<MvxBasePresentationAttribute>((MvxBasePresentationAttribute)null);
        }

        protected virtual INSApplicationDelegate ApplicationDelegate => _applicationDelegate;

        protected virtual List<NSWindow> Windows => NSApplication.SharedApplication.Windows.ToList();

        public MvxMacViewPresenter(INSApplicationDelegate applicationDelegate)
        {
            _applicationDelegate = applicationDelegate;
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Register<MvxWindowPresentationAttribute>(
                    (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowWindowViewController(viewController, attribute, request);
                    },
                    (viewModel, attribute) => Close(viewModel));

            AttributeTypesToActionsDictionary.Register<MvxContentPresentationAttribute>(
                    (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowContentViewController(viewController, attribute, request);
                    },
                    (viewModel, attribute) => Close(viewModel));

            AttributeTypesToActionsDictionary.Register<MvxModalPresentationAttribute>(
                    (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowModalViewController(viewController, attribute, request);
                    },
                    (viewModel, attribute) => Close(viewModel));

            AttributeTypesToActionsDictionary.Register<MvxSheetPresentationAttribute>(
                    (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowSheetViewController(viewController, attribute, request);
                    },
                    (viewModel, attribute) => Close(viewModel));

            AttributeTypesToActionsDictionary.Register<MvxTabPresentationAttribute>(
                    (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowTabViewController(viewController, attribute, request);
                    },
                    (viewModel, attribute) => Close(viewModel));
        }

        protected virtual ValueTask<bool> ShowWindowViewController(
            NSViewController viewController,
            MvxWindowPresentationAttribute? attribute,
            MvxViewModelRequest request)
        {
            NSWindow? window = null;
            MvxWindowController? windowController = null;

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

            window.Identifier = attribute.Identifier ?? viewController.GetType().Name;

            if (!string.IsNullOrEmpty(viewController.Title))
                window.Title = viewController.Title;

            window.ContentView = viewController.View;
            window.ContentViewController = viewController;
            windowController?.ShowWindow(null);
            return new ValueTask<bool>(true);
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

        protected virtual ValueTask<bool> ShowContentViewController(
            NSViewController viewController,
            MvxContentPresentationAttribute? attribute,
            MvxViewModelRequest request)
        {
            var window = Windows.FirstOrDefault(w => w.Identifier == attribute.WindowIdentifier) ?? Windows.Last();

            if (!string.IsNullOrEmpty(viewController.Title))
                window.Title = viewController.Title;

            window.ContentView = viewController.View;
            window.ContentViewController = viewController;
            return new ValueTask<bool>(true);
        }

        protected virtual ValueTask<bool> ShowModalViewController(
            NSViewController viewController,
            MvxModalPresentationAttribute? attribute,
            MvxViewModelRequest request)
        {
            if (attribute == null) throw new NullReferenceException(nameof(attribute));

            var window = Windows?.FirstOrDefault(w => w.Identifier == attribute.WindowIdentifier) ?? Windows.Last();

            window.ContentViewController.PresentViewControllerAsModalWindow(viewController);
            return new ValueTask<bool>(true);
        }

        protected virtual ValueTask<bool> ShowSheetViewController(
            NSViewController viewController,
            MvxSheetPresentationAttribute? attribute,
            MvxViewModelRequest request)
        {
            if (attribute == null) throw new NullReferenceException(nameof(attribute));

            var window = Windows?.FirstOrDefault(w => w.Identifier == attribute.WindowIdentifier) ?? Windows.Last();

            window.ContentViewController.PresentViewControllerAsSheet(viewController);
            return new ValueTask<bool>(true);
        }

        protected virtual ValueTask<bool> ShowTabViewController(
            NSViewController viewController,
            MvxTabPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var window = Windows.FirstOrDefault(w => w.Identifier == attribute.WindowIdentifier) ?? Windows.Last();

            var tabViewController = window.ContentViewController as IMvxTabViewController;
            if (tabViewController == null)
                throw new MvxException($"trying to display a tab but there is no TabViewController! View type: {viewController.GetType()}");

            tabViewController.ShowTabView(viewController, attribute.TabTitle);
            return new ValueTask<bool>(true);
        }

        public override ValueTask<bool> Close(IMvxViewModel viewModel)
        {
            var currentWindows = Windows;
            foreach(var window in currentWindows.Reverse<NSWindow>())
            {
                // if toClose is a sheet or modal
                if (window?.ContentViewController.PresentedViewControllers.Any() ?? false)
                {
                    var modal = window.ContentViewController.PresentedViewControllers
                                      .Select(v => v as MvxViewController)
                                      .FirstOrDefault(v => v?.ViewModel == viewModel);

                    if (modal != null)
                    {
                        window.ContentViewController.DismissViewController(modal);
                        return new ValueTask<bool>(true);
                    }
                }
                // if toClose is a tab
                var tabViewController = window?.ContentViewController as IMvxTabViewController;
                if (tabViewController != null && tabViewController.CloseTabView(viewModel))
                {
                    return new ValueTask<bool>(true);
                }

                // toClose is a content
                var controller = window?.ContentViewController as MvxViewController;
                if (controller != null && controller.ViewModel == viewModel)
                {
                    window?.Close();
                    return new ValueTask<bool>(true);
                }
            }
            return new ValueTask<bool>(true);
        }

        protected virtual MvxWindowController CreateWindowController(NSWindow window)
        {
            return new MvxWindowController(window);
        }
    }
}
