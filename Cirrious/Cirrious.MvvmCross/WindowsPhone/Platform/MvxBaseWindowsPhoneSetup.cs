#region Copyright
// <copyright file="MvxBaseWindowsPhoneSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;

namespace Cirrious.MvvmCross.WindowsPhone.Platform
{
    public abstract class MvxBaseWindowsPhoneSetup 
        : MvxBaseSetup        
        , IMvxServiceProducer<IMvxWindowsPhoneViewModelRequestTranslator>
    {
        private readonly PhoneApplicationFrame _rootFrame;

        protected MvxBaseWindowsPhoneSetup(PhoneApplicationFrame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        protected override MvxViewsContainer CreateViewsContainer()
        {
            var container = CreateViewsContainer(_rootFrame);
            this.RegisterServiceInstance<IMvxWindowsPhoneViewModelRequestTranslator>(container);
            return container;
        }

        protected override IMvxViewDispatcherProvider CreateViewDispatcherProvider()
        {
            return CreateViewDispatcherProvider(_rootFrame);
        }

        protected virtual IMvxViewDispatcherProvider CreateViewDispatcherProvider(PhoneApplicationFrame rootFrame)
        {
            return new MvxPhoneViewDispatcherProvider(rootFrame);
        }

        protected virtual MvxPhoneViewsContainer CreateViewsContainer(PhoneApplicationFrame rootFrame)
        {
            return new MvxPhoneViewsContainer(rootFrame);
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().Assembly, typeof(IMvxWindowsPhoneView));
        }
    }
}