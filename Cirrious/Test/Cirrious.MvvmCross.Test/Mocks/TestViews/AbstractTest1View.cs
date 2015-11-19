// AbstractTest1View.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Test.Mocks.TestViews
{
    public abstract class AbstractTest1View : IMvxView
    {
        public object DataContext { get; set; }
        public IMvxViewModel ViewModel { get; set; }
    }
}