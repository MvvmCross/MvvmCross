using System;

namespace MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates
{
    public class MvxDefaultTemplateSelector : MvxBaseTemplateSelector
    {
        public int ItemTemplateId { get; set; }

        public MvxDefaultTemplateSelector(int itemTemplateId)
        {
            ItemTemplateId = itemTemplateId;
        }

        public MvxDefaultTemplateSelector()
        {
        }

        protected override int GetItemViewType(object forItemObject)
            => 0;

        protected override int GetItemLayoutId(int fromViewType)
            => ItemTemplateId;
    }
}