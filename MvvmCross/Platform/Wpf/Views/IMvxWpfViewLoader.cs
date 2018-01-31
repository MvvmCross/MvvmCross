// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using MvvmCross.Core.ViewModels;
using System;

namespace MvvmCross.Wpf.Views
{
    public interface IMvxWpfViewLoader
    {
        FrameworkElement CreateView(MvxViewModelRequest request);

        FrameworkElement CreateView(Type viewType);
    }
}