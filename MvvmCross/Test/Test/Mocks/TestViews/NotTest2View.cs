// NotTest2View.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Test.Mocks.TestViewModels;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Test.Mocks.TestViews
{
    public class NotTest2View : IMvxView
    {
        public object DataContext { get; set; }
        IMvxViewModel IMvxView.ViewModel { get; set; }
        public Test2ViewModel ViewModel { get; set; }
    }
}