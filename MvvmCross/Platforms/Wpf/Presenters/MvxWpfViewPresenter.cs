// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Wpf.Presenters
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
                    _wpfViewLoader = Mvx.IoCProvider.Resolve<IMvxWpfViewLoader>();
                return _wpfViewLoader;
            }
        }

        private Dictionary<ContentControl, Stack<FrameworkElement>> _frameworkElementsDictionary;
        protected Dictionary<ContentControl, Stack<FrameworkElement>> FrameworkElementsDictionary
        {
            get
            {
                if (_frameworkElementsDictionary == null)
                    _frameworkElementsDictionary = new Dictionary<ContentControl, Stack<FrameworkElement>>();
                return _frameworkElementsDictionary;
            }
        }

        protected MvxWpfViewPresenter()
        {
        }

        public MvxWpfViewPresenter(ContentControl contentControl) // Accept ContentControl only for the first host view 
        {
            if (contentControl is Window window)
                window.Closed += Window_Closed;

            FrameworkElementsDictionary.Add(contentControl, new Stack<FrameworkElement>());
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Register<MvxWindowPresentationAttribute>(
                    (_, attribute, request) =>
                    {
                        var view = WpfViewLoader.CreateView(request);
                        return ShowWindow(view, attribute, request);
                    },
                    (viewModel, _) => CloseWindow(viewModel));

            AttributeTypesToActionsDictionary.Register<MvxContentPresentationAttribute>(
                    (_, attribute, request) =>
                    {
                        var view = WpfViewLoader.CreateView(request);
                        return ShowContentView(view, attribute, request);
                    },
                    (viewModel, _) => CloseContentView(viewModel));
        }

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (viewType.IsSubclassOf(typeof(Window)))
            {
                MvxLogHost.Default?.Log(LogLevel.Trace, "PresentationAttribute not found for {ViewTypeName}. " +
                    "Assuming window presentation", viewType.Name);
                return new MvxWindowPresentationAttribute { ViewModelType = viewModelType, ViewType = viewType };
            }

            MvxLogHost.Default?.Log(LogLevel.Trace, "PresentationAttribute not found for {ViewTypeName}. " +
                    "Assuming content presentation", viewType.Name);
            return new MvxContentPresentationAttribute { ViewType = viewType, ViewModelType = viewModelType };
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

        protected virtual Task<bool> ShowWindow(FrameworkElement element, MvxWindowPresentationAttribute attribute, MvxViewModelRequest request)
        {
            Window window;
            if (element is IMvxWindow mvxWindow)
            {
                window = (Window)element;
                mvxWindow.Identifier = attribute.Identifier ?? element.GetType().Name;
            }
            else if (element is Window normalWindow)
            {
                // Accept normal Window class
                window = normalWindow;
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
            FrameworkElementsDictionary.Add(window, new Stack<FrameworkElement>());

            if (!(element is Window))
            {
                FrameworkElementsDictionary[window].Push(element);
                window.Content = element;
            }

            if (attribute.Modal)
                window.ShowDialog();
            else
                window.Show();
            return Task.FromResult(true);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var window = sender as Window;
            window.Closed -= Window_Closed;

            if (FrameworkElementsDictionary.ContainsKey(window))
                FrameworkElementsDictionary.Remove(window);
        }

        protected virtual Task<bool> ShowContentView(FrameworkElement element, MvxContentPresentationAttribute attribute, MvxViewModelRequest request)
        {
            var contentControl = FrameworkElementsDictionary.Keys.FirstOrDefault(w => (w as MvxWindow)?.Identifier == attribute.WindowIdentifier) ?? FrameworkElementsDictionary.Keys.Last();

            if (!attribute.StackNavigation && FrameworkElementsDictionary[contentControl].Any())
                FrameworkElementsDictionary[contentControl].Pop(); // Close previous view

            FrameworkElementsDictionary[contentControl].Push(element);
            contentControl.Content = element;
            return Task.FromResult(true);
        }

        public override async Task<bool> Close(IMvxViewModel viewModel)
        {
            // toClose is window
            if (FrameworkElementsDictionary.Any(i => (i.Key as IMvxWpfView)?.ViewModel == viewModel) && await CloseWindow(viewModel))
                return true;

            // toClose is content
            if (FrameworkElementsDictionary.Any(i => i.Value.Any() && (i.Value.Peek() as IMvxWpfView)?.ViewModel == viewModel) && await CloseContentView(viewModel))
                return true;

            MvxLogHost.Default?.Log(LogLevel.Warning, "Could not close ViewModel type {ViewModelTypeName}", viewModel.GetType().Name);
            return false;
        }

        protected virtual Task<bool> CloseWindow(IMvxViewModel toClose)
        {
            var item = FrameworkElementsDictionary.FirstOrDefault(i => (i.Key as IMvxWpfView)?.ViewModel == toClose);
            var contentControl = item.Key;
            if (contentControl is Window window)
            {
                FrameworkElementsDictionary.Remove(window);
                window.Close();
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        protected virtual Task<bool> CloseContentView(IMvxViewModel toClose)
        {
            var item = FrameworkElementsDictionary.FirstOrDefault(i => i.Value.Any() && (i.Value.Peek() as IMvxWpfView)?.ViewModel == toClose);
            var contentControl = item.Key;
            var elements = item.Value;

            if (elements.Any())
                elements.Pop(); // Pop closing view

            if (elements.Any())
            {
                contentControl.Content = elements.Peek();
                return Task.FromResult(true);
            }

            // Close window if no contents
            if (contentControl is Window window)
            {
                FrameworkElementsDictionary.Remove(window);
                window.Close();
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
