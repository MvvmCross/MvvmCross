#nullable enable

using System.Diagnostics.CodeAnalysis;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Navigation;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels.MultiWindow;
using UIKit;

namespace Playground.iOS.MultiWindow;

[Register("MultiWindowSceneDelegate")]
[RequiresUnreferencedCode("The demo uses MvvmCross navigation with presentation attributes and view type lookups.")]
public class MultiWindowSceneDelegate : MvxSceneDelegate
{
    private CancellationTokenSource? _connection;
    private MvxIosMultiWindowViewPresenter? _presenter;

    [SuppressMessage("AsyncFixer", "AsyncFixer03", Justification = "UIKit requires a void callback; asynchronous navigation exceptions are handled here.")]
    public override async void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
    {
        if (scene is not UIWindowScene windowScene)
            return;

        var services = ((MultiWindowAppDelegate)UIApplication.SharedApplication.Delegate).Host.Services;
        _presenter = services.GetRequiredService<MvxIosMultiWindowViewPresenter>();
        _connection = new CancellationTokenSource();
        var token = _connection.Token;
        var window = new UIWindow(windowScene);
        Window = window;
        _presenter.RegisterWindow(session.PersistentIdentifier, window);

        try
        {
            var navigation = services.GetRequiredService<IMvxNavigationService>();
            var context = new WindowContext(session.PersistentIdentifier, token);
            var shown = await navigation.Navigate<WindowViewModel, WindowContext>(context,
                new MvxWindowPresentationBundle(context.WindowId), token);
            if (shown && !token.IsCancellationRequested)
            {
                window.MakeKeyAndVisible();
                return;
            }
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
            // UIKit disconnected this scene before initial navigation completed.
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

        if (!token.IsCancellationRequested)
            ReleaseWindow(session.PersistentIdentifier);
    }

    public override void DidDisconnect(UIScene scene)
    {
        ReleaseWindow(scene.Session.PersistentIdentifier);
        base.DidDisconnect(scene);
    }

    private void ReleaseWindow(string windowId)
    {
        _connection?.Cancel();
        _connection?.Dispose();
        _connection = null;
        _presenter?.UnregisterWindow(windowId);
        Window = null;
    }
}
