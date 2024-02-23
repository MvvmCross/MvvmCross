// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.WeakSubscription;
using SearchView = AndroidX.AppCompat.Widget.SearchView;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxAppCompatSearchViewQueryTextTargetBinding
    : MvxAndroidTargetBinding
{
    private MvxAndroidTargetEventSubscription<SearchView, SearchView.QueryTextChangeEventArgs>? _subscription;

    public MvxAppCompatSearchViewQueryTextTargetBinding(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
            SearchView target)
        : base(target)
    {
    }

    public override Type TargetValueType => typeof(string);

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    protected SearchView? SearchView => (SearchView?)Target;

    public override void SubscribeToEvents()
    {
        _subscription = SearchView?.WeakSubscribe<SearchView, SearchView.QueryTextChangeEventArgs>(
            nameof(SearchView.QueryTextChange),
            HandleQueryTextChanged);
    }

    protected override void SetValueImpl(object target, object? value)
    {
        if (target is SearchView searchView)
            searchView.SetQuery((string?)value, true);
    }

    private void HandleQueryTextChanged(object? sender, SearchView.QueryTextChangeEventArgs e)
    {
        if (Target is not SearchView searchView)
            return;

        var value = searchView.Query;
        FireValueChanged(value);
    }

    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _subscription?.Dispose();
            _subscription = null;
        }

        base.Dispose(isDisposing);
    }
}
