using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Cirrious.MvvmCross.WindowsCommon.Views.Helpers;

namespace Cirrious.MvvmCross.WindowsCommon.Views.Handlers
{
    public class NavigationKeyHandler
    {
        private static readonly Type _hardwareButtonsType;
        private static BackKeyPressedHandler _handler;

        static NavigationKeyHandler()
        {
            _hardwareButtonsType = Type.GetType(
                "Windows.Phone.UI.Input.HardwareButtons, " +
                "Windows, Version=255.255.255.255, Culture=neutral, " +
                "PublicKeyToken=null, ContentType=WindowsRuntime");
        }

        private readonly MtPage _page;
        private readonly List<Func<CancelEventArgs, Task>> _goBackActions;

        public NavigationKeyHandler(MtPage page)
        {
            _page = page;
            _goBackActions = new List<Func<CancelEventArgs, Task>>();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;

            page.Loaded += (sender, args) =>
            {
                if (IsRunningOnPhone)
                {
                    if (_handler == null)
                        _handler = new BackKeyPressedHandler();

                    _handler.Add(page, s => TryGoBackAsync());
                }
                else
                {
                    if (page.ActualHeight == Window.Current.Bounds.Height &&
                        page.ActualWidth == Window.Current.Bounds.Width)
                    {
                        Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += OnAcceleratorKeyActivated;
                        Window.Current.CoreWindow.PointerPressed += OnPointerPressed;
                    }
                }
            };

            page.Unloaded += (sender, args) =>
            {
                if (IsRunningOnPhone)
                    _handler.Remove(_page);
                else
                {
                    Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated -= OnAcceleratorKeyActivated;
                    Window.Current.CoreWindow.PointerPressed -= OnPointerPressed;
                }
            };
        }

        /// <summary>Gets a value indicating whether the application is running on a Windows Phone. </summary>
        public static bool IsRunningOnPhone
        {
            get { return _hardwareButtonsType != null; }
        }

        public Func<CancelEventArgs, Task> AddGoBackHandler(Action<CancelEventArgs> action)
        {
            var func = new Func<CancelEventArgs, Task>(args =>
            {
                action(args);
                return null;
            });

            AddGoBackAsyncHandler(func);
            return func;
        }

        public void AddGoBackAsyncHandler(Func<CancelEventArgs, Task> action)
        {
            _goBackActions.Insert(0, action);
        }

        public void RemoveGoBackAsyncHandler(Func<CancelEventArgs, Task> action)
        {
            _goBackActions.Remove(action);
        }

        private void OnAcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            _page.OnKeyActivated(args);

            if (args.KeyStatus.IsKeyReleased)
                _page.OnKeyUp(args);

            if (args.Handled)
                return;

            var virtualKey = args.VirtualKey;
            if (args.KeyStatus.IsKeyReleased)
            {
                var isLeftOrRightKey =
                    _page.UseAltLeftOrRightToNavigate && (
                        virtualKey == VirtualKey.Left ||
                        virtualKey == VirtualKey.Right ||
                        (int)virtualKey == 166 ||
                        (int)virtualKey == 167);

                var isBackKey = _page.UseBackKeyToNavigate && virtualKey == VirtualKey.Back;

                if (isLeftOrRightKey || isBackKey)
                {
                    if (PopupHelper.IsPopupVisible)
                        return;

                    var element = FocusManager.GetFocusedElement();
                    if (element is FrameworkElement && PopupHelper.IsInPopup((FrameworkElement)element))
                        return;

                    if (isBackKey)
                    {
                        if (!(element is TextBox) && !(element is PasswordBox) &&
                                 (_page.UseBackKeyToNavigateInWebView || !(element is WebView)) && _page.Frame.CanGoBack)
                        {
                            args.Handled = true;
                            TryGoBackAsync();
                        }
                    }
                    else
                    {
                        var altKey = Keyboard.IsAltKeyDown;
                        var controlKey = Keyboard.IsControlKeyDown;
                        var shiftKey = Keyboard.IsShiftKeyDown;

                        var noModifiers = !altKey && !controlKey && !shiftKey;
                        var onlyAlt = altKey && !controlKey && !shiftKey;

                        if (((int)virtualKey == 166 && noModifiers) || (virtualKey == VirtualKey.Left && onlyAlt))
                        {
                            args.Handled = true;
                            TryGoBackAsync();
                        }
                        else if (((int)virtualKey == 167 && noModifiers) || (virtualKey == VirtualKey.Right && onlyAlt))
                        {
                            args.Handled = true;
                            TryGoForwardAsync();
                        }
                    }
                }
            }
        }

        private void OnPointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            if (!_page.UsePointerButtonsToNavigate)
                return;

            var properties = args.CurrentPoint.Properties;
            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed || properties.IsMiddleButtonPressed)
                return;

            var backPressed = properties.IsXButton1Pressed;
            var forwardPressed = properties.IsXButton2Pressed;
            if (backPressed ^ forwardPressed)
            {
                args.Handled = true;

                if (backPressed)
                    TryGoBackAsync();

                if (forwardPressed)
                    TryGoForwardAsync();
            }
        }

        private async void TryGoForwardAsync()
        {
            if (_page.Frame.CanGoForward)
                await _page.Frame.GoForwardAsync();
        }

        private bool TryGoBackAsync()
        {
            if (_page.Frame.CanGoBack)
            {
                var args = new CancelEventArgs();
                CallGoBackActions(args, _goBackActions);
                return _page.Frame.CanGoBack || args.Cancel;
            }
            return false;
        }

        private void CallGoBackActions(CancelEventArgs e, List<Func<CancelEventArgs, Task>> actions)
        {
            Func<CancelEventArgs, Task> lastAction = null;
            var copy = new CancelEventArgs();
            foreach (var action in actions)
            {
                lastAction = action;

                var task = action(copy);
                if (task != null && !task.IsCompleted)
                {
                    e.Cancel = true;
                    task.ContinueWith(t => { CheckGoBackActions(actions, action, copy, true); });
                    return;
                }

                if (copy.Cancel)
                    break;
            }

            e.Cancel = copy.Cancel;
            CheckGoBackActions(actions, lastAction, copy, false);
        }

        private void CheckGoBackActions(List<Func<CancelEventArgs, Task>> actions, Func<CancelEventArgs, Task> action, CancelEventArgs copy, bool perform)
        {
            if (!copy.Cancel)
            {
                var nextActions = actions.Skip(actions.IndexOf(action) + 1).ToList();
                if (nextActions.Count == 0)
                    GoBack();
                else
                {
                    CallGoBackActions(copy, nextActions);
                    if (!copy.Cancel)
                        GoBack();
                }
            }
        }

        private void GoBack()
        {
            _page.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async delegate
            {
                if (_page.Frame.CanGoBack && !_page.Frame.IsNavigating)
                    await _page.Frame.GoBackAsync();
                else
                {
                    // TODO: Go back out of app on WP
                }
            });
        }
    }
}
