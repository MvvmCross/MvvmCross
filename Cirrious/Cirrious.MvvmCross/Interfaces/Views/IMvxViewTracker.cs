#region Copyright

// <copyright file="IMvxViewTracker.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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