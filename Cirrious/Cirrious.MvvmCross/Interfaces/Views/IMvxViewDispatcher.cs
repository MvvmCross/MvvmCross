// IMvxViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxViewDispatcher : IMvxMainThreadDispatcher
    {
        bool RequestNavigate(MvxShowViewModelRequest request);

        [Obsolete("RequestClose doesn't really work on all platforms and in all scenarios - you may be better off using a custom Message and a Messenger")]
        bool RequestClose(IMvxViewModel whichViewModel);
        [Obsolete("RequestRemoveBackStep doesn't really work on all platforms and in all scenarios - you may be better off using a custom Message and a Messenger")]
        bool RequestRemoveBackStep();
    }
}