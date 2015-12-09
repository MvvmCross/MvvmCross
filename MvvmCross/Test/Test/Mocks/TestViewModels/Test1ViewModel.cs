// Test1ViewModel.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;
using System;

namespace Cirrious.MvvmCross.Test.Mocks.TestViewModels
{
    public class Test1ViewModel : MvxViewModel
    {
        public ITestThing Thing { get; private set; }
        public IMvxBundle BundleInit { get; private set; }
        public IMvxBundle BundleState { get; private set; }
        public bool StartCalled { get; private set; }
        public string TheInitString1Set { get; private set; }
        public Guid TheInitGuid1Set { get; private set; }
        public Guid TheInitGuid2Set { get; private set; }
        public BundleObject TheInitBundleSet { get; private set; }
        public string TheReloadString1Set { get; private set; }
        public Guid TheReloadGuid1Set { get; private set; }
        public Guid TheReloadGuid2Set { get; private set; }
        public BundleObject TheReloadBundleSet { get; private set; }

        public Test1ViewModel(ITestThing thing)
        {
            Thing = thing;
        }

        public void Init(string TheString1)
        {
            TheInitString1Set = TheString1;
        }

        public void Init(Guid TheGuid1, Guid TheGuid2)
        {
            TheInitGuid1Set = TheGuid1;
            TheInitGuid2Set = TheGuid2;
        }

        public void Init(BundleObject bundle)
        {
            TheInitBundleSet = bundle;
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            BundleInit = parameters;
        }

        public void ReloadState(string TheString1)
        {
            TheReloadString1Set = TheString1;
        }

        public void ReloadState(Guid TheGuid1, Guid TheGuid2)
        {
            TheReloadGuid1Set = TheGuid1;
            TheReloadGuid2Set = TheGuid2;
        }

        public void ReloadState(BundleObject bundle)
        {
            TheReloadBundleSet = bundle;
        }

        protected override void ReloadFromBundle(IMvxBundle state)
        {
            BundleState = state;
        }

        public override void Start()
        {
            StartCalled = true;
        }
    }
}