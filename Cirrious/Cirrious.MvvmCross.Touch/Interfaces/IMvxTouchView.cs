// IMvxTouchView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.Interfaces
{
    public interface IMvxTouchView
        : IMvxView
		, IMvxServiceConsumer
	{
		MvxShowViewModelRequest ShowRequest { get; set; }
    }

    public interface IMvxTouchView<TViewModel>
        : IMvxView<TViewModel>
        , IMvxTouchView
        where TViewModel : class, IMvxViewModel
    {
		new TViewModel ViewModel { get; set; }
	}
}