// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.UnitTest.Mocks.Dispatchers
{
    public class NavigationMockDispatcher
        : IMvxViewDispatcher
    {
        public readonly List<MvxViewModelRequest> Requests = [];
        public readonly List<MvxPresentationHint> Hints = [];

        public bool IsOnMainThread => true;

        public bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception)
            {
                if (!maskExceptions)
                    throw;

                return false;
            }
        }

        public Task<bool> ShowViewModel(MvxViewModelRequest request)
        {
            Requests.Add(request);
            return Task.FromResult(true);
        }

        public Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            Hints.Add(hint);
            return Task.FromResult(true);
        }

        public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true)
        {
            return Task.Run(() =>
            {
                try
                {
                    action();
                }
                catch (Exception)
                {
                    if (!maskExceptions)
                        throw;
                }
            });
        }

        public async Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true)
        {
            try
            {
                await action();
            }
            catch (Exception)
            {
                if (!maskExceptions)
                    throw;
            }
        }
    }
}
