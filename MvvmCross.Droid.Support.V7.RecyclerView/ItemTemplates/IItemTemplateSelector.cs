namespace MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates
{
    public interface IItemTemplateSelector
    {
        int GetItemViewType(object forItemObject);

        int GetItemLayoutId(int fromViewType);
    }
}