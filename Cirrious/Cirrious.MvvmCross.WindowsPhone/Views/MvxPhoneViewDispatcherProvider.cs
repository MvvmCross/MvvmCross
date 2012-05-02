#region Copyright
// <copyright file="MvxPhoneViewDispatcherProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

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