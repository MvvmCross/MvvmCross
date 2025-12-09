using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.Navigation;

public class MultiBackStackViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService) : MvxNavigationViewModel(logFactory, navigationService)
{
    private bool _initialNavigationDone = false;

    private async Task ShowInitialViewModelsExecute()
    {
        if (_initialNavigationDone)
            return;
        _initialNavigationDone = true;
        await NavigationService.Navigate<MultiBackStackTab1ViewModel>();
    }

    public override void ViewAppearing()
    {
        base.ViewAppearing();
        //we can only show tabs after our host view is created
        Task.Run(ShowInitialViewModelsExecute);
    }

    protected override void SaveStateToBundle(IMvxBundle bundle)
    {
        base.SaveStateToBundle(bundle);
        bundle.Data[nameof(_initialNavigationDone)] = _initialNavigationDone.ToString();
    }

    protected override void ReloadFromBundle(IMvxBundle state)
    {
        base.ReloadFromBundle(state);
        if (state.Data.TryGetValue(nameof(_initialNavigationDone), out string initDone))
            _ = bool.TryParse(initDone, out _initialNavigationDone);
    }
}

public class MultiBackStackTab1ViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService) : MvxNavigationViewModel(logFactory, navigationService)
{
    public IMvxCommand GoDeeperCommand { get; init; } = new MvxAsyncCommand(async () => await navigationService.Navigate<MultiBackStackInnerViewModel>());
}

public class MultiBackStackTab2ViewModel : MvxViewModel;

public class MultiBackStackInnerViewModel : MvxNavigationViewModel<int>
{
    public IMvxCommand GoDeeperCommand { get; init; }
    public IMvxCommand CloseCommand { get; init; }

    private int _depth;
    public int Depth
    {
        get => _depth;
        set => SetProperty(ref _depth, value);
    }

    public MultiBackStackInnerViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService) : base(logFactory, navigationService)
    {
        GoDeeperCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<MultiBackStackInnerViewModel, int>(Depth + 1));
        CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this));
    }

    public override void Prepare()
    {
        base.Prepare();
        Depth = 1;
    }

    public override void Prepare(int parameter)
    {
        Depth = parameter;
    }

    protected override void SaveStateToBundle(IMvxBundle bundle)
    {
        base.SaveStateToBundle(bundle);
        bundle.Data[nameof(Depth)] = Depth.ToString();
    }

    protected override void ReloadFromBundle(IMvxBundle state)
    {
        base.ReloadFromBundle(state);
        if (state.Data.TryGetValue(nameof(Depth), out string ds))
            _ = int.TryParse(ds, out _depth);
    }
}
