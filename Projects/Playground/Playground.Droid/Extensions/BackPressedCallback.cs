using AndroidX.Activity;

namespace Playground.Droid.Extensions;

public sealed class BackPressedCallback(
        bool enabled,
        Action onBackPressedAction)
    : OnBackPressedCallback(enabled)
{
    private readonly Action _onBackPressedAction = onBackPressedAction;

    public override void HandleOnBackPressed() => _onBackPressedAction.Invoke();
}
