#region Copyright
// <copyright file="IMvxViewDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;

#endregion

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxViewDispatcher : IMvxMainThreadDispatcher
    {
        bool RequestNavigate(MvxShowViewModelRequest request);
        bool RequestClose(IMvxViewModel whichViewModel);
        bool RequestRemoveBackStep();
    }
}