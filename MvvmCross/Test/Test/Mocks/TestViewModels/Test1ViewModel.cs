// Test1ViewModel.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Test.Mocks.TestViewModels
{
    using System;

    using MvvmCross.Core.ViewModels;

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
            this.Thing = thing;
        }

        public void Init(string TheString1)
        {
            this.TheInitString1Set = TheString1;
        }

        public void Init(Guid TheGuid1, Guid TheGuid2)
        {
            this.TheInitGuid1Set = TheGuid1;
            this.TheInitGuid2Set = TheGuid2;
        }

        public void Init(BundleObject bundle)
        {
            this.TheInitBundleSet = bundle;
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            this.BundleInit = parameters;
        }

        public void ReloadState(string TheString1)
        {
            this.TheReloadString1Set = TheString1;
        }

        public void ReloadState(Guid TheGuid1, Guid TheGuid2)
        {
            this.TheReloadGuid1Set = TheGuid1;
            this.TheReloadGuid2Set = TheGuid2;
        }

        public void ReloadState(BundleObject bundle)
        {
            this.TheReloadBundleSet = bundle;
        }

        protected override void ReloadFromBundle(IMvxBundle state)
        {
            this.BundleState = state;
        }

        public override void Start()
        {
            this.StartCalled = true;
        }
    }
}