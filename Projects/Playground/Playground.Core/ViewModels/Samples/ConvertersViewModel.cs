using System.Drawing;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.Samples;

public sealed class ConvertersViewModel : MvxViewModel
{
    private bool _showText = true;
    private string _colorText;
    private Color _textColor;
    private int _colorPairIndex;
    private readonly (string, Color)[] _colorPairs;

    public string UppercaseConverterTestText => "this text was lowercase";

    public string LowercaseConverterTestText => "THIS TEXT WAS UPPERCASE";

    public bool ShowText
    {
        get => _showText;
        set => SetProperty(ref _showText, value);
    }

    public string ColorText
    {
        get => _colorText;
        set => SetProperty(ref _colorText, value);
    }

    public Color TextColor
    {
        get => _textColor;
        set => SetProperty(ref _textColor, value);
    }

    public ICommand ToggleVisibilityCommand { get; }
    public ICommand ToggleColorCommand { get; }

    public ConvertersViewModel()
    {
        ToggleVisibilityCommand = new MvxCommand(DoToggleVisibility);
        ToggleColorCommand = new MvxCommand(DoToggleColor);

        _colorText = "I am green!";
        _textColor = Color.Green;
        _colorPairs = new[]
        {
            ("green", Color.Green), ("yellow", Color.Yellow), ("brown", Color.Brown), ("orange", Color.Orange)
        };
    }

    private void DoToggleColor()
    {
        var nextColorPair = _colorPairs[++_colorPairIndex % _colorPairs.Length];

        ColorText = $"I am {nextColorPair.Item1}!";
        TextColor = nextColorPair.Item2;
    }

    private void DoToggleVisibility()
    {
        ShowText = !ShowText;
    }
}
