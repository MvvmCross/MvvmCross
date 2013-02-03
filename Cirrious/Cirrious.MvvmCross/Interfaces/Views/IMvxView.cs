// IMvxView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxView
    {
#warning IsVisible should go?
		[Obsolete("IsVisible should be removed from the base view")]
        bool IsVisible { get; }
		IMvxViewModel ViewModel { get; set; }
    }

	public interface IMvxOldSkoolGenericView
	{
	}

    public interface IMvxView<TViewModel>
        : IMvxView
		, IMvxOldSkoolGenericView
        where TViewModel : class, IMvxViewModel
    {
        new TViewModel ViewModel { get; set; }
    }
}