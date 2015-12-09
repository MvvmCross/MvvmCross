// Test4ViewModel.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;
using System;

namespace Cirrious.MvvmCross.Test.Mocks.TestViewModels
{
    public class Test4ViewModel : MvxViewModel
    {
        public ITestThing Thing { get; private set; }

        public Test4ViewModel(ITestThing thing)
        {
            Thing = thing;
        }

        public override void Start()
        {
            throw new NullReferenceException();
        }
    }
}