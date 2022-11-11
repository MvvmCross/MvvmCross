// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.DroidX.RecyclerView.ItemTemplates
{
    public abstract class MvxTemplateSelector<TItem> : IMvxTemplateSelector where TItem : class
    {
        public int ItemTemplateId { get; set; }

        public int GetItemViewType(object forItemObject)
        {
            return SelectItemViewType(forItemObject as TItem);
        }

        public abstract int GetItemLayoutId(int fromViewType);

        protected abstract int SelectItemViewType(TItem forItemObject);
    }
}
