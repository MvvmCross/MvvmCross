// MvxPhoneViewDispatcherProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Views;
using Microsoft.Phone.Controls;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public class MvxPhoneViewDispatcherProvider
        : IMvxViewDispatcherProvider
    {
        private readonly IMvxPhoneViewPresenter _presenter;
        private readonly PhoneApplicationFrame _rootFrame;

        public MvxPhoneViewDispatcherProvider(IMvxPhoneViewPresenter presenter, PhoneApplicationFrame frame)
        {
            _presenter = presenter;
            _rootFrame = frame;
        }

        public IMvxViewDispatcher ViewDispatcher
        {
            get { return new MvxPhoneViewDispatcher(_presenter, _rootFrame); }
        }
    }
}