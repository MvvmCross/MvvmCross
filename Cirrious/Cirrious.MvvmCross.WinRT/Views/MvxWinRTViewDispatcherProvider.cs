// MvxWinRTViewDispatcherProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.WinRT.Interfaces;
using Windows.UI.Xaml.Controls;

namespace Cirrious.MvvmCross.WinRT.Views
{
    public class MvxWinRTViewDispatcherProvider
        : IMvxViewDispatcherProvider
    {
        private readonly IMvxWinRTViewPresenter _presenter;
        private readonly Frame _rootFrame;

        public MvxWinRTViewDispatcherProvider(IMvxWinRTViewPresenter presenter, Frame frame)
        {
            _presenter = presenter;
            _rootFrame = frame;
        }

        public IMvxViewDispatcher Dispatcher
        {
            get { return new MvxWinRTViewDispatcher(_presenter, _rootFrame); }
        }
    }
}