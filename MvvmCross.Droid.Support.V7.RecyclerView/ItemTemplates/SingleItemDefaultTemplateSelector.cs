namespace MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates
{
    public class SingleItemDefaultTemplateSelector : IItemTemplateSelector
    {
        public int ItemTemplateId { get; set; }

        public SingleItemDefaultTemplateSelector(int itemTemplateId)
        {
            ItemTemplateId = itemTemplateId;
        }

        public SingleItemDefaultTemplateSelector()
        {
        }

        public int GetItemViewType(object forItemObject)
            => 0;

        public int GetItemLayoutId(int fromViewType)
            => ItemTemplateId;
    }
}