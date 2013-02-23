// MvxSimpleBindingActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Droid.Simple
{
    public class MvxSimpleBindingActivity
        : MvxActivityView
    {
        public new IMvxViewModel ViewModel
        {
            get { return base.ViewModel; }
            set
            {
                throw new MvxException(
                    "You've chosen to use simple binding.... so you need to just use DataContext, not ViewModel");
            }
        }

        protected override sealed void OnViewModelSet()
        {
            // ignored  here
        }
    }
}