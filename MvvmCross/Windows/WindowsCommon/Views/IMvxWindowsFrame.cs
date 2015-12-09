// IMvxWindowsFrame.cs
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