// MvxMacViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
using System;
using System.Collections.Generic;
using System.Linq;

using AppKit;
using CoreGraphics;
using MvvmCross.Core.ViewModels;
using MvvmCross.Mac.Views.Presenters.Attributes;
using System.Reflection;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Mac.Views.Presenters
{
    public class MvxMacViewPresenter
        : MvxBaseMacViewPresenter
    {
        private readonly INSApplicationDelegate _applicationDelegate;
        private List<NSWindow> _windows;
        protected Dictionary<Type, Action<NSViewController, MvxBasePresentationAttribute, MvxViewModelRequest>> _attributeTypesToShowMethodDictionary;

        protected virtual INSApplicationDelegate ApplicationDelegate => _applicationDelegate;

        protected virtual List<NSWindow> Windows => _windows;

        public MvxMacViewPresenter(NSApplicationDelegate applicationDelegate)
        {
            _applicationDelegate = applicationDelegate;
            _windows = new List<NSWindow>();

            _attributeTypesToShowMethodDictionary = new Dictionary<Type, Action<NSViewController, MvxBasePresentationAttribute, MvxViewModelRequest>>();

            RegisterAttributeTypes();
        }

        protected virtual void RegisterAttributeTypes()
        {
            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxWindowPresentationAttribute),
                (vc, attribute, request) => ShowWindowViewController(vc, (MvxWindowPresentationAttribute)attribute, request));

            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxChildPresentationAttribute),
                (vc, attribute, request) => ShowChildViewController(vc, (MvxChildPresentationAttribute)attribute, request));

            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxModalPresentationAttribute),
                (vc, attribute, request) => ShowModalViewController(vc, (MvxModalPresentationAttribute)attribute, request));

            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxSheetPresentationAttribute),
                (vc, attribute, request) => ShowSheetViewController(vc, (MvxSheetPresentationAttribute)attribute, request));
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
            if (!_attributeTypesToShowMethodDictionary.TryGetValue(attribute.GetType(), out showAction))
                throw new KeyNotFoundException($"The type {attribute.GetType().Name} is not configured in the presenter dictionary");

            showAction.Invoke(viewController, attribute, request);
        }

        protected virtual void ShowWindowViewController(
            NSViewController viewController,
            MvxWindowPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var window = new NSWindow(
                new CGRect(attribute.PositionX, attribute.PositionY, attribute.Width, attribute.Height),
                attribute.WindowStyle,
                attribute.BufferingType,
                false,
                NSScreen.MainScreen)
            {
                Identifier = attribute.Identifier ?? viewController.GetType().Name
            };

            if (!string.IsNullOrEmpty(viewController.Title))
                window.Title = viewController.Title;

            Windows.Add(window);
            window.ContentView = viewController.View;
            window.ContentViewController = viewController;

            var windowController = CreateWindowController(window);
            windowController.ShouldCascadeWindows = true;
            windowController.ShowWindow(null);
        }

        protected virtual void ShowChildViewController(
            NSViewController viewController,
            MvxChildPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var window = Windows.FirstOrDefault(w => w.Identifier == attribute.WindowIdentifier) ?? Windows.Last();

            if (!string.IsNullOrEmpty(viewController.Title))
                window.Title = viewController.Title;

            // Setting current content view to window
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

        public override void Close(IMvxViewModel toClose)
        {
            for (int i = Windows.Count - 1; i >= 0; i--)
            {
                var window = Windows[i];
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
                else
                {
                    var controller = window.ContentViewController as MvxViewController;
                    if (controller != null && controller.ViewModel == toClose)
                    {
                        window.Close();
                        Windows.Remove(window);
                        return;
                    }
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