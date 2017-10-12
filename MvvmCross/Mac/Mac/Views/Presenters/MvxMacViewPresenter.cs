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
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Mac.Views.Presenters
{
    public class MvxMacViewPresenter
        : MvxBaseMacViewPresenter
    {
        private readonly INSApplicationDelegate _applicationDelegate;
        private List<NSWindow> _windows;
        private ConditionalWeakTable<NSWindow, NSWindowController> _windowsToWindowControllers = new ConditionalWeakTable<NSWindow, NSWindowController>();

        private Dictionary<Type, Action<NSViewController, MvxBasePresentationAttribute, MvxViewModelRequest>> _attributeTypesToShowMethodDictionary;
        protected Dictionary<Type, Action<NSViewController, MvxBasePresentationAttribute, MvxViewModelRequest>> AttributeTypesToShowMethodDictionary
        {
            get
            {
                if (_attributeTypesToShowMethodDictionary == null)
                {
                    _attributeTypesToShowMethodDictionary = new Dictionary<Type, Action<NSViewController, MvxBasePresentationAttribute, MvxViewModelRequest>>();
                    RegisterAttributeTypes();
                }
                return _attributeTypesToShowMethodDictionary;
            }
        }

        protected virtual INSApplicationDelegate ApplicationDelegate => _applicationDelegate;

        protected virtual List<NSWindow> Windows => _windows;

        public MvxMacViewPresenter(NSApplicationDelegate applicationDelegate)
        {
            _applicationDelegate = applicationDelegate;
            _windows = new List<NSWindow>();
        }

        protected virtual void RegisterAttributeTypes()
        {
            AttributeTypesToShowMethodDictionary.Add(
                typeof(MvxWindowPresentationAttribute),
                (vc, attribute, request) => ShowWindowViewController(vc, (MvxWindowPresentationAttribute)attribute, request));

            AttributeTypesToShowMethodDictionary.Add(
                typeof(MvxContentPresentationAttribute),
                (vc, attribute, request) => ShowContentViewController(vc, (MvxContentPresentationAttribute)attribute, request));

            AttributeTypesToShowMethodDictionary.Add(
                typeof(MvxModalPresentationAttribute),
                (vc, attribute, request) => ShowModalViewController(vc, (MvxModalPresentationAttribute)attribute, request));

            AttributeTypesToShowMethodDictionary.Add(
                typeof(MvxSheetPresentationAttribute),
                (vc, attribute, request) => ShowSheetViewController(vc, (MvxSheetPresentationAttribute)attribute, request));

            AttributeTypesToShowMethodDictionary.Add(
                typeof(MvxTabPresentationAttribute),
                (vc, attribute, request) => ShowTabViewController(vc, (MvxTabPresentationAttribute)attribute, request));
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is MvxClosePresentationHint)
            {
                Close((hint as MvxClosePresentationHint).ViewModelToClose);
                return;
            }

            base.ChangePresentation(hint);
        }

        public override void Show(MvxViewModelRequest request)
        {
            var view = this.CreateViewControllerFor(request);

            Show(view, request);
        }

        protected virtual void Show(IMvxMacView view, MvxViewModelRequest request)
        {
            var viewController = view as NSViewController;

            var attribute = GetPresentationAttributes(viewController);

            Action<NSViewController, MvxBasePresentationAttribute, MvxViewModelRequest> showAction;
            if (!AttributeTypesToShowMethodDictionary.TryGetValue(attribute.GetType(), out showAction))
                throw new KeyNotFoundException($"The type {attribute.GetType().Name} is not configured in the presenter dictionary");

            showAction.Invoke(viewController, attribute, request);
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

        public override void Close(IMvxViewModel toClose)
        {
            for (int i = Windows.Count - 1; i >= 0; i--)
            {
                var window = Windows[i];

                // if toClose is a sheet or modal
                if (window.ContentViewController.PresentedViewControllers.Any())
                {
                    var modal = window.ContentViewController.PresentedViewControllers
                                      .Select(v => v as MvxViewController)
                                      .FirstOrDefault(v => v.ViewModel == toClose);

                    if (modal != null)
                    {
                        window.ContentViewController.DismissViewController(modal);
                        return;
                    }
                }
                // if toClose is a tab
                var tabViewController = window.ContentViewController as IMvxTabViewController;
                if (tabViewController != null && tabViewController.CloseTabView(toClose))
                {
                    return;
                }

                // toClose is a content
                var controller = window.ContentViewController as MvxViewController;
                if (controller != null && controller.ViewModel == toClose)
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

        protected MvxBasePresentationAttribute GetPresentationAttributes(NSViewController viewController)
        {
            if (viewController is IMvxOverridePresentationAttribute vc)
            {
                var presentationAttribute = vc.PresentationAttribute();

                if (presentationAttribute != null)
                    return presentationAttribute;
            }

            var attributes = viewController.GetType().GetCustomAttribute<MvxBasePresentationAttribute>();
            if (attributes != null)
            {
                return attributes;
            }

            MvxTrace.Trace($"PresentationAttribute not found for {viewController.GetType().Name}. Assuming new window presentation");
            return new MvxWindowPresentationAttribute();
        }
    }
}