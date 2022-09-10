// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.DroidX.RecyclerView.ItemTemplates
{
    //MvxDefaultTemplateSelector
    public class MvxDefaultTemplateSelector : IMvxTemplateSelector
    {
        public int ItemTemplateId { get; set; }

        public MvxDefaultTemplateSelector(int itemTemplateId)
        {
            ItemTemplateId = itemTemplateId;
        }

        public MvxDefaultTemplateSelector()
        {
        }

        public int GetItemViewType(object forItemObject)
            => 0;

        public int GetItemLayoutId(int fromViewType)
            => ItemTemplateId;
    }
}
