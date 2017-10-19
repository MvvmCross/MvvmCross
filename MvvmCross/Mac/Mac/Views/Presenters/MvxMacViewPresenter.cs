// MvxMacViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AppKit;
using CoreGraphics;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Mac.Views.Presenters.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Mac.Views.Presenters
{
    public class MvxMacViewPresenter
        : MvxViewPresenter, IMvxMacViewPresenter, IMvxAttributeViewPresenter
    {
        private readonly INSApplicationDelegate _applicationDelegate;
        private List<NSWindow> _windows;
        private ConditionalWeakTable<NSWindow, NSWindowController> _windowsToWindowControllers = new ConditionalWeakTable<NSWindow, NSWindowController>();

        private IMvxViewModelTypeFinder _viewModelTypeFinder;
        public IMvxViewModelTypeFinder ViewModelTypeFinder
        {
            get
            {
                if (_viewModelTypeFinder == null)
                    _viewModelTypeFinder = Mvx.Resolve<IMvxViewModelTypeFinder>();
                return _viewModelTypeFinder;
            }
            set
            {
                _viewModelTypeFinder = value;
            }
        }

        private IMvxViewsContainer _viewsContainer;
        public IMvxViewsContainer ViewsContainer
        {
            get
            {
                if (_viewsContainer == null)
                    _viewsContainer = Mvx.Resolve<IMvxViewsContainer>();
                return _viewsContainer;
            }
            set
            {
                _viewsContainer = value;
            }
        }

        private Dictionary<Type, MvxPresentationAttributeAction> _attributeTypesActionsDictionary;
        public Dictionary<Type, MvxPresentationAttributeAction> AttributeTypesToActionsDictionary
        {
            get
            {
                if (_attributeTypesActionsDictionary == null)
                {
                    _attributeTypesActionsDictionary = new Dictionary<Type, MvxPresentationAttributeAction>();
                    RegisterAttributeTypes();
                }
                return _attributeTypesActionsDictionary;
            }
        }

        public virtual MvxBasePresentationAttribute GetPresentationAttribute(Type viewModelType)
        {
            var viewType = ViewsContainer.GetViewType(viewModelType);

            var overrideAttribute = GetOverridePresentationAttribute(viewModelType, viewType);
            if (overrideAttribute != null)
                return overrideAttribute;

            var attribute = viewType
                .GetCustomAttributes(typeof(MvxBasePresentationAttribute), true)
                .FirstOrDefault() as MvxBasePresentationAttribute;
            if (attribute != null)
            {
                if (attribute.ViewType == null)
                    attribute.ViewType = viewType;

                if (attribute.ViewModelType == null)
                    attribute.ViewModelType = viewModelType;

                return attribute;
            }

            return CreatePresentationAttribute(viewModelType, viewType);
        }

        public virtual MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            MvxTrace.Trace($"PresentationAttribute not found for {viewType.Name}. Assuming new window presentation");
            return new MvxWindowPresentationAttribute();
        }

        public virtual MvxBasePresentationAttribute GetOverridePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (viewType?.GetInterface(nameof(IMvxOverridePresentationAttribute)) != null)
            {
                var viewInstance = this.CreateViewControllerFor(viewType, null) as NSViewController;
                using (viewInstance)
                {
                    var presentationAttribute = (viewInstance as IMvxOverridePresentationAttribute)?.PresentationAttribute();

                    if (presentationAttribute == null)
                    {
                        MvxTrace.Warning("Override PresentationAttribute null. Falling back to existing attribute.");
                    }
                    else
                    {
                        if (presentationAttribute.ViewType == null)
                            presentationAttribute.ViewType = viewType;

                        if (presentationAttribute.ViewModelType == null)
                            presentationAttribute.ViewModelType = viewModelType;

                        return presentationAttribute;
                    }
                }
            }

            return null;
        }

        protected virtual INSApplicationDelegate ApplicationDelegate => _applicationDelegate;

        protected virtual List<NSWindow> Windows => _windows;

        public MvxMacViewPresenter(NSApplicationDelegate applicationDelegate)
        {
            _applicationDelegate = applicationDelegate;
            _windows = new List<NSWindow>();
        }

        public virtual void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Add(
                typeof(MvxWindowPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        ShowWindowViewController(viewController, (MvxWindowPresentationAttribute)attribute, request);
                    },
                CloseAction = (viewModel, attribute) => {
                    Close(viewModel);
                    return true;
                    }
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxContentPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        ShowContentViewController(viewController, (MvxContentPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => {
                        Close(viewModel);
                        return true;
                    }
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxModalPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        ShowModalViewController(viewController, (MvxModalPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => {
                        Close(viewModel);
                        return true;
                    }
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxSheetPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        ShowSheetViewController(viewController, (MvxSheetPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => {
                        Close(viewModel);
                        return true;
                    }
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxTabPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (NSViewController)this.CreateViewControllerFor(request);
                        ShowTabViewController(viewController, (MvxTabPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => {
                        Close(viewModel);
                        return true;
                    }
                });
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint presentationHint)
            {
                Close(presentationHint.ViewModelToClose);
                return;
            }

            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }

        public override void Show(MvxViewModelRequest request)
        {
            var attribute = GetPresentationAttribute(request.ViewModelType);
            attribute.ViewModelType = request.ViewModelType;
            var attributeType = attribute.GetType();

            if (AttributeTypesToActionsDictionary.TryGetValue(
                attributeType,
                out MvxPresentationAttributeAction attributeAction))
            {
                if (attributeAction.ShowAction == null)
                    throw new NullReferenceException($"attributeAction.ShowAction is null for attribute: {attributeType.Name}");

                attributeAction.ShowAction.Invoke(attribute.ViewType, attribute, request);
                return;
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }

        protected virtual void ShowWindowViewController(
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

            Windows.Add(window);
            window.ContentView = viewController.View;
            window.ContentViewController = viewController;
            windowController.ShowWindow(null);

            _windowsToWindowControllers.Add(window, windowController);
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

        protected virtual void ShowContentViewController(
            NSViewController viewController,
            MvxContentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var window = Windows.FirstOrDefault(w => w.Identifier == attribute.WindowIdentifier) ?? Windows.Last();

            if (!string.IsNullOrEmpty(viewController.Title))
                window.Title = viewController.Title;

            window.ContentView = viewController.View;
            window.ContentViewController = viewController;
        }

        protected virtual void ShowModalViewController(
            NSViewController viewController,
            MvxModalPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var window = Windows.FirstOrDefault(w => w.Identifier == attribute.WindowIdentifier) ?? Windows.Last();

            window.ContentViewController.PresentViewControllerAsModalWindow(viewController);
        }

        protected virtual void ShowSheetViewController(
            NSViewController viewController,
            MvxSheetPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var window = Windows.FirstOrDefault(w => w.Identifier == attribute.WindowIdentifier) ?? Windows.Last();

            window.ContentViewController.PresentViewControllerAsSheet(viewController);
        }

        protected virtual void ShowTabViewController(
            NSViewController viewController,
            MvxTabPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var window = Windows.FirstOrDefault(w => w.Identifier == attribute.WindowIdentifier) ?? Windows.Last();

            var tabViewController = window.ContentViewController as IMvxTabViewController;
            if (tabViewController == null)
                throw new MvxException($"trying to display a tab but there is no TabViewController! View type: {viewController.GetType()}");

            tabViewController.ShowTabView(viewController, attribute.TabTitle);
        }

        public override void Close(IMvxViewModel viewModel)
        {
            for (int i = Windows.Count - 1; i >= 0; i--)
            {
                var window = Windows[i];

                // if toClose is a sheet or modal
                if (window.ContentViewController.PresentedViewControllers.Any())
                {
                    var modal = window.ContentViewController.PresentedViewControllers
                                      .Select(v => v as MvxViewController)
                                      .FirstOrDefault(v => v.ViewModel == viewModel);

                    if (modal != null)
                    {
                        window.ContentViewController.DismissViewController(modal);
                        return;
                    }
                }
                // if toClose is a tab
                var tabViewController = window.ContentViewController as IMvxTabViewController;
                if (tabViewController != null && tabViewController.CloseTabView(viewModel))
                {
                    return;
                }

                // toClose is a content
                var controller = window.ContentViewController as MvxViewController;
                if (controller != null && controller.ViewModel == viewModel)
                {
                    window.Close();
                    Windows.Remove(window);
                    return;
                }
            }
        }

        protected virtual MvxWindowController CreateWindowController(NSWindow window)
        {
            return new MvxWindowController(window);
        }
    }
}