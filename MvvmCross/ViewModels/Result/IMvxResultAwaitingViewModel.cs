// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.ViewModels.Result;

/// <summary>
/// Implementing this interface requires to manually register and unregister ViewModel to result:
/// <code>
/// protected override void ReloadFromBundle(IMvxBundle state)
/// {
///     base.ReloadFromBundle(state);
///     this.ReloadAndRegisterToResult&lt;<typeparamref name="TResult"/>&gt;(state, Mvx.IoCProvider.Resolve&lt;IMvxResultViewModelManager&gt;());
/// }
/// protected override void SaveStateToBundle(IMvxBundle bundle)
/// {
///     base.SaveStateToBundle(bundle);
///     this.SaveRegisterToResult&lt;<typeparamref name="TResult"/>&gt;(bundle, Mvx.IoCProvider.Resolve&lt;IMvxResultViewModelManager&gt;());
/// }
/// public override ViewDestroy(bool viewFinishing = true)
/// {
///     base.ViewDestroy();
///     if (viewFinishing)
///         this.UnregisterToResult&lt;<typeparamref name="TResult"/>&gt;(Mvx.IoCProvider.Resolve&lt;IMvxResultViewModelManager&gt;());
/// }
/// </code>
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IMvxResultAwaitingViewModel<TResult> : IMvxBaseResultAwaitingViewModel
{
    bool ResultSet(IMvxResultSettingViewModel<TResult> viewModel, TResult result);
}

public interface IMvxBaseResultAwaitingViewModel
{
}
