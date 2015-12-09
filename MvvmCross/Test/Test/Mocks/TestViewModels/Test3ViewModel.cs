// Test3ViewModel.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Test.Mocks.TestViewModels
{
    using System.Collections.Generic;

    using MvvmCross.Core.ViewModels;

    public class Test3ViewModel : MvxViewModel
    {
        public BundleObject SaveStateBundleObject { get; set; }
        public Dictionary<string, string> AdditionalSaveStateFields { get; set; }

        public BundleObject SaveState()
        {
            return this.SaveStateBundleObject;
        }

        protected override void SaveStateToBundle(IMvxBundle bundle)
        {
            if (this.AdditionalSaveStateFields != null)
            {
                foreach (var kvp in this.AdditionalSaveStateFields)
                {
                    bundle.Data[kvp.Key] = kvp.Value;
                }
            }
            base.SaveStateToBundle(bundle);
        }
    }
}