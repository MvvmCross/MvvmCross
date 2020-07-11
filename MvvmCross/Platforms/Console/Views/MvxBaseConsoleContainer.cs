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
        private readonly Dictionary<Type, Func<MvxPresentationHint, ValueTask<bool>>> _presentationHintHandlers = new Dictionary<Type, Func<MvxPresentationHint, ValueTask<bool>>>();

        public void AddPresentationHintHandler<THint>(Func<THint, ValueTask<bool>> action) where THint : MvxPresentationHint
        {
            _presentationHintHandlers[typeof(THint)] = hint => action((THint)hint);
        }

        protected ValueTask<bool> HandlePresentationChange(MvxPresentationHint hint)
        {
            Func<MvxPresentationHint, ValueTask<bool>> handler;

            if (_presentationHintHandlers.TryGetValue(hint.GetType(), out handler))
            {
                return handler(hint);
            }

            return new ValueTask<bool>(false);
        }

        public abstract ValueTask<bool> Show(MvxViewModelRequest request);

        public abstract ValueTask<bool> GoBack();

        public abstract void RemoveBackEntry();

        public abstract bool CanGoBack();

        public virtual async ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (await HandlePresentationChange(hint).ConfigureAwait(false)) return true;

            MvxLog.Instance.Warn("Hint ignored {0}", hint.GetType().Name);
            return false;
        }

        public abstract ValueTask<bool> Close(IMvxViewModel toClose);
    }
}
