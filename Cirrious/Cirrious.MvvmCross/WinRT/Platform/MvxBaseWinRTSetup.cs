#region Copyright
// <copyright file="MvxBaseWinRTSetup.cs" company="Cirrious">
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
using System.Reflection;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WinRT.Interfaces;
using Cirrious.MvvmCross.WinRT.Views;
using Windows.UI.Xaml.Controls;

namespace Cirrious.MvvmCross.WinRT.Platform
{

    public abstract class MvxBaseWinRTSetup 
        : MvxBaseSetup
    {
        private readonly Frame _rootFrame;

        protected MvxBaseWinRTSetup(Frame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        protected override MvxViewsContainer CreateViewsContainer()
        {
            var container = CreateViewsContainer(_rootFrame);;
            return container;
        }

        protected virtual MvxWinRTViewsContainer CreateViewsContainer(Frame rootFrame)
        {
            return new MvxWinRTViewsContainer(rootFrame);
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().GetTypeInfo().Assembly, typeof(IMvxWinRTView));
        }
    }
}