#nullable enable

using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using Playground.Core.ViewModels.MultiWindow;
using UIKit;

namespace Playground.iOS.Views.MultiWindow;

public abstract class WindowView<TViewModel> : MvxViewController<TViewModel> where TViewModel : WindowViewModel
{
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        Title = GetType().Name;
        View!.BackgroundColor = UIColor.SystemBackground;
        var identity = new UILabel { Lines = 0, Text = ViewModel.WindowId };
        var count = new UILabel();
        var increment = Button("Increment shared counter");
        var child = Button("Push in this window");
        var modal = Button("Modal in this window");
        var delayed = Button("Push after 3 seconds");
        var close = Button("Close this view");
        var newWindow = Button("New window");
        newWindow.TouchUpInside += (_, _) =>
        {
            using var options = new UISceneActivationRequestOptions { RequestingScene = View.Window?.WindowScene };
            if (OperatingSystem.IsIOSVersionAtLeast(17) || OperatingSystem.IsMacCatalystVersionAtLeast(17))
            {
                using var request = UISceneSessionActivationRequest.Create();
                request.Options = options;
                UIApplication.SharedApplication.ActivateSceneSession(request,
                    error => Console.WriteLine(error.LocalizedDescription));
            }
            else
            {
                UIApplication.SharedApplication.RequestSceneSessionActivation(null, null, options,
                    error => Console.WriteLine(error.LocalizedDescription));
            }
        };
        newWindow.Enabled = UIApplication.SharedApplication.SupportsMultipleScenes;
        var closeWindow = Button("Close this window");
        closeWindow.TouchUpInside += (_, _) =>
        {
            if (View.Window?.WindowScene is { } scene)
                UIApplication.SharedApplication.RequestSceneSessionDestruction(scene.Session, null,
                    error => Console.WriteLine(error.LocalizedDescription));
        };
        closeWindow.Enabled = UIApplication.SharedApplication.SupportsMultipleScenes;
        close.Hidden = typeof(TViewModel) == typeof(WindowViewModel);

        var stack = new UIStackView([identity, count, increment, child, modal, delayed, close, newWindow, closeWindow])
        {
            Axis = UILayoutConstraintAxis.Vertical,
            Spacing = 12,
            TranslatesAutoresizingMaskIntoConstraints = false
        };
        View.AddSubview(stack);
        var safe = View.SafeAreaLayoutGuide;
        NSLayoutConstraint.ActivateConstraints([
            stack.TopAnchor.ConstraintEqualTo(safe.TopAnchor, 20),
            stack.LeadingAnchor.ConstraintEqualTo(safe.LeadingAnchor, 20),
            stack.TrailingAnchor.ConstraintEqualTo(safe.TrailingAnchor, -20)
        ]);

        using var bindings = CreateBindingSet();
        bindings.Bind(count).To(vm => vm.Counter.Count);
        bindings.Bind(increment).To(vm => vm.Increment);
        bindings.Bind(child).To(vm => vm.ShowChild);
        bindings.Bind(modal).To(vm => vm.ShowModal);
        bindings.Bind(delayed).To(vm => vm.ShowDelayedChild);
        bindings.Bind(close).To(vm => vm.Close);
    }

    private static UIButton Button(string title)
    {
        var button = new UIButton(UIButtonType.System);
        button.SetTitle(title, UIControlState.Normal);
        return button;
    }
}

[MvxRootPresentation(WrapInNavigationController = true)]
public class WindowView : WindowView<WindowViewModel>;

[MvxChildPresentation(Animated = false)]
public class WindowChildView : WindowView<WindowChildViewModel>;

[MvxModalPresentation(WrapInNavigationController = true, Animated = false)]
public class WindowModalView : WindowView<WindowModalViewModel>;
