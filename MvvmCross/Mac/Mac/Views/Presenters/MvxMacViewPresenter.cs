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
using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Mac.Views.Presenters.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Mac.Views;
using System.Reflection;

namespace MvvmCross.Mac.Views.Presenters
{
    public class MvxMacViewPresenter
        : MvxBaseMacViewPresenter
    {
        private readonly INSApplicationDelegate _applicationDelegate;
        private NSWindow _window, _presentedModal, _presentedSheet;
        protected Dictionary<Type, Action<NSViewController, MvxBasePresentationAttribute, MvxViewModelRequest>> _attributeTypesToShowMethodDictionary;

        protected virtual INSApplicationDelegate ApplicationDelegate => _applicationDelegate;

        protected virtual NSWindow Window => _window;

        public MvxMacViewPresenter(NSApplicationDelegate applicationDelegate, NSWindow window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;

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
            if (_presentedSheet != null)
            {
                throw new MvxException("Only one sheet at a time is allowed!");
            }

            var window = new NSWindow(this.GetRectForWindowWithViewController(viewController, attribute), NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled, NSBackingStore.Buffered, false, NSScreen.MainScreen);

            // Setting Title, if available
            if (!string.IsNullOrEmpty(viewController.Title))
            {
                window.Title = viewController.Title;
            }

            window.ContentView = viewController.View;

            // WindowController to display the new Window
            NSWindowController windowController = new NSWindowController(window);
            windowController.ShouldCascadeWindows = true; // Attempt to change the size a bit of the Window, so we can see something changed
            windowController.ShowWindow(null);
        }

        protected virtual void ShowChildViewController(
            NSViewController viewController,
            MvxChildPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // If NSApplication.MainWindow is null, then we are properbly starting the app, and should select the Window we got on creation time
            var currentWindow = NSApplication.SharedApplication.MainWindow ?? Window;

            // Setting Title, if available
            if (!string.IsNullOrEmpty(viewController.Title))
            {
                currentWindow.Title = viewController.Title;
            }

            // Setting current content view to window
            currentWindow.ContentView = viewController.View;
        }

        protected virtual void ShowModalViewController(
            NSViewController viewController,
            MvxModalPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (_presentedModal != null)
            {
                throw new MvxException("Only one modal at a time is allowed!");
            }

            // The Window we will present the Modal as
            _presentedModal = new NSWindow(this.GetRectForWindowWithViewController(viewController, attribute), NSWindowStyle.Titled, NSBackingStore.Buffered, false, NSScreen.MainScreen);

            // Setting Title if Window, if available
            if (!string.IsNullOrEmpty(viewController.Title))
            {
                _presentedModal.Title = viewController.Title;
            }

            _presentedModal.ContentView = viewController.View;

            // Present the new modal Window with NSApplication
            NSApplication.SharedApplication.RunModalForWindow(_presentedModal);
        }

        protected virtual void ShowSheetViewController(
            NSViewController viewController,
            MvxSheetPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // Creating Window for the Sheet presentation
            _presentedSheet = new NSWindow(this.GetRectForWindowWithViewController(viewController, attribute), NSWindowStyle.Resizable, NSBackingStore.Buffered, false, NSScreen.MainScreen);

            // Setting content view for the new Window
            _presentedSheet.ContentView = viewController.View;

            // Showing the Sheet with NSApplication. We can use MainWindow here, because we can't present a Sheet as the first opening Window
            NSApplication.SharedApplication.BeginSheet(_presentedSheet, NSApplication.SharedApplication.MainWindow);
        }

        //protected virtual void Show(NSViewController viewController, MvxViewModelRequest request)
        //{
        //    var viewController = view as NSViewController;

        //    var attribute = GetPresentationAttributes(viewController);

        //    Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest> showAction;
        //    if (!_attributeTypesToShowMethodDictionary.TryGetValue(attribute.GetType(), out showAction))
        //        throw new KeyNotFoundException($"The type {attribute.GetType().Name} is not configured in the presenter dictionary");

        //    showAction.Invoke(viewController, attribute, request);


        //    while (Window.ContentView.Subviews.Any())
        //    {
        //        Window.ContentView.Subviews[0].RemoveFromSuperview();
        //    }

        //    if (!viewController.ViewLoaded)
        //        viewController.LoadView();

        //    Window.ContentView = viewController.View;

        //    AddLayoutConstraints(viewController, request);
        //}

        /// <summary>
        /// Overridable method to dictate the size and position of Windows that are about the be created
        /// </summary>
        public virtual CGRect GetRectForWindowWithViewController(NSViewController viewController, MvxBasePresentationAttribute attribute)
        {
            return this.Window?.Frame ?? new CGRect(200, 200, 600, 400);
        }


        //protected virtual void AddLayoutConstraints(NSViewController viewController, MvxViewModelRequest request)
        //{
        //    var child = viewController.View;
        //    var container = Window.ContentView;

        //    // See http://blog.xamarin.com/autolayout-with-xamarin.mac/ for more on constraints
        //    // as well as https://gist.github.com/garuma/3de3bbeb954ad5679e87 (latter may be helpful as tools...)

        //    child.TranslatesAutoresizingMaskIntoConstraints = false;
        //    container.AddConstraints(new[]
        //        { NSLayoutAttribute.Left, NSLayoutAttribute.Right, NSLayoutAttribute.Top, NSLayoutAttribute.Bottom }
        //        .Select(attr => NSLayoutConstraint.Create(child, attr, NSLayoutRelation.Equal, container, attr, 1, 0)).ToArray());
        //}

        public override void Close(IMvxViewModel toClose)
        {
            if (_presentedModal != null)
            {
                // We should close the Modal

                NSApplication.SharedApplication.StopModal();

                _presentedModal.Close();
                _presentedModal = null;

            }
            else if (_presentedSheet != null)
            {
                // We should close the Sheet

                NSApplication.SharedApplication.EndSheet(_presentedSheet);

                _presentedSheet.Close();
                _presentedSheet = null;

            }
            else
            {
                // "Normal" ViewController close

                // The Window where the current ViewModel/View Controller should be presented in
                //var window = _viewModelWindowDictionary[toClose];
                //var stack = _windowViewControllers[window];

                //if (stack.Count > 1)
                //{

                //	// Removing the top View Controller (because it's the current one)
                //	stack.Pop();

                //	// Getting the one before the one we closed to present it
                //	var viewController = stack.Peek();

                //	// Setting content view, and title for Window, if available
                //	window.ContentView = viewController.View;
                //	if (!string.IsNullOrEmpty(viewController.Title))
                //	{
                //		window.Title = viewController.Title;
                //	}

                //	// Gone from ViewModel index
                //	_viewModelWindowDictionary.Remove(toClose);
                //}
                //else
                //{
                //	// Last one? We will close the current Window
                //	stack.Clear();
                //	_windowViewControllers.Remove(window);

                //	window.WillClose -= Window_WillClose;
                //	window.Close();
                //}
            }
        }

        protected MvxBasePresentationAttribute GetPresentationAttributes(NSViewController viewController)
        {
            var type = viewController.GetType();
            var attributes = type.GetCustomAttribute<MvxBasePresentationAttribute>();
            if (attributes != null)
            {
                return attributes;
            }

            //if (MasterNavigationController == null)
            //{
            //	MvxTrace.Trace($"PresentationAttribute nor MasterNavigationController found for {viewController.GetType().Name}. Assuming Root presentation");
            //	return new MvxRootPresentationAttribute() { WrapInNavigationController = true };
            //}

            //MvxTrace.Trace($"PresentationAttribute not found for {viewController.GetType().Name}. Assuming animated Child presentation");
            return new MvxWindowPresentationAttribute();
        }
    }
}