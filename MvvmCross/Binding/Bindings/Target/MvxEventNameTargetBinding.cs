// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Binding.Bindings.Target;

public class MvxEventNameTargetBinding<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TTarget> : MvxTargetBinding
    where TTarget : class
{
    private readonly bool _useEventArgsAsCommandParameter;
    private readonly IDisposable _eventSubscription;

    private ICommand? _currentCommand;

    public MvxEventNameTargetBinding(
        TTarget target, string targetEventName, bool useEventArgsAsCommandParameter = true)
        : base(target)
    {
        _useEventArgsAsCommandParameter = useEventArgsAsCommandParameter;
        _eventSubscription = target.WeakSubscribe(targetEventName, HandleEvent);
    }

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public override Type TargetValueType { get; } = typeof(ICommand);

    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
            _eventSubscription.Dispose();

        base.Dispose(isDisposing);
    }

    private void HandleEvent(object? sender, EventArgs parameter)
    {
        var commandParameter = _useEventArgsAsCommandParameter ? (object)parameter : null;
        if (_currentCommand?.CanExecute(commandParameter) == true)
            _currentCommand.Execute(commandParameter);
    }

    public override void SetValue(object? value) => _currentCommand = value as ICommand;
}
