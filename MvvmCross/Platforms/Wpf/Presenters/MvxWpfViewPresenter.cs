// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MvvmCross.Logging;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using System.Threading.Tasks;
using System.Windows.Navigation;

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
                    async (viewType, attribute, request) =>
                    {
                        var view = await WpfViewLoader.CreateView(request).ConfigureAwait(false);
                        return await ShowWindow(view, attribute, request).ConfigureAwait(false);
                    },
                    (viewModel, attribute) => CloseWindow(viewModel));

            AttributeTypesToActionsDictionary.Register<MvxContentPresentationAttribute>(
                    async (viewType, attribute, request) =>
                    {
                        var view = await WpfViewLoader.CreateView(request).ConfigureAwait(false);
                        return await ShowContentView(view, attribute, request).ConfigureAwait(false);
                    },
                    (viewModel, attribute) => CloseContentView(viewModel));
        }

        public override ValueTask<MvxBasePresentationAttribute?> CreatePresentationAttribute(Type? viewModelType, Type? viewType)
        {
            if (viewType?.IsSubclassOf(typeof(Window)) ?? false)
            {
                MvxLog.Instance.Trace($"PresentationAttribute not found for {viewType?.Name}. " +
                    $"Assuming window presentation");
                return new ValueTask<MvxBasePresentationAttribute?>(new MvxWindowPresentationAttribute());
            }

            MvxLog.Instance.Trace($"PresentationAttribute not found for {viewType?.Name}. " +
                    $"Assuming content presentation");
            return new ValueTask<MvxBasePresentationAttribute?>(new MvxContentPresentationAttribute());
        }

        public override async ValueTask<MvxBasePresentationAttribute?> GetOverridePresentationAttribute(MvxViewModelRequest request, Type viewType)
        {
            if (viewType?.GetInterface(nameof(IMvxOverridePresentationAttribute)) != null)
            {
                var viewInstance = (await WpfViewLoader.CreateView(viewType).ConfigureAwait(false)) as IDisposable;
                using (viewInstance)
                {
                    MvxBasePresentationAttribute? presentationAttribute = null;
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

        protected virtual Task<bool> ShowWindow(FrameworkElement element, MvxWindowPresentationAttribute? attribute, MvxViewModelRequest request)
        {
            Window window;
            if (element is IMvxWindow mvxWindow)
            {
                window = (Window)element;
                mvxWindow.Identifier = attribute?.Identifier ?? element.GetType().Name;
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

        protected virtual ValueTask<bool> ShowContentView(FrameworkElement element, MvxContentPresentationAttribute? attribute, MvxViewModelRequest request)
        {
            if (attribute == null) throw new NullReferenceException(nameof(attribute));

            var contentControl = FrameworkElementsDictionary.Keys.FirstOrDefault(w => (w as MvxWindow)?.Identifier == attribute.WindowIdentifier) ?? FrameworkElementsDictionary.Keys.Last();

            if (!attribute.StackNavigation && FrameworkElementsDictionary[contentControl].Count > 0)
                FrameworkElementsDictionary[contentControl].Pop(); // Close previous view

            FrameworkElementsDictionary[contentControl].Push(element);
            contentControl.Content = element;
            return new ValueTask<bool>(true);
        }

        public override async ValueTask<bool> Close(IMvxViewModel toClose)
        {
            // toClose is window
            if (FrameworkElementsDictionary.Any(i => (i.Key as IMvxWpfView)?.ViewModel == toClose) && await CloseWindow(toClose).ConfigureAwait(false))
                return true;

            // toClose is content
            if (FrameworkElementsDictionary.Any(i => i.Value.Any() && (i.Value.Peek() as IMvxWpfView)?.ViewModel == toClose) && await CloseContentView(toClose).ConfigureAwait(false))
                return true;

            MvxLog.Instance.Warn($"Could not close ViewModel type {toClose?.GetType().Name}");
            return false;
        }

        protected virtual ValueTask<bool> CloseWindow(IMvxViewModel toClose)
        {
            var item = FrameworkElementsDictionary.FirstOrDefault(i => (i.Key as IMvxWpfView)?.ViewModel == toClose);
            var contentControl = item.Key;
            if (contentControl is Window window)
            {
                FrameworkElementsDictionary.Remove(window);
                window.Close();
                return new ValueTask<bool>(true);
            }

            return new ValueTask<bool>(false);
        }

        protected virtual ValueTask<bool> CloseContentView(IMvxViewModel toClose)
        {
            var item = FrameworkElementsDictionary.FirstOrDefault(i => i.Value.Any() && (i.Value.Peek() as IMvxWpfView)?.ViewModel == toClose);
            var contentControl = item.Key;
            var elements = item.Value;

            if (elements.Count > 0)
                elements.Pop(); // Pop closing view

            if (elements.Count > 0)
            {
                contentControl.Content = elements.Peek();
                return new ValueTask<bool>(true);
            }

            // Close window if no contents
            if (contentControl is Window window)
            {
                FrameworkElementsDictionary.Remove(window);
                window.Close();
                return new ValueTask<bool>(true);
            }

            return new ValueTask<bool>(false);
        }
    }
}
