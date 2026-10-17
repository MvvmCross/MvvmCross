#nullable enable

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.MultiWindow;

// Window identity is an opaque string; no UIKit objects enter the ViewModels.
public record WindowContext(string WindowId, CancellationToken Disconnected);

public class SharedCounter : MvxNotifyPropertyChanged
{
    private int _count;
    public int Count
    {
        get => _count;
        set => SetProperty(ref _count, value);
    }
}

public class WindowViewModel : MvxViewModel<WindowContext>
{
    private readonly IMvxNavigationService _navigation;
    private WindowContext _context = null!;

    public SharedCounter Counter { get; }
    public string WindowId => _context.WindowId;
    public IMvxCommand Increment { get; }
    public IMvxAsyncCommand ShowChild { get; }
    public IMvxAsyncCommand ShowModal { get; }
    public IMvxAsyncCommand ShowDelayedChild { get; }
    public IMvxAsyncCommand Close { get; }

    public WindowViewModel(IMvxNavigationService navigation, SharedCounter counter)
    {
        _navigation = navigation;
        Counter = counter;
        Increment = new MvxCommand(() => Counter.Count++);
        ShowChild = new MvxAsyncCommand(Navigate<WindowChildViewModel>);
        ShowModal = new MvxAsyncCommand(Navigate<WindowModalViewModel>);
        ShowDelayedChild = new MvxAsyncCommand(async () =>
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(3), _context.Disconnected);
                await Navigate<WindowChildViewModel>();
            }
            catch (OperationCanceledException) when (_context.Disconnected.IsCancellationRequested)
            {
                // UIKit disconnected this scene while the command was waiting.
            }
        });
        Close = new MvxAsyncCommand(() => _navigation.Close(this, _context.Disconnected));
    }

    public override void Prepare(WindowContext parameter) => _context = parameter;

    private Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class, IMvxViewModel<WindowContext>
        => _context.Disconnected.IsCancellationRequested
            ? Task.FromResult(false)
            : _navigation.Navigate<T, WindowContext>(_context,
                new MvxWindowPresentationBundle(WindowId), _context.Disconnected);
}

public class WindowChildViewModel(IMvxNavigationService navigation, SharedCounter counter)
    : WindowViewModel(navigation, counter);

public class WindowModalViewModel(IMvxNavigationService navigation, SharedCounter counter)
    : WindowViewModel(navigation, counter);
