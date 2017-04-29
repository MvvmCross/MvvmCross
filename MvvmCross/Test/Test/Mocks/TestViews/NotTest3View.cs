// NotTest3View.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Test.Mocks.TestViewModels;

namespace MvvmCross.Test.Mocks.TestViews
{
    [MvxViewFor(typeof(Test3ViewModel))]
    public class NotTest3View : IMvxView
    {
        public object DataContext { get; set; }
        public IMvxViewModel ViewModel { get; set; }
    }
}