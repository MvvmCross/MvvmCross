// IMvxViewTracker.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxViewTracker
    {
        void RegisterView(IMvxView view);
        void UnRegisterView(IMvxView view);
        void ActOnRegisteredViews(Action<IMvxView> action);
    }
}