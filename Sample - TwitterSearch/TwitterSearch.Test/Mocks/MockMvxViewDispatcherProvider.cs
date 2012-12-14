// <copyright file="MockMvxViewDispatcherProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com - Hire me - I'm worth it!

using Cirrious.MvvmCross.Interfaces.Views;

namespace TwitterSearch.Test.Mocks
{
    public class MockMvxViewDispatcherProvider : IMvxViewDispatcherProvider
    {
        #region IMvxViewDispatcherProvider implementation

        public IMvxViewDispatcher Dispatcher { get; set; }

        #endregion
    }
}