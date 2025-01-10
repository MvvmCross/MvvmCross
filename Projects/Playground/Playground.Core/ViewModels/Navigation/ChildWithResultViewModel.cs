#nullable enable
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCross.ViewModels.Result;
using Playground.Core.Models;

namespace Playground.Core.ViewModels.Navigation;

public sealed class ChildWithResultViewModel(
        ILoggerFactory logFactory,
        IMvxNavigationService navigationService,
        IMvxResultViewModelManager resultViewModelManager)
    : MvxNavigationResultSettingViewModel<SampleModel, SampleModel>(
        logFactory,
        navigationService,
        resultViewModelManager)
{
    private SampleModel _model = null!;

    public MvxAsyncCommand CloseCommand => new(DoClose);

    public string Message
    {
        get => _model.Message;
        set
        {
            _model = _model with { Message = value };
            RaisePropertyChanged();
        }
    }

    public decimal Value
    {
        get => _model.Value;
        set
        {
            _model = _model with { Value = value };
            RaisePropertyChanged();
        }
    }

    private Task DoClose()
    {
        return NavigationService.CloseSettingResult(this, _model);
    }

    public override void Prepare(SampleModel parameter)
    {
        _model = parameter;
    }
}
