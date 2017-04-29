namespace MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates
{
    public class MvxDefaultTemplateSelector : MvxBaseTemplateSelector
    {
        public MvxDefaultTemplateSelector(int itemTemplateId)
        {
            ItemTemplateId = itemTemplateId;
        }

        public MvxDefaultTemplateSelector()
        {
        }

        public int ItemTemplateId { get; set; }

        protected override int GetItemViewType(object forItemObject)
        {
            return 0;
        }

        protected override int GetItemLayoutId(int fromViewType)
        {
            return ItemTemplateId;
        }
    }
}