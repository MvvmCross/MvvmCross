// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace MvvmCross.Platforms.Wpf.Views
{
    public interface IMvxWpfViewLoader
    {
        ValueTask<FrameworkElement> CreateView(MvxViewModelRequest request);

        ValueTask<FrameworkElement> CreateView(Type viewType);
    }
}
