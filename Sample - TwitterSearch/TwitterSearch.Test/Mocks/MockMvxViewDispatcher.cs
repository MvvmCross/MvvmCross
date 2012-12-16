// <copyright file="MockMvxViewDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com - Hire me - I'm worth it!

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;

namespace TwitterSearch.Test.Mocks
{
    public class MockMvxViewDispatcher : IMvxViewDispatcher
    {
        public List<IMvxViewModel> CloseRequests = new List<IMvxViewModel>();
        public List<MvxShowViewModelRequest> NavigateRequests = new List<MvxShowViewModelRequest>();

        #region IMvxViewDispatcher implementation

        public bool RequestNavigate(MvxShowViewModelRequest request)
        {
            NavigateRequests.Add(request);
            return true;
        }

        public bool RequestClose(IMvxViewModel whichViewModel)
        {
            CloseRequests.Add(whichViewModel);
            return true;
        }

        public bool RequestRemoveBackStep()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMvxMainThreadDispatcher implementation

        public bool RequestMainThreadAction(Action action)
        {
            action();
            return true;
        }

        #endregion
    }
}