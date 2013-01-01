// MvxPhoneViewDispatcherProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.Views;
using Microsoft.Phone.Controls;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public class MvxPhoneViewDispatcherProvider
        : IMvxViewDispatcherProvider
    {
        private readonly PhoneApplicationFrame _rootFrame;

        public MvxPhoneViewDispatcherProvider(PhoneApplicationFrame frame)
        {
            _rootFrame = frame;
        }

        public IMvxViewDispatcher Dispatcher
        {
            get { return new MvxPhoneViewDispatcher(_rootFrame); }
        }
    }
}