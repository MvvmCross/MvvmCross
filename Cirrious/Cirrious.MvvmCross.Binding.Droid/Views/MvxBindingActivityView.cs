// MvxBindingActivityView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public partial class MvxBindingActivityView<TViewModel>
        : MvxActivityView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
    }
}