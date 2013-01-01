// MvxSimpleBindingActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Binding.Droid.Simple
{
    public class MvxSimpleBindingActivity<TViewModel>
        : MvxBindingActivityView<MvxNullViewModel>
    {
        public new TViewModel ViewModel { get; set; }

        public override object DefaultBindingSource
        {
            get { return ViewModel; }
        }

        protected override sealed void OnViewModelSet()
        {
            // ignored  here
        }
    }
}