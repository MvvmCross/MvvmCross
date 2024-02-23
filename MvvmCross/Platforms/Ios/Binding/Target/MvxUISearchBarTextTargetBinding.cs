// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Platforms.Ios.Binding.Target;

public class MvxUISearchBarTextTargetBinding(UISearchBar target, PropertyInfo targetPropertyInfo)
    : MvxPropertyInfoTargetBinding<UISearchBar>(target, targetPropertyInfo)
{
    private MvxWeakEventSubscription<UISearchBar, UISearchBarTextChangedEventArgs>? _subscription;

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    public override void SubscribeToEvents()
    {
        var searchBar = View;
        if (searchBar == null)
        {
            MvxBindingLog.Instance?.LogError(
                "UISearchBar is null in {TargetBindingName}", nameof(MvxUISearchBarTextTargetBinding));
            return;
        }

        _subscription =
            searchBar.WeakSubscribe<UISearchBar, UISearchBarTextChangedEventArgs>(nameof(searchBar.TextChanged),
                HandleSearchBarValueChanged);
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);
        if (!isDisposing) return;

        _subscription?.Dispose();
        _subscription = null;
    }

    private void HandleSearchBarValueChanged(object? sender, UISearchBarTextChangedEventArgs e)
    {
        FireValueChanged(View?.Text);
    }
}
