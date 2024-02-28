#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.WeakSubscription;
using static Android.Views.View;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxViewFocusChangedTargetBinding : MvxAndroidTargetBinding
{
    private ICommand? _command;
    private MvxWeakEventSubscription<View, FocusChangeEventArgs>? _focusChangeSubscription;

    public MvxViewFocusChangedTargetBinding(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
        View target) : base(target)
    {
        _focusChangeSubscription = target.WeakSubscribe<View, FocusChangeEventArgs>(
            nameof(target.FocusChange), ViewOnFocusChange);
    }

    private void ViewOnFocusChange(object? sender, FocusChangeEventArgs e)
    {
        if (_command == null)
            return;

        if (!_command.CanExecute(e.HasFocus))
            return;

        _command.Execute(e.HasFocus);
    }

    public override Type TargetValueType => typeof(ICommand);

    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    protected override void SetValueImpl(object target, object? value)
    {
        _command = value as ICommand;
    }

    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _focusChangeSubscription?.Dispose();
            _focusChangeSubscription = null;
        }

        base.Dispose(isDisposing);
    }
}
