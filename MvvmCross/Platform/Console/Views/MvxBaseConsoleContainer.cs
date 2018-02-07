// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Logging;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Platform.Console.Views
{
    public abstract class MvxBaseConsoleContainer
        : MvxViewsContainer, IMvxConsoleNavigation
    {
        private readonly Dictionary<Type, Func<MvxPresentationHint, bool>> _presentationHintHandlers = new Dictionary<Type, Func<MvxPresentationHint, bool>>();

        public void AddPresentationHintHandler<THint>(Func<THint, bool> action) where THint : MvxPresentationHint
        {
            _presentationHintHandlers[typeof(THint)] = hint => action((THint)hint);
        }

        protected bool HandlePresentationChange(MvxPresentationHint hint)
        {
            Func<MvxPresentationHint, bool> handler;

            if (_presentationHintHandlers.TryGetValue(hint.GetType(), out handler))
            {
                if (handler(hint)) return true;
            }

            return false;
        }

        public abstract void Show(MvxViewModelRequest request);

        public abstract void GoBack();

        public abstract void RemoveBackEntry();

        public abstract bool CanGoBack();

        public virtual void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            MvxLog.Instance.Warn("Hint ignored {0}", hint.GetType().Name);
        }

        public abstract void Close(IMvxViewModel toClose);
    }
}
