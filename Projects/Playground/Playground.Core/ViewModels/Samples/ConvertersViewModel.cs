using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.Samples;

public sealed class ConvertersViewModel : MvxViewModel
{
    private bool _showText = true;
    
    public string UppercaseConverterTestText => "this text was lowercase";

    public string LowercaseConverterTestText => "THIS TEXT WAS UPPERCASE";

    public bool ShowText
    {
        get => _showText;
        set => SetProperty(ref _showText, value);
    }
    
    public ICommand ToggleVisibilityCommand { get; }

    public ConvertersViewModel()
    {
        ToggleVisibilityCommand = new MvxCommand(DoToggleVisibility);
    }

    private void DoToggleVisibility()
    {
        ShowText = !ShowText;
    }
}
