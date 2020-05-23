using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using static Playground.Core.ViewModels.CollectionViewModel;

namespace Playground.Droid.TemplateSelectors
{
    public class AnimalTemplateSelector : MvxTemplateSelector<AnimalViewModel>
    {
        public override int GetItemLayoutId(int fromViewType)
        {
            switch(fromViewType)
            {
                case 0:
                    return Resource.Layout.itemtemplate_cat;
                case 1:
                    return Resource.Layout.itemtemplate_dog;
                case 2:
                    return Resource.Layout.itemtemplate_monkey;
            }

            return -1;
        }

        protected override int SelectItemViewType(AnimalViewModel forItemObject)
        {
            switch(forItemObject)
            {
                case CatViewModel _:
                    return 0;
                case DogViewModel _:
                    return 1;
                case MonkeyViewModel _:
                    return 2;
            }

            return -1;
        }
    }
}
