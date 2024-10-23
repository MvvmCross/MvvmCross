// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.ViewModels.Result;

public class MvxResultViewModelManager : IMvxResultViewModelManager
{
    private readonly Dictionary<Type, HashSet<IMvxViewModel>> _registrations = new();

    public void RegisterToResult<TResult>(IMvxResultAwaitingViewModel<TResult> viewModel)
    {
        if (!_registrations.TryGetValue(typeof(TResult), out var resultRegistrations))
        {
            resultRegistrations = new();
            _registrations[typeof(TResult)] = resultRegistrations;
        }

        resultRegistrations.Add(viewModel);
    }

    public bool UnregisterToResult<TResult>(IMvxResultAwaitingViewModel<TResult> viewModel)
    {
        if (_registrations.TryGetValue(typeof(TResult), out var resultRegistrations) &&
            resultRegistrations.Remove(viewModel))
        {
            return true;
        }
        return false;
    }

    public bool IsRegistered<TResult>(IMvxResultAwaitingViewModel<TResult> viewModel)
    {
        if (_registrations.TryGetValue(typeof(TResult), out var resultRegistrations) &&
            resultRegistrations.Contains(viewModel))
        {
            return true;
        }
        return false;
    }

    public void SetResult<TResult>(IMvxResultSettingViewModel<TResult> viewModel, TResult result)
    {
        if (_registrations.TryGetValue(typeof(TResult), out var resultRegistrations))
        {
            foreach (var resultRegistration in resultRegistrations.Cast<IMvxResultAwaitingViewModel<TResult>>().ToArray())
            {
                if (resultRegistration.ResultSet(viewModel, result))
                {
                    resultRegistrations.Remove(resultRegistration);
                }
            }
        }
    }
}
