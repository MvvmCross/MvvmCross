// MvxWrappedFrame.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Cirrious.MvvmCross.WindowsCommon.Views
{
    public class MvxWrappedFrame : IMvxWindowsFrame
    {
        private readonly Frame _frame;

        public MvxWrappedFrame(Frame frame)
        {
            _frame = frame;
        }

        public Control UnderlyingControl => _frame;

        public object Content => _frame.Content;

        public bool CanGoBack => _frame.CanGoBack;

        public bool Navigate(Type viewType, object parameter)
        {
            return _frame.Navigate(viewType, parameter);
        }

        public void GoBack()
        {
            _frame.GoBack();
        }

        public void ClearValue(DependencyProperty property)
        {
            _frame.ClearValue(property);
        }

        public object GetValue(DependencyProperty property)
        {
            return _frame.GetValue(property);
        }

        public void SetValue(DependencyProperty property, object value)
        {
            _frame.SetValue(property, value);
        }

        public void SetNavigationState(string state)
        {
            _frame.SetNavigationState(state);
        }

        public string GetNavigationState()
        {
            return _frame.GetNavigationState();
        }
    }
}