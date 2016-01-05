// NotReallyAView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Test.Mocks.TestViews
{
    using MvvmCross.Core.ViewModels;

    public class NotReallyAView
    {
        public object DataContext { get; set; }
        public IMvxViewModel ViewModel { get; set; }
    }
}