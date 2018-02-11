﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MvvmCross.Logging;
using MvvmCross.Platform.Wpf.Presenters.Attributes;
using MvvmCross.Platform.Wpf.Views;
using MvvmCross.ViewModels;
using MvvmCross.Presenters;

namespace MvvmCross.Platform.Wpf.Presenters
{
    public class MvxWpfViewPresenter
        : MvxAttributeViewPresenter, IMvxWpfViewPresenter
    {
        private IMvxWpfViewLoader _wpfViewLoader;
        protected IMvxWpfViewLoader WpfViewLoader
        {
            get
            {
                if (_wpfViewLoader == null)
                    _wpfViewLoader = Mvx.Resolve<IMvxWpfViewLoader>();
                return _wpfViewLoader;
            }
        }

        private readonly Dictionary<ContentControl, Stack<FrameworkElement>> _frameworkElementsDictionary = new Dictionary<ContentControl, Stack<FrameworkElement>>();

        protected MvxWpfViewPresenter()
        {
        }

        public MvxWpfViewPresenter(ContentControl contentControl) // Accept ContentControl only for the first host view 
        {
            if (contentControl is Window window)
                window.Closed += Window_Closed;

            _frameworkElementsDictionary.Add(contentControl, new Stack<FrameworkElement>());
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Add(
                typeof(MvxWindowPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var view = WpfViewLoader.CreateView(request);
                        ShowWindow(view, (MvxWindowPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => CloseWindow(viewModel)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxContentPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var view = WpfViewLoader.CreateView(request);
                        ShowContentView(view, (MvxContentPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => CloseContentView(viewModel)
                });
        }

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (viewType.IsSubclassOf(typeof(Window)))
            {
                MvxLog.Instance.Trace($"PresentationAttribute not found for {viewType.Name}. " +
                    $"Assuming window presentation");
                return new MvxWindowPresentationAttribute();
            }

            MvxLog.Instance.Trace($"PresentationAttribute not found for {viewType.Name}. " +
                    $"Assuming content presentation");
            return new MvxContentPresentationAttribute();
        }

        public override MvxBasePresentationAttribute GetOverridePresentationAttribute(MvxViewModelRequest request, Type viewType)
        {
            if (viewType?.GetInterface(nameof(IMvxOverridePresentationAttribute)) != null)
            {
                var viewInstance = WpfViewLoader.CreateView(viewType) as IDisposable;
                using (viewInstance)
                {
                    MvxBasePresentationAttribute presentationAttribute = null;
                    if (viewInstance is IMvxOverridePresentationAttribute overrideInstance)
                        presentationAttribute = overrideInstance.PresentationAttribute(request);

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

        protected virtual void ShowWindow(FrameworkElement element, MvxWindowPresentationAttribute attribute, MvxViewModelRequest request)
        {
            Window window;
            if (element is MvxWindow)
            {
                window = (Window)element;
                ((MvxWindow)window).Identifier = attribute.Identifier ?? element.GetType().Name;
            }
            else if (element is Window)
            {
                // Accept normal Window class
                window = (Window)element;
            }
            else
            {
                // Wrap in window
                window = new MvxWindow
                {
                    Identifier = attribute.Identifier ?? element.GetType().Name
                };
            }
            window.Closed += Window_Closed;
            _frameworkElementsDictionary.Add(window, new Stack<FrameworkElement>());

            if (!(element is Window))
            {
                _frameworkElementsDictionary[window].Push(element);
                window.Content = element;
            }

            if (attribute.Modal)
                window.ShowDialog();
            else
                window.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var window = sender as Window;
            window.Closed -= Window_Closed;

            if (_frameworkElementsDictionary.ContainsKey(window))
                _frameworkElementsDictionary.Remove(window);
        }

        protected virtual void ShowContentView(FrameworkElement element, MvxContentPresentationAttribute attribute, MvxViewModelRequest request)
        {
            var contentControl = _frameworkElementsDictionary.Keys.FirstOrDefault(w => (w as MvxWindow)?.Identifier == attribute.WindowIdentifier) ?? _frameworkElementsDictionary.Keys.Last();

            if (!attribute.StackNavigation && _frameworkElementsDictionary[contentControl].Any())
                _frameworkElementsDictionary[contentControl].Pop(); // Close previous view

            _frameworkElementsDictionary[contentControl].Push(element);
            contentControl.Content = element;
        }

        public override void Close(IMvxViewModel toClose)
        {
            // toClose is window
            if (_frameworkElementsDictionary.Any(i => (i.Key as IMvxWpfView)?.ViewModel == toClose) && CloseWindow(toClose))
                return;

            // toClose is content
            if (_frameworkElementsDictionary.Any(i => i.Value.Any() && (i.Value.Peek() as IMvxWpfView)?.ViewModel == toClose) && CloseContentView(toClose))
                return;

            MvxLog.Instance.Warn($"Could not close ViewModel type {toClose.GetType().Name}");
        }

        protected virtual bool CloseWindow(IMvxViewModel toClose)
        {
            var item = _frameworkElementsDictionary.FirstOrDefault(i => (i.Key as IMvxWpfView)?.ViewModel == toClose);
            var contentControl = item.Key;
            if (contentControl is Window window)
            {
                _frameworkElementsDictionary.Remove(window);
                window.Close();
                return true;
            }

            return false;
        }

        protected virtual bool CloseContentView(IMvxViewModel toClose)
        {
            var item = _frameworkElementsDictionary.FirstOrDefault(i => i.Value.Any() && (i.Value.Peek() as IMvxWpfView)?.ViewModel == toClose);
            var contentControl = item.Key;
            var elements = item.Value;

            if (elements.Any())
                elements.Pop(); // Pop closing view

            if (elements.Any())
            {
                contentControl.Content = elements.Peek();
                return true;
            }

            // Close window if no contents
            if (contentControl is Window window)
            {
                _frameworkElementsDictionary.Remove(window);
                window.Close();
                return true;
            }

            return false;
        }
    }
}
