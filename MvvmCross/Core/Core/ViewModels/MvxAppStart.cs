// MvxAppStart.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using MvvmCross.Platform.Platform;

    public class MvxAppStart<TViewModel>
        : MvxNavigatingObject
          , IMvxAppStart
        where TViewModel : IMvxViewModel
    {
        public void Start(object hint = null)
        {
            if (hint != null)
            {
                MvxTrace.Trace("Hint ignored in default MvxAppStart");
            }
            this.ShowViewModel<TViewModel>();
        }
    }
}