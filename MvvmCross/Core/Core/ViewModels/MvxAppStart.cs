// MvxAppStart.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading.Tasks;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Core.ViewModels
{
    [Obsolete("Please use MvxNavigationServiceAppStart instead")]
    public class MvxAppStart<TViewModel>
        : MvxNavigatingObject, IMvxAppStart
        where TViewModel : IMvxViewModel
    {
        public async Task Start(object hint = null)
        {
            if (hint != null)
            {
                MvxTrace.Trace("Hint ignored in default MvxAppStart");
            }
            ShowViewModel<TViewModel>();
        }
    }
}