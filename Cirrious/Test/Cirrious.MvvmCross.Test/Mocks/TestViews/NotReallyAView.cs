// NotReallyAView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Test.Mocks.TestViews
{
    public class NotReallyAView
    {
        public object DataContext { get; set; }
        public IMvxViewModel ViewModel { get; set; }
    }
}