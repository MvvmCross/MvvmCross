#region Copyright
// <copyright file="IMvxMainThreadDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using System;

#endregion

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxMainThreadDispatcher
    {
        bool RequestMainThreadAction(Action action);
    }
}