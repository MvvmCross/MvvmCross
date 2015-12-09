// MvxWrappedFrame.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsCommon.Views
{
    using System;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class MvxWrappedFrame : IMvxWindowsFrame
    {
        private readonly Frame _frame;

        public MvxWrappedFrame(Frame frame)
        {
            this._frame = frame;
        }

        public Control UnderlyingControl => this._frame;

        public object Content => this._frame.Content;

        public bool CanGoBack => this._frame.CanGoBack;

        public bool Navigate(Type viewType, object parameter)
        {
            return this._frame.Navigate(viewType, parameter);
        }

        public void GoBack()
        {
            this._frame.GoBack();
        }

        public void ClearValue(DependencyProperty property)
        {
            this._frame.ClearValue(property);
        }

        public object GetValue(DependencyProperty property)
        {
            return this._frame.GetValue(property);
        }

        public void SetValue(DependencyProperty property, object value)
        {
            this._frame.SetValue(property, value);
        }

        public void SetNavigationState(string state)
        {
            this._frame.SetNavigationState(state);
        }

        public string GetNavigationState()
        {
            return this._frame.GetNavigationState();
        }
    }
}