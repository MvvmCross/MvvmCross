using Example.Core.Model;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Example.Droid.Common.TemplateSelectors
{
    public class MultiItemTemplateModelTemplateSelector : MvxTemplateSelector<MultiItemTemplateModel>
    {
        protected override int SelectItemViewType(MultiItemTemplateModel forItemObject)
        {
            return forItemObject.Type == SampleItemType.TitleItem ? 0 : 1;
        }

        protected override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType == 0 ?
            Resource.Layout.listitem_recyclerviewmultiitem_titletemplate :
            Resource.Layout.listitem_recyclerviewmultiitem_descriptiontemplate;
        }
    }
}