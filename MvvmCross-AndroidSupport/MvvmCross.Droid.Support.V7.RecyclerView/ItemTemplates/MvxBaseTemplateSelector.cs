using System;
using MvvmCross.Droid.Support.V7.RecyclerView.Grouping;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemSources.Data;
using MvvmCross.Platform;

namespace MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates
{
    public abstract class MvxBaseTemplateSelector
    {
        public const int HeaderViewTypeId = Int32.MinValue;
        public const int FooterViewTypeId = Int32.MinValue + 1;
        public const int GroupSectionViewTypeId = Int32.MinValue + 2;

        public int GetViewType(object forItemObject)
        {
            if (forItemObject is MvxHeaderItemData)
                return HeaderViewTypeId;

            if (forItemObject is MvxFooterItemData)
                return FooterViewTypeId;

            if (forItemObject is MvxGroupedData)
                return GroupSectionViewTypeId;

            return GetItemViewType(forItemObject);
        }

        protected abstract int GetItemViewType(object forItemObject);

        public int GetLayoutId(int fromViewType)
        {
            if (fromViewType == HeaderViewTypeId)
                return HeaderLayoutId;

            if (fromViewType == FooterViewTypeId)
                return FooterLayoutId;

            if (fromViewType == GroupSectionViewTypeId)
            {
                if (!HasGroupSectionLayoutId)
                    Mvx.Error("You are binding to MvxGroupedData items, you have to set your Group Section layout! Use local:MvxGroupSectionLayoutId=@layout/sectionLayout on your MvxRecyclerView!");

                return GroupSectionLayoutId;
            }

            return GetItemLayoutId(fromViewType);
        }

        protected abstract int GetItemLayoutId(int fromViewType);

        public int HeaderLayoutId { get; set; } = 0;

        public int FooterLayoutId { get; set; } = 0;

        public int GroupSectionLayoutId { get; set; } = 0;

        public bool HasHeaderLayoutId => HeaderLayoutId != 0;

        public bool HasFooterLayoutId => FooterLayoutId != 0;

        public bool HasGroupSectionLayoutId => GroupSectionLayoutId != 0;

    }
}