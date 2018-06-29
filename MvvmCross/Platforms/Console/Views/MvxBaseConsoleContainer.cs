// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Console.Views
{
    public abstract class MvxBaseConsoleContainer
        : MvxViewsContainer, IMvxConsoleNavigation
    {
        private readonly Dictionary<Type, Func<MvxPresentationHint, Task<bool>>> _presentationHintHandlers = new Dictionary<Type, Func<MvxPresentationHint, Task<bool>>>();

        public void AddPresentationHintHandler<THint>(Func<THint, Task<bool>> action) where THint : MvxPresentationHint
        {
            _presentationHintHandlers[typeof(THint)] = hint => action((THint)hint);
        }

        protected Task<bool> HandlePresentationChange(MvxPresentationHint hint)
        {
            Func<MvxPresentationHint, Task<bool>> handler;

            if (_presentationHintHandlers.TryGetValue(hint.GetType(), out handler))
            {
                return handler(hint);
            }

            return Task.FromResult(false);
        }

        public abstract Task<bool> Show(MvxViewModelRequest request);

        public abstract Task<bool> GoBack();

        public abstract void RemoveBackEntry();

        public abstract bool CanGoBack();

        public virtual async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (await HandlePresentationChange(hint)) return true;

            MvxLog.Instance.Warn("Hint ignored {0}", hint.GetType().Name);
            return false;
        }

        public abstract Task<bool> Close(IMvxViewModel toClose);
    }
}
