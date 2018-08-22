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

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            MvxLog.Instance.Trace($"PresentationAttribute not found for {viewType.Name}. Assuming new window presentation");
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
                        MvxLog.Instance.Warn("Override PresentationAttribute null. Falling back to existing attribute.");
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

        protected virtual List<NSWindow> Windows => NSApplication.SharedApplication.Windows.ToList();

        public MvxMacViewPresenter(INSApplicationDelegate applicationDelegate)
        {
            _applicationDelegate = applicationDelegate;
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Add(
                typeof(MvxWindowPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowWindowViewController(viewController, (MvxWindowPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => Close(viewModel)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxContentPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowContentViewController(viewController, (MvxContentPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => Close(viewModel)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxModalPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowModalViewController(viewController, (MvxModalPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => Close(viewModel)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxSheetPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowSheetViewController(viewController, (MvxSheetPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => Close(viewModel)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxTabPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        return ShowTabViewController(viewController, (MvxTabPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => Close(viewModel)
                });
        }

        public override Task<bool> Show(MvxViewModelRequest request)
        {
            return GetPresentationAttributeAction(request, out MvxBasePresentationAttribute attribute).ShowAction.Invoke(attribute.ViewType, attribute, request);
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
                    windowController.ShouldCascadeWindows = attribute.ShouldCascadeWindows ?? MvxWindowPresentationAttribute.DefaultShouldCascadeWindows;
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
            windowController.ShowWindow(null);
            return Task.FromResult(true);
        }

        protected virtual void UpdateWindow(MvxWindowPresentationAttribute attribute, NSWindow window)
        {
            var positionX = attribute.PositionX != MvxWindowPresentationAttribute.InitialPositionX ? attribute.PositionX : (float)window.Frame.X;
            var positionY = attribute.PositionY != MvxWindowPresentationAttribute.InitialPositionY ? attribute.PositionY : (float)window.Frame.Y;
            var width = attribute.Width != MvxWindowPresentationAttribute.InitialWidth ? attribute.Width : (float)window.Frame.Width;
            var height = attribute.Height != MvxWindowPresentationAttribute.InitialHeight ? attribute.Height : (float)window.Frame.Height;

            var newFrame = new CGRect(positionX, positionY, width, height);
            window.SetFrame(newFrame, false);

            window.StyleMask = attribute.WindowStyle ?? window.StyleMask;
            window.BackingType = attribute.BufferingType ?? window.BackingType;
            window.TitleVisibility = attribute.TitleVisibility ?? window.TitleVisibility;
        }

        protected virtual NSWindow CreateWindow(MvxWindowPresentationAttribute attribute)
        {
            NSWindow window;
            var positionX = attribute.PositionX != MvxWindowPresentationAttribute.InitialPositionX ? attribute.PositionX : MvxWindowPresentationAttribute.DefaultPositionX;
            var positionY = attribute.PositionY != MvxWindowPresentationAttribute.InitialPositionY ? attribute.PositionY : MvxWindowPresentationAttribute.DefaultPositionY;
            var width = attribute.Width != MvxWindowPresentationAttribute.InitialWidth ? attribute.Width : MvxWindowPresentationAttribute.DefaultWidth;
            var height = attribute.Height != MvxWindowPresentationAttribute.InitialHeight ? attribute.Height : MvxWindowPresentationAttribute.DefaultHeight;

            window = new NSWindow(
                new CGRect(positionX, positionY, width, height),
                attribute.WindowStyle ?? MvxWindowPresentationAttribute.DefaultWindowStyle,
                attribute.BufferingType ?? MvxWindowPresentationAttribute.DefaultBufferingType,
                false,
                NSScreen.MainScreen)
            {
                TitleVisibility = attribute.TitleVisibility ?? MvxWindowPresentationAttribute.DefaultTitleVisibility,
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
            windowController.ShouldCascadeWindows = attribute.ShouldCascadeWindows ?? windowController.ShouldCascadeWindows;
            return windowController;
        }

        protected virtual Task<bool> ShowContentViewController(
            NSViewController viewController,
            MvxContentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var window = Windows.FirstOrDefault(w => w.Identifier == attribute.WindowIdentifier) ?? Windows.Last();

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
            var window = Windows.FirstOrDefault(w => w.Identifier == attribute.WindowIdentifier) ?? Windows.Last();

            window.ContentViewController.PresentViewControllerAsModalWindow(viewController);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> ShowSheetViewController(
            NSViewController viewController,
            MvxSheetPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var window = Windows.FirstOrDefault(w => w.Identifier == attribute.WindowIdentifier) ?? Windows.Last();

            window.ContentViewController.PresentViewControllerAsSheet(viewController);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> ShowTabViewController(
            NSViewController viewController,
            MvxTabPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var window = Windows.FirstOrDefault(w => w.Identifier == attribute.WindowIdentifier) ?? Windows.Last();

            var tabViewController = window.ContentViewController as IMvxTabViewController;
            if (tabViewController == null)
                throw new MvxException($"trying to display a tab but there is no TabViewController! View type: {viewController.GetType()}");

            tabViewController.ShowTabView(viewController, attribute.TabTitle);
            return Task.FromResult(true);
        }

        public override Task<bool> Close(IMvxViewModel viewModel)
        {
            var currentWindows = Windows;
            for (int i = currentWindows.Count - 1; i >= 0; i--)
            {
                var window = currentWindows[i];

                // if toClose is a sheet or modal
                if (window.ContentViewController.PresentedViewControllers.Any())
                {
                    var modal = window.ContentViewController.PresentedViewControllers
                                      .Select(v => v as MvxViewController)
                                      .FirstOrDefault(v => v.ViewModel == viewModel);

                    if (modal != null)
                    {
                        window.ContentViewController.DismissViewController(modal);
                        return Task.FromResult(true);
                    }
                }
                // if toClose is a tab
                var tabViewController = window.ContentViewController as IMvxTabViewController;
                if (tabViewController != null && tabViewController.CloseTabView(viewModel))
                {
                    return Task.FromResult(true);
                }

                // toClose is a content
                var controller = window.ContentViewController as MvxViewController;
                if (controller != null && controller.ViewModel == viewModel)
                {
                    window.Close();
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(true);
        }

        protected virtual MvxWindowController CreateWindowController(NSWindow window)
        {
            return new MvxWindowController(window);
        }
    }
}
