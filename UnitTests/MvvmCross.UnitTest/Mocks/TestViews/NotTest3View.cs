// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.UnitTest.Mocks.TestViewModels;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.UnitTest.Mocks.TestViews
{
    [MvxViewFor(typeof(Test3ViewModel))]
    public class NotTest3View : IMvxView
    {
        public object DataContext { get; set; }
        public IMvxViewModel ViewModel { get; set; }
    }
}
