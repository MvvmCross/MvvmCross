// MvxSimpleStartApplicationObject.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxSimpleStartApplicationObject<TViewModel>
        : MvxApplicationObject
          , IMvxStartNavigation
        where TViewModel : IMvxViewModel
    {
        public void Start()
        {
            RequestNavigate<TViewModel>();
        }

        public bool ApplicationCanOpenBookmarks
        {
            get { return false; }
        }
    }
}