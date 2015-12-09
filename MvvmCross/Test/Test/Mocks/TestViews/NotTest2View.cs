// NotTest2View.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Test.Mocks.TestViews
{
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Test.Mocks.TestViewModels;

    public class NotTest2View : IMvxView
    {
        public object DataContext { get; set; }
        IMvxViewModel IMvxView.ViewModel { get; set; }
        public Test2ViewModel ViewModel { get; set; }
    }
}