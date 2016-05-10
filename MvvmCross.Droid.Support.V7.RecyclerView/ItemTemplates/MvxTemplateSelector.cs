namespace MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates
{
    public abstract class MvxTemplateSelector<TItem> : IMvxTemplateSelector where TItem : class
    {
        public int GetItemViewType(object forItemObject)
        {
            return SelectItemViewType(forItemObject as TItem);
        }

        public abstract int GetItemLayoutId(int fromViewType);

        protected abstract int SelectItemViewType(TItem forItemObject);
    }
}