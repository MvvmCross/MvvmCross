// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Control = Microsoft.UI.Xaml.Controls.Control;

namespace MvvmCross.Platforms.WinUi.Views
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
