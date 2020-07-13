// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace MvvmCross.UnitTest.Mocks.TestViewModels
{
    public class Test3ViewModel : MvxViewModel
    {
        public BundleObject? SaveStateBundleObject { get; set; }
        public Dictionary<string, string>? AdditionalSaveStateFields { get; set; }

        public BundleObject? SaveState()
        {
            return SaveStateBundleObject;
        }

        protected override ValueTask SaveStateToBundle(IMvxBundle? bundle)
        {
            if (bundle != null && AdditionalSaveStateFields != null)
            {
                foreach (var kvp in AdditionalSaveStateFields)
                {
                    bundle.Data[kvp.Key] = kvp.Value;
                }
            }
            return base.SaveStateToBundle(bundle);
        }
    }
}
