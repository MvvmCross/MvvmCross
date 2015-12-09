// Test4ViewModel.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Test.Mocks.TestViewModels
{
    using System;

    using MvvmCross.Core.ViewModels;

    public class Test4ViewModel : MvxViewModel
    {
        public ITestThing Thing { get; private set; }

        public Test4ViewModel(ITestThing thing)
        {
            this.Thing = thing;
        }

        public override void Start()
        {
            throw new NullReferenceException();
        }
    }
}