// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MvvmCross.Platforms.Uap.Views
{
    public interface IMvxWindowsFrame
    {
        Control UnderlyingControl { get; }
        object Content { get; }
        bool CanGoBack { get; }
        bool Navigate(Type viewType, object parameter);
        void GoBack();
        void ClearValue(DependencyProperty property);
        object GetValue(DependencyProperty property);
        void SetValue(DependencyProperty property, object value);
        void SetNavigationState(string state);
        string GetNavigationState();
    }
}
