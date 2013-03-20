// MvxAppStart.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxAppStart<TViewModel>
        : MvxNavigatingObject
        , IMvxAppStart
        where TViewModel : IMvxViewModel
    {
        public void Start(object hint = null)
        {
            RequestNavigate<TViewModel>();
        }
    }
}