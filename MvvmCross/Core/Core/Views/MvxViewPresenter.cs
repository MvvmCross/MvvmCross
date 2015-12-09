// MvxViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.Views
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Core.ViewModels;

    public abstract class MvxViewPresenter : IMvxViewPresenter
    {
        private readonly Dictionary<Type, Func<MvxPresentationHint, bool>> _presentationHintHandlers = new Dictionary<Type, Func<MvxPresentationHint, bool>>();

        public void AddPresentationHintHandler<THint>(Func<THint, bool> action) where THint : MvxPresentationHint
        {
            this._presentationHintHandlers[typeof(THint)] = hint => action((THint)hint);
        }

        protected bool HandlePresentationChange(MvxPresentationHint hint)
        {
            Func<MvxPresentationHint, bool> handler;

            if (this._presentationHintHandlers.TryGetValue(hint.GetType(), out handler))
            {
                if (handler(hint)) return true;
            }

            return false;
        }

        public abstract void Show(MvxViewModelRequest request);

        public abstract void ChangePresentation(MvxPresentationHint hint);
    }
}