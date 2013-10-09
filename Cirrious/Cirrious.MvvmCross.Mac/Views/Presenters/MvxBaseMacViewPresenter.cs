// MvxBaseMacViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using MonoMac.AppKit;

namespace Cirrious.MvvmCross.Mac.Views.Presenters
{
    public class MvxBaseMacViewPresenter
        : IMvxMacViewPresenter
    {
        public virtual void Show(MvxViewModelRequest view)
        {
        }

        public virtual void ChangePresentation(MvxPresentationHint hint)
        {
            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }
	}
}