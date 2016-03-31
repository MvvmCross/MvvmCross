using Example.Core.Model;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Example.Droid.Common.TemplateSelectors
{
    public class MultiItemTemplateModelTemplateSelector : ItemTemplateSelector<MultiItemTemplateModel>
    {
        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType == 0 ?
                Resource.Layout.listitem_recyclerviewmultiitem_titletemplate :
                Resource.Layout.listitem_recyclerviewmultiitem_descriptiontemplate;
        }

        protected override int SelectItemViewType(MultiItemTemplateModel forItemObject)
        {
            return forItemObject.Type == SampleItemType.TitleItem ? 0 : 1;
        }
    }
}