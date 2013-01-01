// IMvxViewDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;

#endregion

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxViewDispatcher : IMvxMainThreadDispatcher
    {
        bool RequestNavigate(MvxShowViewModelRequest request);
        bool RequestClose(IMvxViewModel whichViewModel);
        bool RequestRemoveBackStep();
    }
}