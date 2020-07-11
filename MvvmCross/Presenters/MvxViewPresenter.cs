// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters
{
    public abstract class MvxViewPresenter : IMvxViewPresenter
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

        public abstract ValueTask<bool> ChangePresentation(MvxPresentationHint hint);

        public abstract ValueTask<bool> Close(IMvxViewModel viewModel);
    }
}
