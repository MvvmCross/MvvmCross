// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Windows.Input;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Platforms.Ios.Binding.Target;

public class MvxUIBarButtonItemTargetBinding : MvxConvertingTargetBinding
{
    private readonly EventHandler<EventArgs> _canExecuteEventHandler;
    private ICommand? _command;
    private MvxWeakEventSubscription<UIBarButtonItem>? _clickSubscription;
    private MvxCanExecuteChangedEventSubscription? _canExecuteSubscription;

    protected UIBarButtonItem? Control => Target as UIBarButtonItem;

    public MvxUIBarButtonItemTargetBinding(UIBarButtonItem control)
        : base(control)
    {
        _clickSubscription = control.WeakSubscribe(nameof(control.Clicked), OnClicked);
        _canExecuteEventHandler = OnCanExecuteChanged;
    }

    public override Type TargetValueType => typeof(ICommand);

    protected override void SetValueImpl(object target, object? value)
    {
        if (_canExecuteSubscription != null)
        {
            _canExecuteSubscription.Dispose();
            _canExecuteSubscription = null;
        }
        _command = value as ICommand;
        if (_command != null)
        {
            _canExecuteSubscription = _command.WeakSubscribe(_canExecuteEventHandler);
        }
        RefreshEnabledState();
    }

    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _clickSubscription?.Dispose();
            _canExecuteSubscription?.Dispose();
            _canExecuteSubscription = null;
            _clickSubscription = null;
        }

        base.Dispose(isDisposing);
    }

    private void OnClicked(object? sender, EventArgs e)
    {
        if (_command == null)
            return;

        if (!_command.CanExecute(null))
            return;

        _command.Execute(null);
    }

    private void OnCanExecuteChanged(object? sender, EventArgs e)
    {
        RefreshEnabledState();
    }

    private void RefreshEnabledState()
    {
        var view = Control;
        if (view == null)
            return;

        var shouldBeEnabled = false;
        if (_command != null)
        {
            shouldBeEnabled = _command.CanExecute(null);
        }
        view.Enabled = shouldBeEnabled;
    }
}
