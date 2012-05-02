#region Copyright
// <copyright file="IMvxTouchView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.Interfaces
{
    public interface IMvxTouchView
		: IMvxView
	{
        MvxShowViewModelRequest ShowRequest { get; }
    }

    public interface IMvxTouchView<TViewModel>
        : IMvxView<TViewModel>
        , IMvxTouchView
        , IMvxServiceConsumer<IMvxViewModelLoader>
        where TViewModel : class, IMvxViewModel
    {
    }
}