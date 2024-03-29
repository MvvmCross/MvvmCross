// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Console.Views;

public class MvxConsoleView<T>
    : IMvxConsoleView
    where T : class, IMvxViewModel
{
    public object? DataContext { get; set; }

    public T? ViewModel
    {
        get => (T?)DataContext;
        set => DataContext = value;
    }

    IMvxViewModel? IMvxView.ViewModel
    {
        get => DataContext as IMvxViewModel;
        set => DataContext = (T?)value;
    }

    public Type ViewModelType => typeof(T);

    public void HackSetViewModel(object? viewModel)
    {
        ViewModel = (T?)viewModel;
        OnViewModelChanged();
    }

    public virtual bool HandleInput(string input)
    {
        return false;
    }

    protected virtual void OnViewModelChanged()
    {
    }
}
