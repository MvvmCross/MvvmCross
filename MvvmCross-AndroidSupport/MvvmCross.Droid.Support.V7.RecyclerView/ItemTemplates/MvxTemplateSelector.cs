namespace MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates
{
    public abstract class MvxTemplateSelector<TItem> : MvxBaseTemplateSelector where TItem : class
    {
        protected override int GetItemViewType(object forItemObject)
        {
            return SelectItemViewType(forItemObject as TItem);
        }

        protected abstract int SelectItemViewType(TItem forItemObject);
    }
}