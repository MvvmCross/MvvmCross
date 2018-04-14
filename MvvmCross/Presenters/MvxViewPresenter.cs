// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters
{
    public abstract class MvxViewPresenter : IMvxViewPresenter
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

        public abstract void ChangePresentation(MvxPresentationHint hint);

        public abstract void Close(IMvxViewModel viewModel);
    }
}
